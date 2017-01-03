using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Resources;
using System.IO;
using System.Collections;
using System.Diagnostics;

using BOMBS.Core.Log;
using BOMBS.Core.Helper;

using Resources = BOMBS.Core.Resources;

namespace BOMBS.Service
{
    public class Controller
    {
        private const string resFile = "BOMBS.Service.Properties.Ports.Resources";
        private const string resTcp = "TcpPort";
        private const string resHttp = "HttpPort";
        private const string resEndPoint = "EndpointPort";

        private Controller()
        {
            resourceBuilder = Resources.Helper.Instance[ApplicationResourceDirectory.DefaultFolderName, resFile];
            resourceBuilder.Sender = this;

            if (!resourceBuilder.IsFileAvailable)
            {
                portTcp = 8511;
                portHttp = 1259;
                portEndPoint = 6268;

                WritePortToResources();
            }
            else ReadPortFromResources();
        }

        private ServiceHost host;

        private Resources.Builder resourceBuilder;

        private short portTcp;
        public short PortTcp
        {
            get { return portTcp; }
            set { portTcp = value; }
        }

        private short portHttp;
        public short PortHttp
        {
            get { return portHttp; }
            set { portHttp = value; }
        }

        private short portEndPoint;
        public short PortEndPoint
        {
            get { return portEndPoint; }
            set { portEndPoint = value; }
        }

        private static Controller communicator = null;
        public static Controller Communicator
        {
            get
            {
                if (communicator == null) communicator = new Controller();
                return communicator;
            }
        }

        public void CreateNewPorts(string[] args)
        {
            IEnumerator enumerator = args.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string str = (string)enumerator.Current;
                string[] arg = str.Split(':');

                if (string.Compare(arg[0], "tcp", true) == 0) portTcp = Convert.ToInt16(arg[1]);
                else if (string.Compare(arg[0], "http", true) == 0) portHttp = Convert.ToInt16(arg[1]);
                else if (string.Compare(arg[0], "endpoint", true) == 0) portEndPoint = Convert.ToInt16(arg[1]);
            }

            WritePortToResources();
        }

        private string GetPortValues()
        {
            return string.Format("(Tcp:{0} Http:{1} EndPoint:{2})", portTcp.ToString(), portHttp.ToString(), portEndPoint.ToString());
        }

        public void ReadPortFromResources()
        {
            if (resourceBuilder.Fill("Retreiving BOMBS Service Port Resource...", "BOMBS Service Port Resource Retreived Successfully..."))
            {
                portTcp = Convert.ToInt16(resourceBuilder[resTcp]);
                portHttp = Convert.ToInt16(resourceBuilder[resHttp]);
                portEndPoint = Convert.ToInt16(resourceBuilder[resEndPoint]);

                LogController.Instance.GetLogger(resourceBuilder.LogSource).LogNow(this, string.Format("Port Values from BOMBS Service Port Resource... {0}", GetPortValues()), EventLogEntryType.Information);
            }
        }

        public void WritePortToResources()
        {
            resourceBuilder[resTcp] = portTcp.ToString();
            resourceBuilder[resHttp] = portHttp.ToString();
            resourceBuilder[resEndPoint] = portEndPoint.ToString();

            string portValues = GetPortValues();
            resourceBuilder.Save(string.Format("Writing BOMBS Service Ports Resouce... {0}", portValues), string.Format("BOMBS Service Ports Resources has been written successfully... {0}", portValues));
        }

        public bool Start()
        {
            LogController.Instance.GetLogger(resourceBuilder.LogSource).LogNow(this, string.Format( "Starting BOMBS Service Communicator... {0}", GetPortValues()), EventLogEntryType.Information);
            Uri[] baseAddress = { new Uri(string.Format(@"net.tcp://localhost:{0}/BombsHost/", portTcp.ToString())), new Uri(string.Format(@"http://localhost:{0}/BombsHost/", portHttp.ToString())) };

            host = new ServiceHost(typeof(Communicator), baseAddress);

            NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None, true)
            {
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                TransferMode = TransferMode.Buffered,
                MaxConnections = int.MaxValue,
                ReceiveTimeout = new TimeSpan(20, 0, 0),
            };

            tcpBinding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            tcpBinding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            tcpBinding.ReaderQuotas.MaxStringContentLength = int.MaxValue;

            ServiceThrottlingBehavior throttle = host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior()
                {
                    MaxConcurrentCalls = int.MaxValue,
                    MaxConcurrentSessions = int.MaxValue
                };
                host.Description.Behaviors.Add(throttle);
            }

            tcpBinding.ReliableSession.Enabled = true;
            tcpBinding.ReliableSession.InactivityTimeout = new TimeSpan(20, 0, 10);

            host.AddServiceEndpoint(typeof(ICommunicator), tcpBinding, "tcp");
            host.Description.Behaviors.Add(new ServiceMetadataBehavior() { HttpGetEnabled = true });
            host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), string.Format("net.tcp://localhost:{0}/BombsHost/mex", portEndPoint.ToString()));

            host.Open();

            LogController.Instance.GetDefaultLogger().LogNow(this, "BOMBS Service Communicator Started Successfully...", EventLogEntryType.Information);
            return host.State == CommunicationState.Opened;
        }

        public bool Stop()
        {
            LogController.Instance.GetDefaultLogger().LogNow(this, "Stopping BOMBS Service Communicator...", EventLogEntryType.Information);
            host.Close();
            LogController.Instance.GetDefaultLogger().LogNow(this, "BOMBS Service Communicator Stopped successfully...", EventLogEntryType.Information);
            return host.State == CommunicationState.Closed;
        }
    }
}

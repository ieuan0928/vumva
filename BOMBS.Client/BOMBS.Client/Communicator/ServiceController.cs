using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

using BOMBS.Core.Common;
using BOMBS.Core.Helper;
using BOMBS.Core.Common.Handlers;

using BResources = BOMBS.Core.Resources;
using BSerialization = BOMBS.Core.Serialization;
using System.ComponentModel;
using System.Reflection;

namespace BOMBS.Client.Communicator
{
    public class ServiceController : ServerCommunication, BombsHost.ICommunicatorCallback
    {
        private const string resActiveServerFile = @"BOMBS.Client.Properties.Active.Server.Resources";

        private ServiceController()
        {
            InitializeTCPBinding();

            localPath = ApplicationResourceDirectory.LocalApplicationData;
            helper = BResources.Helper.Instance[localPath, resActiveServerFile];
        }

        public event NewValueHandler ConnectionStatusChanged;
        public event NewValueHandler ActiveServerInformationChanged;
        public event ResultHandler TestConnectionCompleted;
        public event ResultHandler NewConnectionImplemented;
        public event EventHandler HostInitializedAndOpened;

        private BackgroundWorker testConnectionWorker;
        private NetTcpBinding binding = null;
        private string localPath;
        private BResources.Builder helper;
        private Server.ServerVariables serverToConnect;

        private BombsHost.CommunicatorClient testResultHost;
        private BombsHost.CommunicatorClient connector;
        public BombsHost.CommunicatorClient Host
        {
            get { return connector; }
        }

        private Server.ServerCollection serverCollection = new Server.ServerCollection();
        public Server.ServerCollection ServerCollection
        {
            get { return serverCollection; }
        }

        private static ServiceController communicator = null;
        public static ServiceController Communicator
        {
            get
            {
                if (communicator == null) communicator = new ServiceController();
                return communicator;
            }
        }

        private Server.ConnectionStatus connectionStatus = Server.ConnectionStatus.NotFetched;
        public Server.ConnectionStatus ConnectionStatus
        {
            get { return connectionStatus; }
            set { OnConnectionStatusChange(value); }
        }

        private Server.ServerVariables activeServer = new Server.ServerVariables();
        public Server.ServerVariables ActiveServer
        {
            get { return activeServer; }
            set { activeServer = value; }
        }

        private void OnConnectionStatusChange(Server.ConnectionStatus newConnectionStatus)
        {
            Server.ConnectionStatus oldValue = connectionStatus;
            connectionStatus = newConnectionStatus;

            if (ConnectionStatusChanged != null)
                ConnectionStatusChanged.Invoke(this, new NewValueArgs("ConnectionStatus")
                        {
                            NewValue = connectionStatus,
                            OldValue = oldValue
                        }
                    );
        }

        private void InitializeTCPBinding()
        {
            binding = new NetTcpBinding()
            {
                Name = "BOMBS_Service_Communicator_TCPBinding",
                CloseTimeout = new TimeSpan(0, 1, 0),
                OpenTimeout = new TimeSpan(0, 1, 0),
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                SendTimeout = new TimeSpan(0, 1, 0),
                TransactionFlow = false,
                TransferMode = TransferMode.Buffered,
                TransactionProtocol = TransactionProtocol.OleTransactions,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                ListenBacklog = 10,
                MaxBufferPoolSize = int.MaxValue,
                MaxBufferSize = int.MaxValue,
                MaxConnections = 100,
                MaxReceivedMessageSize = int.MaxValue,
            };

            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            binding.ReliableSession.Ordered = true;

            binding.Security.Mode = System.ServiceModel.SecurityMode.None;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            binding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
            binding.ReliableSession.Enabled = true;
            binding.ReliableSession.InactivityTimeout = new TimeSpan(20, 10, 10);
        }

        public void TestConnection(Server.ServerVariables serverToConnectTest)
        {
            testConnectionWorker = new BackgroundWorker();
            testConnectionWorker.RunWorkerCompleted += testConnectionWorker_RunWorkerCompleted;
            testConnectionWorker.DoWork += testConnectionWorker_DoWork;
            testConnectionWorker.RunWorkerAsync(serverToConnectTest);

        }

        private void testConnectionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Server.ServerVariables serverToConnectTest = (Server.ServerVariables)e.Argument;
            BombsHost.CommunicatorClient testConnector = new BombsHost.CommunicatorClient(new InstanceContext(this), binding, new EndpointAddress(string.Format(@"net.tcp://{0}:{1}/BombsHost/tcp", serverToConnectTest.Address, serverToConnectTest.Port.ToString())));

            testConnector.Endpoint.Name = "BOMBS_Service_Communicator_EndPoint";

            bool result = true;

            try
            {
                testConnector.Open();

                result = true;
            }
            catch
            {
                result = false;
            }

            object[] results = new object[3];

            results[0] = result;
            results[1] = testConnector;
            results[2] = serverToConnectTest;

            e.Result = results;
        }

        private void testConnectionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            object[] result = (object[])e.Result;

            OnTestConnectionCompleted((bool)result[0], (BombsHost.CommunicatorClient)result[1], (Server.ServerVariables)result[2]);
        }

        private void OnTestConnectionCompleted(bool result, BombsHost.CommunicatorClient testCommunicator, Server.ServerVariables serverVariables)
        {
            serverToConnect = serverVariables;
            if (TestConnectionCompleted != null)
            {
                if (!result)
                {
                    TestConnectionCompleted(this, new ResultArg()
                    {
                        Result = new Server.TestConnectionVariablesArgs()
                        {
                            IsTestConnectionSuccessful = false,
                            ServerInformation = null
                        }
                    });
                }
                else
                {
                    if (testCommunicator != null)
                    {
                        testResultHost = testCommunicator;

                        testResultHost.GetServerInformationCompleted -= testCommunicator_GetServerInformationCompleted;
                        testResultHost.GetServerInformationCompleted += testCommunicator_GetServerInformationCompleted;

                        testResultHost.GetServerInformationAsync();
                    }
                }
            }
        }

        public void UseNewHost()
        {
            if (connector != null)
            {
                if (connector.State == CommunicationState.Opened)
                {
                    connector.DisconnectCompleted += connector_DisconnectCompleted;
                    connector.DisconnectAsync();
                }
                else
                {
                    connector.Close();
                    connector = testResultHost;
                    activeServer = serverToConnect;
                    if (HostInitializedAndOpened != null) HostInitializedAndOpened(this, EventArgs.Empty);
                }
            }
            else
            {
                connector = testResultHost;
                activeServer = serverToConnect;
                if (HostInitializedAndOpened != null) HostInitializedAndOpened(this, EventArgs.Empty);
            }
        }

        private void connector_DisconnectCompleted(object sender, AsyncCompletedEventArgs e)
        {
            connector.DisconnectCompleted -= connector_DisconnectCompleted;
            connector.Close();
            activeServer = serverToConnect;
            connector = testResultHost;

            if (HostInitializedAndOpened != null) HostInitializedAndOpened(this, EventArgs.Empty);
            if (NewConnectionImplemented != null) NewConnectionImplemented(this, new ResultArg() { Result = connector });
        }

        public void Connect()
        {
            connector = new BombsHost.CommunicatorClient(new InstanceContext(this), binding, new EndpointAddress(string.Format(@"net.tcp://{0}:{1}/BombsHost/tcp", activeServer.Address, activeServer.Port.ToString())));

            connector.Endpoint.Name = "BOMBS_Service_Communicator_EndPoint";
            try
            {
                connector.Open();
                OnConnectionStatusChange(Server.ConnectionStatus.Success);

                if (HostInitializedAndOpened != null) HostInitializedAndOpened(this, EventArgs.Empty);
            }
            catch
            {
                OnConnectionStatusChange(Server.ConnectionStatus.ConnectionFailure);
                return;
            }

            connector.GetServerInformationCompleted -= connector_GetServerInformationCompleted;
            connector.GetServerInformationCompleted += connector_GetServerInformationCompleted;

            connector.GetServerInformationAsync();
        }

        public void Start()
        {
            try
            {

                if (!ReadResources()) return;

                Connect();
            }
            catch
            {
                OnConnectionStatusChange(Server.ConnectionStatus.ConnectionFailure);
            }
        }

        public void Stop()
        {
            connector.Close();
        }

        private bool ReadResources()
        {
            if (!helper.IsFileAvailable)
            {
                OnConnectionStatusChange(Server.ConnectionStatus.ResourceFileNotFound);
                return false;
            }

            activeServer.Address = helper["Address"];
            activeServer.Port = Convert.ToInt16(helper["Port"]);
            activeServer.Information = BSerialization.XMLHelper.DeserializeFromXMLString<BombsHost.ServerInformation>(helper["Information"]);

            return true;
        }

        public void WriteResources()
        {
            helper["Address"] = activeServer.Address;
            helper["Port"] = activeServer.Port.ToString();
            helper["Information"] = BSerialization.XMLHelper.SerializeToXMLString<BombsHost.ServerInformation>(activeServer.Information);

            helper.Save();

            serverCollection[activeServer.DisplayName] = activeServer;
            serverCollection.WriteResources();
        }

        private void OnActiveServerInformationChanged(BombsHost.ServerInformation oldServerInformation, BombsHost.ServerInformation newServerInformation)
        {
            if (ActiveServerInformationChanged == null) return;

            ActiveServerInformationChanged(this, new NewValueArgs("Information")
                {
                    OldValue = oldServerInformation,
                    NewValue = newServerInformation
                });
        }

        private void connector_GetServerInformationCompleted(object sender, BombsHost.GetServerInformationCompletedEventArgs e)
        {
            try
            {
                if (activeServer.Information != e.Result)
                {
                    BombsHost.ServerInformation previousServerInformation = activeServer.Information;
                    activeServer.Information = e.Result;
                    WriteResources();
                    OnActiveServerInformationChanged(previousServerInformation, activeServer.Information);
                }
            }
            catch (TargetInvocationException) { }
        }

        private void testCommunicator_GetServerInformationCompleted(object sender, BombsHost.GetServerInformationCompletedEventArgs e)
        {
            if (TestConnectionCompleted != null)
                TestConnectionCompleted(this, new ResultArg()
                {
                    Result = new Server.TestConnectionVariablesArgs()
                    {
                        IsTestConnectionSuccessful = true,
                        ServerInformation = e.Result
                    }
                });
        }



    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using BOMBS.Core.Log;
namespace BOMBS.ServiceServer
{
    public partial class BombsService : ServiceBase
    {
        public BombsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    
                    LogController.Instance.GetDefaultLogger().LogNow(this, "Creating New Ports for BOMBS Service...", EventLogEntryType.Information);
                    BOMBS.Service.Controller.Communicator.CreateNewPorts(args);
                    LogController.Instance.GetDefaultLogger().LogNow(this, "New Ports for BOMBS Service Created Successfully...", EventLogEntryType.Information);
                }

                LogController.Instance.GetDefaultLogger().LogNow(this, "Starting BOMBS Service...", EventLogEntryType.Information);
                BOMBS.Service.Controller.Communicator.Start();
                LogController.Instance.GetDefaultLogger().LogNow(this, "BOMBS Service Started Successfully...", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogController.Instance.GetDefaultLogger().LogNow(this, string.Format("BOMBS Service Start Failure ({0} - {1})", ex.Message.ToString(), ex.StackTrace.ToString()), EventLogEntryType.Error);
            }
        }

        protected override void OnStop()
        {
            try
            {
                LogController.Instance.GetDefaultLogger().LogNow(this, "Stopping BOMBS Service...", EventLogEntryType.Information);
                BOMBS.Service.Controller.Communicator.Stop();
                LogController.Instance.GetDefaultLogger().LogNow(this, "BOMBS Service Stopped Successfully...", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                LogController.Instance.GetDefaultLogger().LogNow(this, string.Format("BOMBS Service Stop Failure ({0} - {1})", ex.Message.ToString(), ex.StackTrace.ToString()), EventLogEntryType.Error);
            }
        }
    }
}

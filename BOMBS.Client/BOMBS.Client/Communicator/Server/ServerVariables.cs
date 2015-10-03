using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BOMBS.Client.Communicator.Server
{
    public class ServerVariables : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string address = string.Empty;
        public string Address
        {
            get { return address; }
            set
            {
                if (address != value)
                {
                    address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        private short port = 0;
        public short Port
        {
            get { return port; }
            set
            {
                if (port != value)
                {
                    port = value;
                    OnPropertyChanged("Port");
                }
            }
        }

        private BombsHost.ServerInformation information;
        public BombsHost.ServerInformation Information
        {
            get { return information; }
            set
            {
                if (information != value)
                {
                    information = value;
                    OnPropertyChanged("Information");
                }
            }
        }


        private ServerConfigurationStatus serverConfigurationStatus;
        public ServerConfigurationStatus ServerConfigurationStatus
        {
            get
            {
                ServiceController communicator =  ServiceController.Communicator;
               
                serverConfigurationStatus = communicator.ActiveServer.DisplayName == DisplayName ? communicator.ConnectionStatus == ConnectionStatus.ConnectionFailure ? ServerConfigurationStatus.NoConnection : ServerConfigurationStatus.Active : ServerConfigurationStatus.Unset;

                return serverConfigurationStatus;
            }
            set
            {
                if (serverConfigurationStatus != value)
                {
                    serverConfigurationStatus = value;
                    OnPropertyChanged("IsSetToDefault");
                }
            }
        }

        public string DisplayName
        {
            get { return string.Format("{0}:{1}", address, port.ToString()); }
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BOMBS.Client.Communicator
{
    namespace Database
    {
        public class Settings : INotifyPropertyChanged
        {
            private BombsHost.DatabaseSQLServerOption databaseSQLServerOption = BombsHost.DatabaseSQLServerOption.UseLocalhost;
            public BombsHost.DatabaseSQLServerOption DatabaseSQLServerOption
            {
                get { return databaseSQLServerOption; }
                set
                {
                    if (databaseSQLServerOption != value)
                    {
                        databaseSQLServerOption = value;
                        OnPropertyChanged("DatabaseSQLServerOption");

                        switch (databaseSQLServerOption)
                        {
                            case BombsHost.DatabaseSQLServerOption.UseLocalhost:
                                ServerAddress = "localhost";
                                break;
                            case BombsHost.DatabaseSQLServerOption.UseServerConnectionConfiguration:
                                ServerAddress = Communicator.ServiceController.Communicator.ActiveServer.Address;
                                break;
                            case BombsHost.DatabaseSQLServerOption.UseServerComputerName:
                                ServerAddress = Communicator.ServiceController.Communicator.ActiveServer.Information.ComputerName;
                                break;
                            case BombsHost.DatabaseSQLServerOption.UseServerFullComputerName:
                                ServerAddress = Communicator.ServiceController.Communicator.ActiveServer.Information.FullComputerName;
                                break;
                            case BombsHost.DatabaseSQLServerOption.UseServersIPAddress:
                                ServerAddress = Communicator.ServiceController.Communicator.ActiveServer.Information.IPAddress;
                                break;
                            case BombsHost.DatabaseSQLServerOption.FillInServerOrIPAddress:
                                ServerAddress = fillInServerIpAddressValue;
                                break;
                        }
                    }
                }
            }

            private string fillInServerIpAddressValue = string.Empty;
            public string FillInServerIpAddressValue
            {
                get { return fillInServerIpAddressValue; }
                set { fillInServerIpAddressValue = value; }
            }

            private string databaseName = string.Empty;
            public string DatabaseName
            {
                get { return databaseName; }
                set
                {
                    if (databaseName != value)
                    {
                        databaseName = value;
                        OnPropertyChanged("DatabaseName");
                    }
                }
            }

            private string serverInstanceName = string.Empty;
            public string ServerInstanceName
            {
                get { return serverInstanceName; }
                set
                {
                    if (serverInstanceName != value)
                    {
                        serverInstanceName = value;
                        OnPropertyChanged("ServerInstanceName");
                    }
                }
            }

            private string sqlServerPortString = string.Empty;
            public string SqlServerPortString
            {
                get { return sqlServerPortString; }
                set
                {
                    if (sqlServerPortString != value)
                    {
                        sqlServerPortString = value;
                        OnPropertyChanged("SqlServerPortString");
                    }
                }
            }

            private string serverAddress = "localhost";
            public string ServerAddress
            {
                get { return serverAddress; }
                set
                {
                    if (serverAddress != value)
                    {
                        serverAddress = value;
                        OnPropertyChanged("ServerAddress");
                    }
                }
            }

            private BombsHost.DatabaseAuthentication databaseAuthentication = BombsHost.DatabaseAuthentication.Windows;
            public BombsHost.DatabaseAuthentication DatabaseAuthentication
            {
                get { return databaseAuthentication; }
                set
                {
                    if (databaseAuthentication != value)
                    {
                        databaseAuthentication = value;
                        OnPropertyChanged("DatabaseAuthentication");
                    }
                }
            }

            private bool useWindowsAuthentication = false;
            public bool UseWindowsAuthentication
            {
                get { return useWindowsAuthentication; }
                set
                {
                    if (useWindowsAuthentication != value)
                    {
                        useWindowsAuthentication = value;
                        OnPropertyChanged("UseWindowsAuthentication");

                        DatabaseAuthentication = useWindowsAuthentication ? BombsHost.DatabaseAuthentication.Windows : BombsHost.DatabaseAuthentication.SQLServer;
                    }
                }
            }

            private string userName = string.Empty;
            public string UserName
            {
                get { return userName; }
                set
                {
                    if (userName != value)
                    {
                        userName = value;
                        OnPropertyChanged("UserName");
                    }
                }
            }

            private string password = string.Empty;
            public string Password
            {
                get { return password; }
                set
                {
                    if (password != value)
                    {
                        password = value;
                        OnPropertyChanged("Password");
                    }
                }
            }

            private string confirmPassword = string.Empty;
            public string ConfirmPassword
            {
                get { return confirmPassword; }
                set
                {
                    if (confirmPassword != value)
                    {
                        confirmPassword = value;
                        OnPropertyChanged("ConfirmPassword");
                    }
                }
            }

            private Server.ServerVariables serverVariable;
            public Server.ServerVariables ServerConnection
            {
                get { return serverVariable; }
                set
                {
                    serverVariable = value;
                    OnPropertyChanged("ServerConnection");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            public void Populate(BombsHost.DatabaseInformation sourceInfo)
            {
                if (string.IsNullOrEmpty(sourceInfo.Password))
                {
                    this.UseWindowsAuthentication = false;
                }
                else
                {
                    this.UseWindowsAuthentication = true;
                    this.ConfirmPassword = sourceInfo.Password;
                    this.Password = sourceInfo.Password;
                }
                this.DatabaseAuthentication = sourceInfo.DatabaseAuthentication;
                this.DatabaseName = sourceInfo.DatabaseName;
                this.DatabaseSQLServerOption = sourceInfo.SqlServerOption;
                this.ServerAddress = sourceInfo.ServerAddress;
                this.ServerInstanceName = sourceInfo.ServerInstanceName;
                this.SqlServerPortString = sourceInfo.SQLServerPortString;
                this.UserName = sourceInfo.UserName;
            }
        }
    }
}

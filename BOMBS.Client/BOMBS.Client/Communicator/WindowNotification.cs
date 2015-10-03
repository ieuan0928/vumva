using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using BOMBS.UI.Foundation.Utilities;
using System.Windows.Threading;

namespace BOMBS.Client.Communicator
{
    public class WindowNotification : Window
    {
        public WindowNotification()
        {
            Unloaded += WindowNotification_Unloaded;
            ContentRendered += WindowNotification_ContentRendered;
            communicator = ServiceController.Communicator;

            communicator.HostInitializedAndOpened += communicator_HostInitializedAndOpened;

            if (communicator.Host != null) communicator.Host.GetServerInformationCompleted += CommunicatorHost_GetServerInformationCompleted;

            communicator.DatabaseStatusOnChanged += Communicator_DatabaseStatusOnChanged;
            communicator.ConfigureDatabaseStepsOnChanged += communicator_ConfigureDatabaseStepsOnChanged;
        }

        protected bool isConnectionStatusRetreived = false;
        protected bool isDatabaseValidated = false;
        protected bool isValidationDatabaseConfigurationFailed = false;

        private void WindowNotification_ContentRendered(object sender, EventArgs e)
        {
            ServiceController.Communicator.ValidationDatabaseConfigurationFailed += Communicator_ValidationDatabaseConfigurationFailed;
        }
        
        private void Communicator_ValidationDatabaseConfigurationFailed(object sender, EventArgs e)
        {
            isValidationDatabaseConfigurationFailed = true;

            if (!isDatabaseValidated)
            {
                isDatabaseValidated = true;
                HideBusyMessage();
                Database.Dialogs.ViewServerDatabaseSettings();
            }
        }

        private void communicator_ConfigureDatabaseStepsOnChanged(object sender, Core.Common.Handlers.ResultArg e)
        {
            Dispatcher.BeginInvoke(new Action(delegate()
            {
                switch ((BombsHost.DatabaseConfigurationSteps)e.Result)
                {
                    case BombsHost.DatabaseConfigurationSteps.ConfigurationFailure:
                        Application.Current.MainWindow.Activate();
                        overlappingBusyMessage.Hide();
                        MessageBox.Show("Configuration of Database failed.", "Information", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                    case BombsHost.DatabaseConfigurationSteps.ConfigurationCancelled:
                        Application.Current.MainWindow.Activate();
                        overlappingBusyMessage.Hide();
                        MessageBox.Show("Configuration of Database has been cancelled.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }));
        }

        private void communicator_HostInitializedAndOpened(object sender, EventArgs e)
        {
            communicator.Host.GetServerInformationCompleted -= CommunicatorHost_GetServerInformationCompleted;
            communicator.Host.GetServerInformationCompleted += CommunicatorHost_GetServerInformationCompleted;
        }

        private ServiceController communicator;
        private BombsHost.DatabaseInformation previousDatabaseInformation;
        protected OverlappingBusyMessage overlappingBusyMessage = new OverlappingBusyMessage();

        protected void ShowBusyMessage(string Message, string Title)
        {
            HideBusyMessage();

            overlappingBusyMessage.BusyMessage = Message;
            overlappingBusyMessage.BusyTitle = Title;
            overlappingBusyMessage.Show();
        }

        protected void HideBusyMessage()
        {
            overlappingBusyMessage.Hide();

            UpdateLayout();
        }

        private void InvokeValidateDatabaseStatus(BombsHost.DatabaseStatus databaseStatus)
        {
            if (databaseStatus != BombsHost.DatabaseStatus.Ready)
            {
                Dispatcher.BeginInvoke(new Action(delegate()
                {
                    switch (databaseStatus)
                    {
                        case BombsHost.DatabaseStatus.ConfigurationOnProgress:
                            overlappingBusyMessage.BusyMessage = Properties.Resources.DatabaseStatus_ConfigurationOnProgress;
                            overlappingBusyMessage.Show();
                            break;
                        case BombsHost.DatabaseStatus.DatabaseErrorConfiguration:
                            Application.Current.MainWindow.Activate();
                            overlappingBusyMessage.Hide();
                            MessageBox.Show(Properties.Resources.DatabaseStatus_DatabaseErrorConfiguration, "Error", MessageBoxButton.OK, MessageBoxImage.Information);

                            break;

                    }
                }));
            }
        }

        private void CommunicatorHost_GetServerInformationCompleted(object sender, BombsHost.GetServerInformationCompletedEventArgs e)
        {
            InvokeValidateDatabaseStatus(((BombsHost.ServerInformation)e.Result).DatabaseInformation.Status);
        }

        private void Communicator_DatabaseStatusOnChanged(object sender, Database.ConnectdDatabaseOnProgressArguments e)
        {
            previousDatabaseInformation = e.PreviousDatabaseInformation;

            InvokeValidateDatabaseStatus(e.NewStatus);
        }

        private void WindowNotification_Unloaded(object sender, RoutedEventArgs e)
        {
            communicator.Host.GetServerInformationCompleted += CommunicatorHost_GetServerInformationCompleted;
            communicator.DatabaseStatusOnChanged -= Communicator_DatabaseStatusOnChanged;
        }
    }
}

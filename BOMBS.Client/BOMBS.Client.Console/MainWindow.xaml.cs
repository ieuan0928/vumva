using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;

using BOMBS.Client;
using BOMBS.Core.Common;
using BOMBS.Core.Common.Handlers;
using BOMBS.UI.Foundation.Utilities;
using BOMBS.Client.Communicator;

using ClientServer = BOMBS.Client.Communicator.Server;
using Database = BOMBS.Client.Communicator.Database;
using BOMBS.UI.Foundation;

namespace BOMBS.Client.Console
{
    public partial class MainWindow : WindowNotification
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceController.Communicator.ValidationDatabaseConfigurationFailed += Communicator_ValidationDatabaseConfigurationFailed;
            ServiceController.Communicator.ValidationDatabaseConfigurationSuccess += Communicator_ValidationDatabaseConfigurationSuccess;

            overlappingBusyMessage.ControlToLock = layoutGrid;
            ShowBusyMessage("Intiatilizing Back Office Management Business System.", "Please wait.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BOMBS.Client.Communicator.Server.Dialogs.ViewRegisteredConnection();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Database.Dialogs.ViewServerDatabaseSettings();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            ServiceController.Communicator.ConnectionStatusChanged += Communicator_ConnectionStatusChanged;

            (Application.Current as App).BSplashScreen.AllowClose = true;

            if (ServiceController.Communicator.ConnectionStatus != ClientServer.ConnectionStatus.NotFetched && !isConnectionStatusRetreived)
            {
                isConnectionStatusRetreived = true;

                ValidateConnectionStatus(ServiceController.Communicator.ConnectionStatus);
            }

            //ValidateDatabaseStatus();
        }

        private void Communicator_ValidationDatabaseConfigurationSuccess(object sender, EventArgs e)
        {
            isValidationDatabaseConfigurationFailed = false;

            if (isDatabaseValidated) HideBusyMessage();
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

        private void ValidateDatabaseStatus()
        {
            if (isDatabaseValidated) return;
            isDatabaseValidated = true;
            BombsHost.ServerInformation serverInformation = ServiceController.Communicator.ActiveServer.Information;
            if (serverInformation == null) return;

            BombsHost.DatabaseStatus DatabaseStatus = serverInformation.DatabaseInformation.Status;

            if (!isValidationDatabaseConfigurationFailed && DatabaseStatus != BombsHost.DatabaseStatus.ValidatingConfiguration &&
                    DatabaseStatus != BombsHost.DatabaseStatus.ConfigurationRequiresValidation &&
                    DatabaseStatus != BombsHost.DatabaseStatus.RequiresConfiguration)
                HideBusyMessage();
            else if ((DatabaseStatus == BombsHost.DatabaseStatus.ValidatingConfiguration || DatabaseStatus == BombsHost.DatabaseStatus.ConfigurationRequiresValidation) 
                    && !isValidationDatabaseConfigurationFailed)
            {
                ShowBusyMessage("Validation of Database Configuration is in progress.", "Validating Database Configuration");
                Database.Dialogs.ViewServerDatabaseSettings();
            }
            else if ((DatabaseStatus != BombsHost.DatabaseStatus.Ready && DatabaseStatus != BombsHost.DatabaseStatus.ConfigurationOnProgress) || isValidationDatabaseConfigurationFailed)
            {
                if (DatabaseStatus != BombsHost.DatabaseStatus.RequiresConfiguration) HideBusyMessage();
                else BusyMessageControl.BusyMessage = "Database Requires Configuration!";

                Database.Dialogs.ViewServerDatabaseSettings();
            }
        }
         
        private void ValidateConnectionStatus(ClientServer.ConnectionStatus connectionStatus)
        {
            switch (connectionStatus)
            {
                case ClientServer.ConnectionStatus.ResourceFileNotFound:
                    ClientServer.Dialogs.CreateNewConnection();
                    ValidateDatabaseStatus();
                    break;
                case ClientServer.ConnectionStatus.ConnectionFailure:
                    ClientServer.Dialogs.ViewRegisteredConnection();
                    break;
                default:
                    ValidateDatabaseStatus();
                    break;
            }
        }

        private void Communicator_ConnectionStatusChanged(object sender, NewValueArgs e)
        {
            if (!isConnectionStatusRetreived) ValidateConnectionStatus((ClientServer.ConnectionStatus)e.NewValue);

            ValidateDatabaseStatus();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //close
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            WizardWindow wwTest = new WizardWindow();

            wwTest.ShowDialog();
        }

    }
}

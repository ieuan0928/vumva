using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using BOMBS.UI.Foundation.Controls;
using BOMBS.UI.Foundation.Controls.ValidationRule;

namespace BOMBS.Client.Communicator.Database
{
    public partial class Control : Communicator.ContentNotification, IDialogContent
    {
        public Control()
        {
            InitializeComponent();

            ServiceController.Communicator.ConfigureDatabaseStepsOnChanged += Communicator_ConfigureDatabaseStepsOnChanged;

            DataContext = data;

            userNameTextBoxBinding = userNameTextBox.GetBindingExpression(TextBox.TextProperty);
            passwordBoxBinding = passwordBox.GetBindingExpression(PasswordBoxExtension.PasswordProperty);
            confirmPasswordBoxBinding = confirmPasswordBox.GetBindingExpression(PasswordBoxExtension.PasswordProperty);
            serverIpAddressTextBoxBinding = serverIpAddressTextBox.GetBindingExpression(TextBox.TextProperty);

            serverConfigurationGrid.DataContext = Communicator.ServiceController.Communicator.ActiveServer;
        }

        private Database.Settings data = new Database.Settings();

        private BindingExpression userNameTextBoxBinding;
        private BindingExpression passwordBoxBinding;
        private BindingExpression confirmPasswordBoxBinding;
        private BindingExpression serverIpAddressTextBoxBinding;

        public override void LoadState()
        {
            buttonCollection.IsOkButtonEnabled = false;
        }

        private void useSqlServerAuthentication()
        {
            userNameTextBoxBinding.ParentBinding.ValidationRules.Add(new RequiredRule() { RequiredRuleErrorMessage = "User Name is required." });

            PasswordBoxExtension.SetAttach(passwordBox, true);
            passwordBoxBinding.ParentBinding.ValidationRules.Add(new PasswordRule()
            {
                InvalidValueComparisonMessage = "Password does not match!",
                RequiredRuleErrorMessage = "Password is required.",
                ValueToCompare = data.ConfirmPassword
            });

            PasswordBoxExtension.SetAttach(confirmPasswordBox, true);
            confirmPasswordBoxBinding.ParentBinding.ValidationRules.Add(new PasswordRule()
            {
                InvalidValueComparisonMessage = "Password does not match!",
                RequiredRuleErrorMessage = "Confirmation of Password is required.",
                ValueToCompare = data.Password
            });

            DatabaseLoginSettingsUpdateTarget();

            data.DatabaseAuthentication = BombsHost.DatabaseAuthentication.SQLServer;
        }

        private void useWindowsAuthentication()
        {
            data.UserName = userNameTextBox.Text;
            userNameTextBoxBinding.ParentBinding.ValidationRules.Clear();

            PasswordBoxExtension.SetAttach(passwordBox, false);
            passwordBoxBinding.ParentBinding.ValidationRules.Clear();

            PasswordBoxExtension.SetAttach(confirmPasswordBox, false);
            confirmPasswordBoxBinding.ParentBinding.ValidationRules.Clear();

            DatabaseLoginSettingsUpdateTarget();

            data.DatabaseAuthentication = BombsHost.DatabaseAuthentication.Windows;
        }

        private void DatabaseLoginSettingsUpdateTarget()
        {
            userNameTextBoxBinding.UpdateTarget();

            passwordBoxBinding.UpdateTarget();
            confirmPasswordBoxBinding.UpdateTarget();
        }

        private void DialogContent_Loaded(object sender, RoutedEventArgs e)
        {
            if ((bool)useSqlServerAuthenticationRadioButton.IsChecked) useSqlServerAuthentication();
            else useWindowsAuthentication();
        }

        private void useSqlServerAuthenticationRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) useSqlServerAuthentication();
        }

        private void useSqlServerAuthenticationRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (IsLoaded) useWindowsAuthentication();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            if (confirmPasswordBoxBinding.ParentBinding.ValidationRules.Count > 0)
            {
                (confirmPasswordBoxBinding.ParentBinding.ValidationRules[0] as PasswordRule).ValueToCompare = passwordBox.Password;
                confirmPasswordBoxBinding.UpdateSource();
            }
        }

        private void confirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            if (passwordBoxBinding.ParentBinding.ValidationRules.Count > 0)
            {
                (passwordBoxBinding.ParentBinding.ValidationRules[0] as PasswordRule).ValueToCompare = confirmPasswordBox.Password;
                passwordBoxBinding.UpdateSource();
            }
        }

        private void connectDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            ShowBusyMessage("Attempting to connect on BOMBS Service Database");
            BombsHost.CommunicatorClient service = ServiceController.Communicator.Host;

            service.ConnectDatabaseAsync(new BombsHost.DatabaseInformation()
            {
                DatabaseAuthentication = data.DatabaseAuthentication,
                DatabaseName = data.DatabaseName,
                Password = data.Password,
                ServerAddress = data.ServerAddress,
                ServerConnectionConfiguration = serverNameTextBlock.Text.Trim(),
                ServerInstanceName = data.ServerInstanceName,
                SQLServerPortString = data.SqlServerPortString,
                UserName = data.UserName
            });
        }

        private void isSetServerOrIPaddress(bool isServerIPAddressEnabled)
        {
            serverIpAddressLabel.IsEnabled = isServerIPAddressEnabled;
            serverIpAddressColon.IsEnabled = isServerIPAddressEnabled;
            serverIpAddressTextBox.IsEnabled = isServerIPAddressEnabled;

            if (isServerIPAddressEnabled)
            {
                data.DatabaseSQLServerOption = BombsHost.DatabaseSQLServerOption.FillInServerOrIPAddress;
                serverIpAddressTextBoxBinding.ParentBinding.ValidationRules.Add(new RequiredRule() { RequiredRuleErrorMessage = "Server or IP Address is required." });
            }
            else
            {
                data.FillInServerIpAddressValue = serverIpAddressTextBox.Text.Trim();
                serverIpAddressTextBoxBinding.ParentBinding.ValidationRules.Clear();
            }


            serverIpAddressTextBoxBinding.UpdateSource();
        }

        private void sqlServerMachineOptions_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            RadioButton senderRadioButton = sender as RadioButton;

            if (senderRadioButton == localhostRadioButton) data.DatabaseSQLServerOption = BombsHost.DatabaseSQLServerOption.UseLocalhost;
            else if (senderRadioButton == serverConnectionConfigurationRadioButton) data.DatabaseSQLServerOption = BombsHost.DatabaseSQLServerOption.UseServerConnectionConfiguration;
            else if (senderRadioButton == serverComputerNameRadioButton) data.DatabaseSQLServerOption = BombsHost.DatabaseSQLServerOption.UseServerComputerName;
            else if (senderRadioButton == serverFullComputerNameRadioButton) data.DatabaseSQLServerOption = BombsHost.DatabaseSQLServerOption.UseServerFullComputerName;
            else if (senderRadioButton == serversIpAddressRadioButton) data.DatabaseSQLServerOption = BombsHost.DatabaseSQLServerOption.UseServersIPAddress;
            else if (senderRadioButton == fillInServerOrIpAddressRadioButton) isSetServerOrIPaddress(true);
        }

        private void sqlServerMachineOptions_UnChecked(object sender, RoutedEventArgs e)
        {
            if (!IsLoaded) return;

            RadioButton senderRadioButton = sender as RadioButton;

            if (senderRadioButton == fillInServerOrIpAddressRadioButton) isSetServerOrIPaddress(false);
        }

        private void Communicator_ConfigureDatabaseStepsOnChanged(object sender, Core.Common.Handlers.ResultArg e)
        {
            Dispatcher.BeginInvoke(new Action(delegate()
            {
                BombsHost.DatabaseConfigurationSteps configurationStep = (BombsHost.DatabaseConfigurationSteps)e.Result;

                switch (configurationStep)
                {
                    case BombsHost.DatabaseConfigurationSteps.InstanceFailure:
                        MessageBox.Show("Error occured on SQL Server Connection. \n\n\nPossible Issue:\n\n    Target Machine does not have SQL Server installed.\n    Invalid Database Settings.\n    Authentication is invalid.\n    ", "Database Configuration Failure", MessageBoxButton.OK, MessageBoxImage.Question);
                        HideBusyMessage();
                        break;
                    case BombsHost.DatabaseConfigurationSteps.DatabaseNotAvailable:
                        MessageBoxResult result = MessageBox.Show("Database is not available. \n\nWould you like to create the database now?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        BombsHost.CommunicatorClient communicator = ServiceController.Communicator.Host;
                        if (result == MessageBoxResult.Yes)
                        {
                            communicator.PushDatabaseConfigurationCompleted += communicatorDoNothing_PushDatabaseConfigurationCompleted;
                            communicator.PushDatabaseConfigurationAsync(BombsHost.DatabaseConfigurationSteps.CreateDatabase);
                        }
                        else
                        {
                            communicator.PushDatabaseConfigurationCompleted += communicatorHideMessage_PushDatabaseConfigurationCompleted;
                            communicator.PushDatabaseConfigurationAsync(BombsHost.DatabaseConfigurationSteps.ConfigurationCancel);
                        }
                        break;
                    case BombsHost.DatabaseConfigurationSteps.Completed:
                        MessageBox.Show("Database Configuration has been created successfully.", "Database Configuration", MessageBoxButton.OK, MessageBoxImage.Information);
                        HideBusyMessage();
                        break;
                }
            }));
        }

        private void communicatorDoNothing_PushDatabaseConfigurationCompleted(object sender, BombsHost.PushDatabaseConfigurationCompletedEventArgs e)
        {
            ((BombsHost.CommunicatorClient)sender).PushDatabaseConfigurationCompleted -= communicatorDoNothing_PushDatabaseConfigurationCompleted;
        }

        private void communicatorHideMessage_PushDatabaseConfigurationCompleted(object sender, BombsHost.PushDatabaseConfigurationCompletedEventArgs e)
        {
            ((BombsHost.CommunicatorClient)sender).PushDatabaseConfigurationCompleted -= communicatorHideMessage_PushDatabaseConfigurationCompleted;
            Dispatcher.BeginInvoke(new Action(delegate() { HideBusyMessage(); }));
        }
    }
}

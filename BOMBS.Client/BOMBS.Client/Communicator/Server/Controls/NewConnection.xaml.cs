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

using BOMBS.Client.Communicator.Server;
using BOMBS.UI.Foundation.Controls;
using BOMBS.UI.Foundation.Utilities;
using BOMBS.UI.Foundation.Controls.Dialog;
using BOMBS.Core.Common.Handlers;
using System.Windows.Threading;
using System.ComponentModel;

namespace BOMBS.Client.Communicator.Server.Controls
{
    public partial class NewConnection : Communicator.ContentNotification, IDialogContent
    {
        public NewConnection()
        {
            InitializeComponent();

            communicator = ServiceController.Communicator;

            serverVariables = new ServerVariables();
            DataContext = serverVariables;
            OnDialogContentRequest += NewConnection_OnDialogContentRequest;
        }

        private ServiceController communicator;

        private void NewConnection_OnDialogContentRequest(object sender, DialogContentRequestEventArgs e)
        {
            if (e.RequestType == DialogContentRequestType.Ok)
            {
                if (MessageBox.Show(string.Format(@"Would you like to connect to this server?{0}{1}{2}", Environment.NewLine, Environment.NewLine, communicator.ActiveServer.DisplayName), "Connection Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

                communicator.WriteResources();

                DoDialogRequest(DialogContentRequestType.Close);
            }
        }

        private ServerVariables serverVariables;
        public ServerVariables ServerConfiguration { get { return serverVariables; } }

        public override void LoadState()
        {
            buttonCollection.IsOkButtonEnabled = false;
        }

        private void PopulateServerInformation()
        {
            BombsHost.ServerInformation serverInformation = serverVariables.Information;

            computerNameTextBlock.SetValue(TextBlock.TextProperty, serverInformation.ComputerName);
            fullComputerNameTextBlock.SetValue(TextBlock.TextProperty, serverInformation.FullComputerName);
            hostNameTextBlock.SetValue(TextBlock.TextProperty, serverInformation.HostName);
            domainNameTextBlock.SetValue(TextBlock.TextProperty, serverInformation.DomainName);
            ipAddressTextBlock.SetValue(TextBlock.TextProperty, serverInformation.IPAddress);
        }

        private void ClearServerInformation()
        {
            if (serverVariables.Information == null) return;

            serverVariables.Information = null;

            computerNameTextBlock.ClearValue(TextBlock.TextProperty);
            fullComputerNameTextBlock.ClearValue(TextBlock.TextProperty);
            hostNameTextBlock.ClearValue(TextBlock.TextProperty);
            domainNameTextBlock.ClearValue(TextBlock.TextProperty);
            ipAddressTextBlock.ClearValue(TextBlock.TextProperty);
        }

        private void communicator_TestConnectionCompleted(object sender, ResultArg e)
        {
            HideBusyMessage();
            communicator.TestConnectionCompleted -= communicator_TestConnectionCompleted;

            Server.TestConnectionVariablesArgs result = ((Server.TestConnectionVariablesArgs)e.Result);
            bool isTestConnectionSuccessful = result.IsTestConnectionSuccessful;
            buttonCollection.IsOkButtonEnabled = isTestConnectionSuccessful;

            if (isTestConnectionSuccessful)
            {
                MessageBox.Show("Connection Successful", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                serverVariables.Information = result.ServerInformation;
                PopulateServerInformation();
            }
            else
            {
                ClearServerInformation();
                MessageBox.Show("Invalid Connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                serverAddressTextBox.Focus();
            }
        }

        private void testConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            communicator.TestConnectionCompleted += communicator_TestConnectionCompleted;

            ShowBusyMessage("Attempting to connect on BOMBS Server.");

            communicator.TestConnection(serverVariables);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            serverAddressTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            portTextBox.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
        }

        private void testConnectionButton_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!testConnectionButton.IsEnabled && IsLoaded)
            {
                ClearServerInformation();
                buttonCollection.IsOkButtonEnabled = false;
            }
        }
    }
}

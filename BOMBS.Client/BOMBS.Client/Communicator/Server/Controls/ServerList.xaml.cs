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
using System.ComponentModel;

using BOMBS.UI.Foundation.Controls;
using BOMBS.Client.Communicator.Server;


namespace BOMBS.Client.Communicator.Server.Controls
{
    public partial class ServerList : Communicator.ContentNotification, IDialogContent
    {
        public ServerList()
        {
            InitializeComponent();

            communicator = ServiceController.Communicator;

            serverCollection = ServiceController.Communicator.ServerCollection;
            registeredServersListView.ItemsSource = serverCollection;
            if (serverCollection.Count > 0) SetRegisteredServersListVeiewSelectedItemToDefault();
        }

        private ServerCollection serverCollection;
        private Communicator.ServiceController communicator;
        private ICollectionView registeredServersView;

        private bool isSetToActive = false;

        private void SetRegisteredServersListVeiewSelectedItemToDefault()
        {
            registeredServersListView.SelectedItem = serverCollection.FirstOrDefault(itm => itm.Value.ServerConfigurationStatus != ServerConfigurationStatus.Unset);
            SetButtonEnable(true);
        }

        private void RefreshRegisteredServersView()
        {
            if (registeredServersView == null)
                registeredServersView = CollectionViewSource.GetDefaultView(registeredServersListView.ItemsSource);

            registeredServersView.Refresh();
        }
        private void SetButtonEnable(bool isButtonEnable)
        {
            addNewServerConnectionConfigurationButton.IsEnabled = isButtonEnable;
            setToCurrentActiveServerConfigurationButton.IsEnabled = isButtonEnable;
            testConnectionToRetrieveOrReloadServerConfigurationButton.IsEnabled = isButtonEnable;
        }

        private void TestConnection()
        {
            communicator.TestConnectionCompleted += communicator_TestConnectionCompleted;
            Server.ServerVariables serverVariables = ((KeyValuePair<string, ServerVariables>)registeredServersListView.SelectedItem).Value;
            ShowBusyMessage("Attempting to connet to BOMBS Server.");
            communicator.TestConnection(serverVariables);
        }

        private void registeredServersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataContext = registeredServersListView.SelectedItem;
        }

        private void addNewServerConnectionConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonEnable(false);
            if (Server.Dialogs.CreateNewConnection(false))
            {
                RefreshRegisteredServersView();
                SetRegisteredServersListVeiewSelectedItemToDefault();
            }
        }

        private void communicator_TestConnectionCompleted(object sender, Core.Common.Handlers.ResultArg e)
        {
            HideBusyMessage();

            communicator.TestConnectionCompleted -= communicator_TestConnectionCompleted;
            Server.ServerVariables serverVariables = ((KeyValuePair<string, ServerVariables>)registeredServersListView.SelectedItem).Value;
            Server.TestConnectionVariablesArgs result = ((Server.TestConnectionVariablesArgs)e.Result);

            bool isTestConnectionSuccessful = result.IsTestConnectionSuccessful;

            if (isTestConnectionSuccessful)
            {
                if (isSetToActive)
                {
                    serverVariables.Information = result.ServerInformation;
                    if (MessageBox.Show("Connection Successful. \n\nWould you like to set selection as Active Server Configuration?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        communicator.UseNewHost();
                        communicator.WriteResources();
                        communicator.Connect();
                    }
                }
                else
                {
                    communicator.UseNewHost();
                    communicator.ServerCollection.WriteResources();
                    MessageBox.Show("Connection Successful.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }

                RefreshRegisteredServersView();
            }
            else MessageBox.Show("Invalid Connection.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            SetButtonEnable(true);
        }

        private void setToCurrentActiveServerConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonEnable(false);
            isSetToActive = true;
            TestConnection();
        }

        private void testConnectionToRetrieveOrReloadServerConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            SetButtonEnable(false);
            isSetToActive = false;
            TestConnection();
        }
    }
}
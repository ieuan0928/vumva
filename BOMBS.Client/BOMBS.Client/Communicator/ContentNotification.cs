using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;

using BOMBS.Core.Common.Handlers;
using BOMBS.UI.Foundation.Controls;

namespace BOMBS.Client.Communicator
{
    public class ContentNotification : DialogContent, IDialogContent
    {
        private BombsHost.DatabaseInformation previousDatabaseInformation;
        private Window parentWindow;

        public ContentNotification()
        {
            Loaded += ContentNotification_Loaded;
            Unloaded += ContentNotification_Unloaded;

            ServiceController.Communicator.DatabaseStatusOnChanged += Communicator_DatabaseStatusOnChanged;
            ServiceController.Communicator.ConfigureDatabaseStepsOnChanged += Communicator_ConfigureDatabaseStepsOnChanged;
            parentWindow = Window.GetWindow(this);
        }

        private void Communicator_ConfigureDatabaseStepsOnChanged(object sender, ResultArg e)
        {
            Dispatcher.BeginInvoke(new Action(delegate()
            {
                switch ((BombsHost.DatabaseConfigurationSteps)e.Result)
                {
                    case BombsHost.DatabaseConfigurationSteps.ConfigurationFailure:
                    case BombsHost.DatabaseConfigurationSteps.ConfigurationCancelled:
                        HideBusyMessage();
                        break;
                }
            }));
        }

        private void ContentNotification_Loaded(object sender, RoutedEventArgs e)
        {
            parentWindow = Window.GetWindow(this);
        }

        private void Communicator_DatabaseStatusOnChanged(object sender, Database.ConnectdDatabaseOnProgressArguments e)
        {
            previousDatabaseInformation = e.PreviousDatabaseInformation;

            BombsHost.DatabaseStatus newValue = e.NewStatus;
            if (newValue != BombsHost.DatabaseStatus.Ready)
            {
                Dispatcher.BeginInvoke(new Action(delegate()
                {
                    switch (newValue)
                    {
                        case BombsHost.DatabaseStatus.ConfigurationOnProgress:
                            ShowBusyMessage(Properties.Resources.DatabaseStatus_ConfigurationOnProgress);
                            break;
                        case BombsHost.DatabaseStatus.DatabaseErrorConfiguration:
                        case BombsHost.DatabaseStatus.Ready:
                            HideBusyMessage();
                            break;
                    }
                }));
            }
        }

        private void ContentNotification_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ServiceController.Communicator.DatabaseStatusOnChanged -= Communicator_DatabaseStatusOnChanged;
        }
    }
}

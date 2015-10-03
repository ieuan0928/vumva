﻿using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UIFoundation = BOMBS.UI.Foundation;
using BOMBS.UI.Foundation;

namespace BOMBS.Client.Communicator
{
    namespace Server
    {
        public class Dialogs
        {
            #region New Connection

            public static bool CreateNewConnection(bool isShutDownWhenCancel = true)
            {
                UIFoundation.Popup popup = new UIFoundation.Popup()
                    {
                        TitleImage = UIFoundation.Helper.GetResourceBitmapSource(@"BOMBS.Client;component/Resources/server_connect.png"),
                        Caption = "New Server Configuration",
                        PopupTitle = "Create New Server Connection",
                        TypeOfContent = typeof(Controls.NewConnection),
                    };

                if (isShutDownWhenCancel)
                {
                    popup.Closing -= popup_Closing;
                    popup.Closing += popup_Closing;
                }

                WindowBase.DialogValue result = popup.ShowDialog();
                switch (result)
                {
                    case WindowBase.DialogValue.None:
                    case WindowBase.DialogValue.Incomplete:
                    case WindowBase.DialogValue.Failed:
                        if (isShutDownWhenCancel)
                            Application.Current.Shutdown();
                        break;
                    case WindowBase.DialogValue.Success:
                        ServiceController communicator = ServiceController.Communicator;

                        Server.ServerVariables serverConfiguration = ((Server.Controls.NewConnection)popup.DialogContent).ServerConfiguration;

                        communicator.ActiveServer = serverConfiguration;
                        communicator.UseNewHost();

                        communicator.WriteResources();
                        break;
                }

                return result == WindowBase.DialogValue.Success;
            }

            private static void popup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                if (MessageBox.Show("There is no connection to the Server. \n\nThis action would Exit BOMBS. \n\nWould you like to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    e.Cancel = true;
            }

            #endregion

            #region Registered Servers

            public static void ViewRegisteredConnection()
            {
                UIFoundation.Popup registeredServersPopup = new UIFoundation.Popup()
                    {
                        TitleImage = UIFoundation.Helper.GetResourceBitmapSource(@"BOMBS.Client;component/Resources/server_connect.png"),
                        Caption = "Server Configuration",
                        PopupTitle = "Registered Server Connection",
                        TypeOfContent = typeof(Controls.ServerList),
                    };

                registeredServersPopup.ButtonCollection.ButtonSet = UIFoundation.Controls.DialogButtons.DialogType.CloseOnly;

                registeredServersPopup.Closing += registeredServersPopup_Closing;

                registeredServersPopup.ShowDialog();
            }

            private static void registeredServersPopup_Closing(object sender, System.ComponentModel.CancelEventArgs e)
            {
                if (ServiceController.Communicator.ActiveServer.ServerConfigurationStatus != ServerConfigurationStatus.Active)
                {
                    if (MessageBox.Show("There is no connection to the Server. \n\nThis action would Exit BOMBS. \n\nWould you like to continue?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        e.Cancel = true;
                    else Application.Current.Shutdown();
                }
            }

            #endregion
        }
    }

    namespace Database
    {
        public class Dialogs
        {
            public static bool ViewServerDatabaseSettings()
            {
                UIFoundation.Popup popup = new UIFoundation.Popup()
                {
                    TitleImage = UIFoundation.Helper.GetResourceBitmapSource(@"BOMBS.Client;component/Resources/server_connect.png"),
                    Caption = "Server Database Connection",
                    PopupTitle = "Database Configuration for the Server", 
                    TypeOfContent = typeof(Control)
                };

                popup.DialogButton.ButtonSet = UIFoundation.Controls.DialogButtons.DialogType.CloseOnly;
                
                return popup.ShowDialog() == WindowBase.DialogValue.Success;
            }
        }
    }
}

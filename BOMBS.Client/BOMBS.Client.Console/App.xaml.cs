using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using BOMBS.Client.Communicator;
using BOMBS.Core.Common.Handlers;
using BUtil = BOMBS.UI.Foundation.Utilities;
using System.Windows.Threading;

namespace BOMBS.Client.Console
{
    public partial class App : Application
    {
        public BUtil.SplashScreen BSplashScreen { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            BSplashScreen = new BUtil.SplashScreen();
            BSplashScreen.Closed += BSplashScreen_Closed;
            BSplashScreen.Show();

            ServiceController.Communicator.Start();

            base.MainWindow = new MainWindow();
            base.OnStartup(e);
            base.MainWindow.Show();
        }

        private void BSplashScreen_Closed(object sender, EventArgs e)
        {
            base.MainWindow.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)(delegate()
            {
                WindowState oldWindowState = base.MainWindow.WindowState;

                if (oldWindowState == WindowState.Minimized) base.MainWindow.WindowState = WindowState.Normal;
                else
                {
                    base.MainWindow.WindowState = WindowState.Minimized;
                    base.MainWindow.WindowState = oldWindowState;
                }
            }));
        }
    }
}

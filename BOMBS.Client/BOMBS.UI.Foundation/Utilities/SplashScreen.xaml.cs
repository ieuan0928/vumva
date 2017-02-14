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
using System.Windows.Shapes;
using System.Timers;
using System.Diagnostics;
using System.Threading;

namespace BOMBS.UI.Foundation.Utilities
{
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private Stopwatch timer;

        private int splashTime = 200;
        public int SplashTime
        {
            get { return splashTime; }
            set { splashTime = value; }
        }

        private bool allowClose = false;
        public bool AllowClose
        {
            get { return allowClose; }
            set
            {
                allowClose = value;

                if (allowClose)
                {
                    timer.Stop();
                    if (timer.ElapsedMilliseconds < splashTime) Thread.Sleep((int)(splashTime - timer.ElapsedMilliseconds));
                    Close();
                }
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            timer = new Stopwatch();
            timer.Start();
        }
    }
}

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

namespace Bombs.Service.AppController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BOMBS.Service.Controller.Communicator.Start();
            btnConnect.IsEnabled = false;
        }

        private void btnConnect_Copy_Click(object sender, RoutedEventArgs e)
        {
            string[] args = { "TCP:1", "http:2", "endpoint:3" };

            BOMBS.Service.Controller.Communicator.CreateNewPorts(args);
        }


    }
}

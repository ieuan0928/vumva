﻿using BOMBS.UI.Foundation.Wizard.Controls;
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

namespace BOMBS.Client.Initialization
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : PageBase
    {
        public WelcomePage()
        {
            InitializeComponent();

            Title = "Welcome to Initialization Wizard";
        }
    }
}

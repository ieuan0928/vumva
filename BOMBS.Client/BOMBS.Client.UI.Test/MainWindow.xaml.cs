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
using BOMBS.UI.Foundation;


namespace BOMBS.Client.UI.Test
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
            BOMBS.UI.Foundation.Dialog diag = new BOMBS.UI.Foundation.Dialog();

            diag.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            BOMBS.UI.Foundation.Wizard_Window userwizard = new BOMBS.UI.Foundation.Wizard_Window();

            userwizard.DataContext = new BOMBS.Administration.User.UserInformation();
            userwizard.AddControl("Before You Begin", new BOMBS.Administration.User.ucBegin());
            userwizard.AddControl("General Information", new BOMBS.Administration.User.ucUserGeneralinfo());
            userwizard.AddControl("System User Registration", new BOMBS.Administration.User.ucCreateUser());
            userwizard.AddControl("Summary", new BOMBS.Administration.User.ucUserSummary());
            userwizard.AddControl("Finish", new BOMBS.Administration.User.ucFinished());
            userwizard.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            BOMBS.UI.Foundation.Wizard_Window emp_wizard = new BOMBS.UI.Foundation.Wizard_Window();
            emp_wizard.DataContext = new BOMBS.HRMS.Entites.Employee();
            emp_wizard.AddControl("Before You Begin", new BOMBS.HRMS.Entites.Employees.ucBegin_Emp());
            emp_wizard.AddControl("Employee Details", new BOMBS.HRMS.Entites.Employees.ucEmp_info());
            emp_wizard.AddControl("Employee Catalog", new BOMBS.HRMS.Entites.Employees.ucEmp_Catalog());
            emp_wizard.AddControl("Summarry", new BOMBS.HRMS.Entites.Employees.ucEmp_Summary());
            emp_wizard.Show();



        }
    }
}

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
using BOFP.ClientRelationManagement.Controls;
namespace BOFP.ClientRelationManagement.Wizard_View
{
    /// <summary>
    /// Interaction logic for Occupation_Permit.xaml
    /// </summary>
    public partial class Occupation_Permit : BaseControl, IControl
    {
        public Occupation_Permit()
        {
            InitializeComponent();
            Title = "Owner Details...";
            IsCreateEnable = false;
        }

        private void rbCreate_Click(object sender, RoutedEventArgs e)
        {
          
        }

        private void rbCreate_Checked(object sender, RoutedEventArgs e)
        {
            if (rbCreate.IsChecked == true)
            {
                txtOwnerFName.IsEnabled = true;
                txtOwnerMname.IsEnabled = true;
                txtOwnerLname.IsEnabled = true;
            }
       
        }

        private void rbCreate_Unchecked(object sender, RoutedEventArgs e)
        {
            txtOwnerFName.IsEnabled = false;
            txtOwnerMname.IsEnabled = false;
            txtOwnerLname.IsEnabled = false;
        }

        private void rbSelect_Checked(object sender, RoutedEventArgs e)
        {
            cmbExisting.IsEnabled = true;
        }

        private void rbSelect_Unchecked(object sender, RoutedEventArgs e)
        {
            cmbExisting.IsEnabled = false;
        }
    }
}

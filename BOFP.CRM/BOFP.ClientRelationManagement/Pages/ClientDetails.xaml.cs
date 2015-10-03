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
using BOFP.ClientRelationManagement.Model;

namespace BOFP.ClientRelationManagement.Pages
{
    /// <summary>
    /// Interaction logic for ClientDetails.xaml
    /// </summary>
    public partial class ClientDetails : UserControl
    {
        public ClientDetails()
        {
            InitializeComponent();
        }

        private void btnAddnewClient_Click(object sender, RoutedEventArgs e)
        {
            Wizard_Window clientwizard = new Wizard_Window();
            clientwizard.DataContext = new Owner_Details();
            clientwizard.AddControl("Before You Begin", new Wizard_View.ucBegin());
            clientwizard.AddControl("Owner Details", new Wizard_View.Occupation_Permit());
            clientwizard.AddControl("Business Details", new Wizard_View.Business());
            clientwizard.AddControl("Summaryy", new Wizard_View.ucSummary());
            clientwizard.ShowDialog();

        }

    }
}

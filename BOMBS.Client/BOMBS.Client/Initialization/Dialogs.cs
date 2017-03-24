using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Wizard = BOMBS.UI.Foundation.Wizard;

namespace BOMBS.Client.Initialization
{
    public class Dialogs
    {
        public static void CreateWizard()
        {
            Wizard.Content content = new Wizard.Content();

            content.Steps.Add(new Wizard.Step("Before You Begin", typeof(WelcomePage)));
            content.Steps.Add(new Wizard.Step("New Administrator", typeof(AdminProfilePage)));
            content.Steps.Add(new Wizard.Step("User Roles", typeof(UserRolesPage)));
            content.Steps.Add(new Wizard.Step("Initial Users", typeof(UsersPage)));
            content.Steps.Add(new Wizard.Step("Service Mapping", typeof(ServiceMapPage)));
            content.Steps.Add(new Wizard.Step("Summary", typeof(SummaryPage)));
            content.Steps.Add(new Wizard.Step("Confirmation", typeof(ResultsPage)));

            new Wizard.Window(content).ShowDialog();
        }

    }
}

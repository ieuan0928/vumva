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
    /// Interaction logic for ucBegin.xaml
    /// </summary>
    public partial class ucBegin : BaseControl, IControl
    {
        public ucBegin()
        {
            InitializeComponent();
            Title = "Before You Begin...";
            IsPreviousEnabled = false;
            IsCreateEnable = false;
        }
    }
}

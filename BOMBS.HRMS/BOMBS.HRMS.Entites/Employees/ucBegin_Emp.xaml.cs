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
using BOMBS.UI.Foundation.Controls;
namespace BOMBS.HRMS.Entites.Employees
{
    /// <summary>
    /// Interaction logic for ucBegin_Emp.xaml
    /// </summary>
    public partial class ucBegin_Emp : BaseControl, IControl
    {
        public ucBegin_Emp()
        {
            InitializeComponent();
            Title = "Before You Begin...";
            IsPreviousEnabled = false;
            IsCreateEnable = false;
        }
    }
}

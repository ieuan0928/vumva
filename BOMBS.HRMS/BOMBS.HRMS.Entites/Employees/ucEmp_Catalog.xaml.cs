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
    /// Interaction logic for ucEmp_Catalog.xaml
    /// </summary>
    public partial class ucEmp_Catalog : BaseControl, IControl
    {
        public ucEmp_Catalog()
        {
            InitializeComponent();
            Title = "Employee Catalog...";
            IsCreateEnable = false;
        }
    }
}

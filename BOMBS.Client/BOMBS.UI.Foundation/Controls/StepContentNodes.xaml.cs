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

namespace BOMBS.UI.Foundation.Controls
{
    /// <summary>
    /// Interaction logic for StepContentNodes.xaml
    /// </summary>
    public partial class StepContentNodes : UserControl
    {
        public enum StateEnum : short
        {
            Default = 0,
            Disabled = -1,
            Active = 1
        }

        //public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(StateEnum), typeof(StepContentNodes), new PropertyMetadata(StateEnum.Default));

        public StepContentNodes()
        {
            InitializeComponent();
        }


        private StateEnum _state = StateEnum.Default;
        public StateEnum State
        {
            get { return _state; }
            set { _state = value; }
        }

        public override void OnApplyTemplate()
        {
            if (State == StateEnum.Active)
            {
                lbl.Background = new SolidColorBrush(Colors.White );
            }
            base.OnApplyTemplate();
        }
    }
}

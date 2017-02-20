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
using System.Windows.Shapes;

namespace BOMBS.UI.Foundation.Wizard
{
    /// <summary>
    /// Interaction logic for WizardWindow.xaml
    /// </summary>
    public partial class Window : WindowBase
    {
        public Window()
        {
            InitializeComponent();
        }

        public Window(Content wizardContent) : this()
        {
            this.wizardContent = wizardContent;
        }

        private Content wizardContent = null;
        public Content WizardContent
        {
            get { return wizardContent; }
            set { wizardContent = value; }
        }

        public override void OnApplyTemplate()
        {
            stepContentControl.Steps = wizardContent.Steps;
            base.OnApplyTemplate();
        }
    }
}

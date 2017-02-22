using BOMBS.UI.Foundation.Controls.Dialog;
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
    public partial class Window : WindowBase
    {
        public Window()
        {
            InitializeComponent();

            dialogButtonControl.OnDialogButtonClicked += DialogButtonControl_OnDialogButtonClicked;
        }

        private void DialogButtonControl_OnDialogButtonClicked(object sender, Foundation.Controls.Dialog.ButtonClickEventArgs e)
        {
            switch (e.Type)
            {
                case ButtonClickEventArgs.ButtonType.NextButton:
                    stepContentControl.SelectedIndex++;
                    break;
                case ButtonClickEventArgs.ButtonType.PreviousButton:
                    stepContentControl.SelectedIndex--;
                    break;
            }
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

            Step firstStep = stepContentControl.Steps[0];
            firstStep.StepInstance = Activator.CreateInstance(firstStep.TypeOfStep) as UserControl;
            pageViewer.Content = firstStep.StepInstance;

            base.OnApplyTemplate();
        }

        private void stepContentControl_OnFocusedStepChanged(object sender, FocusedStepArg e)
        {
            if (e.Step.StepInstance == null)
                e.Step.StepInstance = Activator.CreateInstance(e.Step.TypeOfStep) as UserControl; 
           
            pageViewer.Content = e.Step.StepInstance;
        }
    }
}

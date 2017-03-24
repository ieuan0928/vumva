using BOMBS.UI.Foundation.Controls.Dialog;
using BOMBS.UI.Foundation.Wizard.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        private PageBase InstantiatePageBase(Type typeOfStep)
        {
            PageBase result = null;

            Type[] types = { typeof(Context) };
            ConstructorInfo constructorInfo = typeOfStep.GetConstructor(types);

            if (ConstructorInfo.Equals(constructorInfo, null)) result = Activator.CreateInstance(typeOfStep) as PageBase;
            else
            {
                result = Activator.CreateInstance(typeOfStep, wizardContent.Context) as PageBase;
            }
            return result;
        }

        private void LoadStep(Step stepToLoad)
        {
            if (stepToLoad.StepInstance == null)
                stepToLoad.StepInstance = InstantiatePageBase(stepToLoad.TypeOfStep);

            string titleToLoad = stepToLoad.StepInstance.Header;

            if (string.IsNullOrEmpty(titleToLoad)) headerTextBlock.Text = stepToLoad.Title;
            else if (string.IsNullOrEmpty(titleToLoad.Trim())) headerTextBlock.Text = stepToLoad.Title;
            else headerTextBlock.Text = titleToLoad.Trim();

            txtPopupTitle.Text = stepToLoad.Title;
            pageViewer.Content = stepToLoad.StepInstance;
            if (stepToLoad.StepInstance.AllowHorizontalScrolling)
                pageViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        public override void OnApplyTemplate()
        {
            stepContentControl.Steps = wizardContent.Steps;
            LoadStep(stepContentControl.Steps[0]);
            base.OnApplyTemplate();
        }

        private void stepContentControl_OnFocusedStepChanged(object sender, FocusedStepArg e)
        {
            LoadStep(e.Step);
        }

        private void caption_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //DragMove(); 
        }
    }
}

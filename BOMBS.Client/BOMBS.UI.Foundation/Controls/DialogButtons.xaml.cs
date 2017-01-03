using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BOMBS.UI.Foundation
{
    namespace Controls
    {
        public partial class DialogButtons : UserControl
        {
            public DialogButtons()
            {
                InitializeComponent();
            }

            public enum DialogType
            {
                Default,
                OkOnly,
                CancelOnly,
                CloseOnly,
                OkWithClose,
                DefaultWithClose,
                Wizard,
                CompletedWizard,
                Question,
                QuestionWithCancel,
                QuestionWithClose
            }

            private DialogType buttonSet = DialogType.Default;
            public DialogType ButtonSet
            {
                get { return buttonSet; }
                set
                {
                    buttonSet = value;

                    ShowAllButtons(false);
                    EnableAllButtons();
                    ClearDefaultButtons();
                    ClearCancelButtons();

                    switch (buttonSet)
                    {
                        case DialogType.OkOnly:
                            ShowOkButton = true;
                            IsOkButtonAsDefault = true;
                            IsOkButtonAsCancel = true;

                            break;
                        case DialogType.OkWithClose:
                            ShowOkButton = true;
                            ShowCloseButton = true;

                            IsOkButtonAsDefault = true;
                            IsCloseButtonAsCancel = true;

                            break;
                        case DialogType.CancelOnly:
                            ShowCancelButton = true;
                            IsCancelButtonAsDefault = true;
                            IsCancelButtonAsCancel = true;

                            break;
                        case DialogType.CloseOnly:
                            ShowCloseButton = true;
                            IsCloseButtonAsDefault = true;
                            IsCloseButtonAsCancel = true;

                            break;
                        case DialogType.Default:
                            ShowOkButton = true;
                            ShowCancelButton = true;

                            IsOkButtonAsDefault = true;
                            IsCancelButtonAsCancel = true;

                            break;
                        case DialogType.DefaultWithClose:
                            ShowOkButton = true;
                            ShowCancelButton = true;
                            ShowCloseButton = true;

                            IsOkButtonAsDefault = true;
                            IsCloseButtonAsDefault = true;
                            break;
                        case DialogType.Wizard:
                            ShowCancelButton = true;
                            ShowPreviousButton = true;
                            ShowNextButton = true;
                            ShowCreateButton = true;

                            IsNextButtonAsDefault = true;
                            IsCancelButtonAsCancel = true;
                            IsCreateButtonEnabled = false;

                            break;
                        case DialogType.CompletedWizard:
                            ShowCancelButton = true;
                            ShowNextButton = true;
                            ShowPreviousButton = true;
                            ShowCloseButton = true;

                            IsCancelButtonEnabled = false;
                            IsNextButtonEnabled = false;
                            IsPreviousButtonEnabled = false;

                            IsCloseButtonAsDefault = true;
                            IsCloseButtonAsCancel = true;

                            break;
                        case DialogType.Question:
                            ShowYesButton = true;
                            ShowNoButton = true;

                            IsYesButtonAsDefault = true;
                            IsNoButtonAsCancel = true;

                            break;
                        case DialogType.QuestionWithCancel:
                            ShowYesButton = true;
                            ShowNoButton = true;
                            ShowCancelButton = true;

                            IsYesButtonAsDefault = true;
                            IsCancelButtonAsCancel = true;

                            break;
                        case DialogType.QuestionWithClose:
                            ShowYesButton = true;
                            ShowNoButton = true;
                            ShowCloseButton = true;

                            IsYesButtonAsDefault = true;
                            IsCloseButtonAsCancel = true;

                            break;
                    }
                }
            }

            #region Default Functionalities

            public bool IsYesButtonAsDefault
            {
                get { return btnYes.IsDefault; }
                set { btnYes.IsDefault = value; }
            }

            public bool IsOkButtonAsDefault
            {
                get { return btnOk.IsDefault; }
                set { btnOk.IsDefault = value; }
            }

            public bool IsCancelButtonAsDefault
            {
                get { return btnCancel.IsDefault; }
                set { btnCancel.IsDefault = value; }
            }

            public bool IsCloseButtonAsDefault
            {
                get { return btnClose.IsDefault; }
                set { btnClose.IsDefault = value; }
            }

            public bool IsCreateButtonAsDefault
            {
                get { return btnCreate.IsDefault; }
                set { btnCreate.IsDefault = value; }
            }

            public bool IsNextButtonAsDefault
            {
                get { return btnNext.IsDefault; }
                set { btnNext.IsDefault = value; }
            }

            public void ClearDefaultButtons()
            {
                if (IsYesButtonAsDefault) IsYesButtonAsDefault = false;
                if (IsOkButtonAsDefault) IsOkButtonAsDefault = false;
                if (IsCancelButtonAsDefault) IsCancelButtonAsDefault = false;
                if (IsCloseButtonAsDefault) IsCloseButtonAsDefault = false;
                if (IsCreateButtonAsDefault) IsCreateButtonAsDefault = false;
                if (IsNextButtonAsDefault) IsNextButtonAsDefault = false;
            }

            #endregion

            #region Cancel Functionalities

            public bool IsNoButtonAsCancel
            {
                get { return btnCancel.IsCancel; }
                set { btnCancel.IsCancel = value; }
            }

            public bool IsOkButtonAsCancel
            {
                get { return btnOk.IsCancel; }
                set { btnOk.IsCancel = value; }
            }

            public bool IsCancelButtonAsCancel
            {
                get { return btnCancel.IsCancel; }
                set { btnCancel.IsCancel = value; }
            }

            public bool IsCloseButtonAsCancel
            {
                get { return btnClose.IsCancel; }
                set { btnClose.IsCancel = value; }
            }

            public void ClearCancelButtons()
            {
                if (IsNoButtonAsCancel) IsNoButtonAsCancel = false;
                if (IsOkButtonAsCancel) IsOkButtonAsCancel = false;
                if (IsCancelButtonAsCancel) IsCancelButtonAsCancel = false;
                if (IsCloseButtonAsCancel) IsCloseButtonAsCancel = false;
            }

            #endregion

            #region IsButtonEnabled Properties

            public bool IsOkButtonEnabled
            {
                get { return btnOk.IsEnabled; }
                set { btnOk.IsEnabled = value; }
            }

            public bool IsCancelButtonEnabled
            {
                get { return btnCancel.IsEnabled; }
                set { btnCancel.IsEnabled = value; }
            }

            public bool IsPreviousButtonEnabled
            {
                get { return btnPrevious.IsEnabled; }
                set { btnPrevious.IsEnabled = value; }
            }

            public bool IsNextButtonEnabled
            {
                get { return btnNext.IsEnabled; }
                set { btnNext.IsEnabled = value; }
            }

            public bool IsCreateButtonEnabled
            {
                get { return btnCreate.IsEnabled; }
                set { btnCreate.IsEnabled = value; }
            }

            public bool IsCloseButtonEnabled
            {
                get { return btnClose.IsEnabled; }
                set { btnClose.IsEnabled = value; }
            }

            public bool IsYesButtonEnabled
            {
                get { return btnYes.IsEnabled; }
                set { btnYes.IsEnabled = value; }
            }

            public bool IsNoButtonEnabled
            {
                get { return btnNo.IsEnabled; }
                set { btnNo.IsEnabled = value; }
            }

            public void EnableAllButtons(bool value = true)
            {
                if (IsYesButtonEnabled != value) IsYesButtonEnabled = value;
                if (IsNoButtonEnabled != value) IsNoButtonEnabled = value;
                if (IsOkButtonEnabled != value) IsOkButtonEnabled = value;
                if (IsCancelButtonEnabled != value) IsCancelButtonEnabled = value;
                if (IsPreviousButtonEnabled != value) IsPreviousButtonEnabled = value;
                if (IsNextButtonEnabled != value) IsNextButtonEnabled = value;
                if (IsCreateButtonEnabled != value) IsCreateButtonEnabled = value;
                if (IsCloseButtonEnabled != value) IsCloseButtonEnabled = value;
            }

            #endregion

            #region ShowButton Functionalities

            public bool ShowYesButton
            {
                get { return btnYes.Visibility == Visibility.Visible; }
                set { btnYes.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowNoButton
            {
                get { return btnNo.Visibility == Visibility.Visible; }
                set { btnNo.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowOkButton
            {
                get { return btnOk.Visibility == Visibility.Visible; }
                set { btnOk.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowCancelButton
            {
                get { return btnCancel.Visibility == Visibility.Visible; }
                set { btnCancel.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowPreviousButton
            {
                get { return btnPrevious.Visibility == Visibility.Visible; }
                set { btnPrevious.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowNextButton
            {
                get { return btnNext.Visibility == Visibility.Visible; }
                set { btnNext.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowCreateButton
            {
                get { return btnCreate.Visibility == Visibility.Visible; }
                set { btnCreate.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public bool ShowCloseButton
            {
                get { return btnClose.Visibility == Visibility.Visible; }
                set { btnClose.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
            }

            public void ShowAllButtons(bool value = true)
            {
                if (ShowYesButton != value) ShowYesButton = value;
                if (ShowNoButton != value) ShowNoButton = value;
                if (ShowOkButton != value) ShowOkButton = value;
                if (ShowCancelButton != value) ShowCancelButton = value;
                if (ShowPreviousButton != value) ShowPreviousButton = value;
                if (ShowNextButton != value) ShowNextButton = value;
                if (ShowCreateButton != value) ShowCreateButton = value;
                if (ShowCloseButton != value) ShowCloseButton = value;
            }

            #endregion

            #region Event Handlers

            public event Dialog.ButtonClickHandler OnYesButtonClicked;
            public event Dialog.ButtonClickHandler OnNoButtonClicked;
            public event Dialog.ButtonClickHandler OnOkButtonClicked;
            public event Dialog.ButtonClickHandler OnCancelButtonClicked;
            public event Dialog.ButtonClickHandler OnPreviousButtonClicked;
            public event Dialog.ButtonClickHandler OnNextButtonClicked;
            public event Dialog.ButtonClickHandler OnCreateButtonClicked;
            public event Dialog.ButtonClickHandler OnCloseButtonClicked;
            public event Dialog.ButtonClickHandler OnDialogButtonClicked;

            private void RaiseHandler(Button sender, Dialog.ButtonClickEventArgs.ButtonType ButtonType)
            {
                Dialog.ButtonClickEventArgs eventArgs = new Dialog.ButtonClickEventArgs() { Button = sender, Type = ButtonType };

                switch (ButtonType)
                {
                    case Dialog.ButtonClickEventArgs.ButtonType.CancelButton:
                        if (OnCancelButtonClicked != null) OnCancelButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.CloseButton:
                        if (OnCloseButtonClicked != null) OnCloseButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.CreateButton:
                        if (OnCreateButtonClicked != null) OnCreateButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.NextButton:
                        if (OnNextButtonClicked != null) OnNextButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.NoButton:
                        if (OnNoButtonClicked != null) OnNoButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.OkButton:
                        if (OnOkButtonClicked != null) OnOkButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.PreviousButton:
                        if (OnPreviousButtonClicked != null) OnPreviousButtonClicked(this, eventArgs);
                        break;
                    case Dialog.ButtonClickEventArgs.ButtonType.YesButton:
                        if (OnYesButtonClicked != null) OnYesButtonClicked(this, eventArgs);
                        break;
                }
                
                if (OnDialogButtonClicked != null) OnDialogButtonClicked(this, eventArgs);
            }

            private void btnYes_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.YesButton);
            }

            private void btnNo_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.NoButton);
            }

            private void btnOk_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.OkButton);
            }

            private void btnCancel_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.CancelButton);
            }

            private void btnPrevious_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.PreviousButton);
            }

            private void btnNext_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.NextButton);
            }

            private void btnCreate_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.CreateButton);
            }

            private void btnClose_Click(object sender, RoutedEventArgs e)
            {
                RaiseHandler(sender as Button, Dialog.ButtonClickEventArgs.ButtonType.CloseButton);
            }

            #endregion
        }

        namespace Dialog
        {
            public delegate void ButtonClickHandler(object sender, ButtonClickEventArgs e);

            public class ButtonClickEventArgs : EventArgs
            {
                public enum ButtonType
                {
                    Undefined,
                    YesButton,
                    NoButton,
                    OkButton,
                    CancelButton,
                    PreviousButton,
                    NextButton,
                    CreateButton,
                    CloseButton
                }

                private ButtonType type = ButtonType.Undefined;
                public ButtonType Type
                {
                    get { return type; }
                    set { type = value; }
                }

                public Button Button { get; set; }
            }
        }
    }
}


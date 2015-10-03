using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using BOMBS.UI.Foundation.Controls;
using System.ComponentModel;
using BOMBS.UI.Foundation.Controls.Dialog;
using BOMBS.Core.Common.Handlers;

namespace BOMBS.UI.Foundation
{
    public class WindowBase : Window
    {     
        public event EventHandler Cancelled;
        public event NewValueHandler ValueChanged;

        public enum ContentSetProcedure { AsUserControl, AsType }
        public enum DialogValue { None, Success, Incomplete, Failed }
        protected DialogValue value = DialogValue.None;
        public DialogValue Value { get { return value; } }

        public string Caption
        {
            get { return Title; }
            set { Title = value; }
        }

        public DialogButtons ButtonCollection { get; set; }

        protected ContentSetProcedure contentSetProcedure;
        protected UserControl dialogContent = null;
        protected Utilities.OverlappingBusyMessage overlappingMessage = new Utilities.OverlappingBusyMessage();

        protected DialogButtons dialogButtonControl;

        private DialogButtons.DialogType dialogButtonSet = DialogButtons.DialogType.Default;

        public Type TypeOfContent
        {
            set
            {
                dialogContent = Activator.CreateInstance(value) as UserControl;

                IDialogContent dc = (IDialogContent)dialogContent;
                dc.ButtonCollection = ButtonCollection;
                dc.OnDialogContentRequest += dc_OnDialogContentRequest;

                dc.OnDialogShowContentBusyMessage += dc_OnDialogShowContentBusyMessage;
                dc.OnDialogHideContentBusyMessage += dc_OnDialogHideContentBusyMessage;

                contentSetProcedure = ContentSetProcedure.AsType;
            }
        }

        private void dc_OnDialogHideContentBusyMessage(object sender, EventArgs e)
        {
            overlappingMessage.Hide();
        }

        private void dc_OnDialogShowContentBusyMessage(object sender, DialogContentBusyMessageEventArgs e)
        {
            overlappingMessage.BusyMessage = e.Message;
            overlappingMessage.BusyTitle = e.Title;

            overlappingMessage.Show();
        }

        private void dc_OnDialogContentRequest(object sender, DialogContentRequestEventArgs e)
        {
            switch (e.RequestType)
            {
                case DialogContentRequestType.Close:
                    Close();
                    break;
                case DialogContentRequestType.Cancel:
                    if (Cancelled != null) Cancelled(this, new EventArgs());
                    break;
            }
        }

        public UserControl InstanceOfContent
        {
            set
            {
                dialogContent = value;

                contentSetProcedure = ContentSetProcedure.AsUserControl;
            }
        }

        protected ImageSource titleImage = null;
        public ImageSource TitleImage
        {
            get { return titleImage; }
            set { titleImage = value; }
        }

        public new DialogValue ShowDialog()
        {
            base.ShowDialog();
            return value;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (value != DialogValue.Success) base.OnClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (value != DialogValue.Success) base.OnClosed(e);
        }

        private void OnValueChanged(DialogValue newValue)
        {
            if (newValue == value) return;

            DialogValue oldValue = value;
            value = newValue;

            if (ValueChanged != null)
                ValueChanged(this,
                    new NewValueArgs("Value")
                    {
                        OldValue = oldValue,
                        NewValue = newValue
                    });

        }

        private void dialogButtonControl_OnDialogButtonClicked(object sender, Controls.Dialog.ButtonClickEventArgs e)
        {
            switch (e.Type)
            {
                case ButtonClickEventArgs.ButtonType.CancelButton:
                    OnValueChanged(DialogValue.Incomplete);
                    break;
                case ButtonClickEventArgs.ButtonType.CloseButton:
                    OnValueChanged(DialogValue.Incomplete);
                    break;
                case ButtonClickEventArgs.ButtonType.OkButton:
                    OnValueChanged(DialogValue.Success);
                    DialogResult = true;
                    break;
            }
        }
    }
}

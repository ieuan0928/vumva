using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BOMBS.UI.Foundation
{
    namespace Controls
    {
        public class DialogContent : UserControl, IDialogContent
        {
            public DialogContent()
            {
                Loaded += DialogContent_Loaded;
            }

            private void DialogContent_Loaded(object sender, System.Windows.RoutedEventArgs e)
            {
                LoadState();
            }

            public event DialogContentRequestEventHandler OnDialogContentRequest;
            public event DialogContentBusyMessageEventHandler OnDialogShowContentBusyMessage;
            public event EventHandler OnDialogHideContentBusyMessage;

            protected DialogButtons buttonCollection;
            public virtual DialogButtons ButtonCollection
            {
                get { return buttonCollection; }
                set { buttonCollection = value; }
            }

            public virtual void LoadState() { }
            public virtual void SaveState() { }

            public void DoDialogRequest(DialogContentRequestType RequestType)
            {
                if (OnDialogContentRequest != null)
                    OnDialogContentRequest(this, new DialogContentRequestEventArgs() { RequestType = RequestType });
            }

            public void ShowBusyMessage()
            {
                if (OnDialogShowContentBusyMessage != null)
                    OnDialogShowContentBusyMessage(this, new DialogContentBusyMessageEventArgs());
            }

            public void ShowBusyMessage(string message)
            {
                if (OnDialogShowContentBusyMessage != null)
                    OnDialogShowContentBusyMessage(this, new DialogContentBusyMessageEventArgs(message));
            }

            public void ShowBusyMessage(string message, string title)
            {
                if (OnDialogShowContentBusyMessage != null)
                    OnDialogShowContentBusyMessage(this, new DialogContentBusyMessageEventArgs(message, title));
            }

            public void HideBusyMessage()
            {
                if (OnDialogHideContentBusyMessage != null) OnDialogHideContentBusyMessage(this, EventArgs.Empty);
            }

        }
    }
}

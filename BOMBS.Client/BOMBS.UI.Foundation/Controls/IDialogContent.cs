using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.UI.Foundation
{
    namespace Controls
    {
        public enum DialogContentRequestType
        {
            Ok,
            Close,
            Cancel
        }

        public delegate void DialogContentRequestEventHandler(object sender, DialogContentRequestEventArgs e);
        public delegate void DialogContentBusyMessageEventHandler(object sender, DialogContentBusyMessageEventArgs e);

        public class DialogContentRequestEventArgs : EventArgs
        {
            private DialogContentRequestType requestType = DialogContentRequestType.Close;
            public DialogContentRequestType RequestType
            {
                get { return requestType; }
                set { requestType = value; }
            }
        }

        public class DialogContentBusyMessageEventArgs : EventArgs
        {
            private string title = "Please Wait...";
            public string Title
            {
                get { return title; }
                set { title = value; }
            }

            private string message = "BOMBS Server is still Busy!";
            public string Message
            {
                get { return message; }
                set { message = value; }
            }

            public DialogContentBusyMessageEventArgs() : this("BOMBS Server is still Busy!", "Please Wait...") { }
            public DialogContentBusyMessageEventArgs(string message) : this(message, "Please Wait...") { }
            public DialogContentBusyMessageEventArgs(string message, string title)
            {
                this.message = message;
                this.title = title;
            }

        }

        public interface IDialogContent
        {
            DialogButtons ButtonCollection { get; set; }

            void LoadState();
            void SaveState();

            event DialogContentRequestEventHandler OnDialogContentRequest;
            event DialogContentBusyMessageEventHandler OnDialogShowContentBusyMessage;
            event EventHandler OnDialogHideContentBusyMessage;
        }
    }
}

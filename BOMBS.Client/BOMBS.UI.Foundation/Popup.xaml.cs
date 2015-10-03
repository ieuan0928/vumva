using BOMBS.UI.Foundation.Controls;
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

namespace BOMBS.UI.Foundation
{
    using Controls.Dialog;

    public partial class Popup : WindowBase
    {
        public Popup()
        {
            InitializeComponent();

            ButtonCollection = dialogButtonControl;
            overlappingMessage.ControlToLock = layoutGrid;
            Owner = Application.Current.MainWindow;
        }

        public string PopupTitle
        {
            get { return txtPopupTitle.Text; }
            set { txtPopupTitle.Text = value; }
        }

        private void WindowBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (titleImage != null) imgPopup.Source = titleImage;

            ChildContainer.Children.Add(dialogContent);
        }

        public UserControl DialogContent { get { return dialogContent; } }
        public DialogButtons DialogButton { get { return this.dialogButtonControl; } }
    }
}

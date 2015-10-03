using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public partial class MessageNotification : UserControl
    {
        public MessageNotification()
        {
            InitializeComponent();

            MessageCollection = new ObservableCollection<string>();
            MessageCollection.CollectionChanged += MessageCollection_CollectionChanged;
        }

        [Category("Common")]
        public ObservableCollection<string> MessageCollection { get; set; }

        private void MessageCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            string result = string.Empty;

            ObservableCollection<string> messageCollection = MessageCollection;

            if (messageCollection.Count > 0)
            {
                result = messageCollection[0];

                for (int i = 1; i < messageCollection.Count; i++)
                    result += string.Format("{0}{1}", Environment.NewLine, messageCollection[i]);
            }

            messageTextBox.Text = result;
        }
    }
}

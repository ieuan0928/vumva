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
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BOMBS.UI.Foundation.Utilities
{
    public partial class OverlappingBusyMessage : UserControl
    {
        public OverlappingBusyMessage()
        {
            InitializeComponent();

            Visibility = System.Windows.Visibility.Collapsed;
        }

        public OverlappingBusyMessage(Panel controlToLock)
            : this()
        {
            ControlToLock = controlToLock;
        }

        private List<UIElement> elementList = null;
        private UIElementCollection elementCollection = null;

        private Panel controlToLock;
        public Panel ControlToLock
        {
            get { return controlToLock; }
            set
            {
                controlToLock = value;

                controlToLock.Children.Add(this);

                if (controlToLock.GetType() == typeof(Grid)) InitializeGrid();

                elementList = new List<UIElement>();

                elementCollection = controlToLock.Children;
                int zIndex = 1;

                var enumerator = elementCollection.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    var element = (UIElement)enumerator.Current;
                    int elemZIndex = Panel.GetZIndex(element);
                    if (elemZIndex >= zIndex) zIndex = elemZIndex++;

                    if (element.IsEnabled) elementList.Add(element);
                }

                Panel.SetZIndex(this, zIndex);
            }
        }

        public string BusyTitle
        {
            get { return titleTextBox.Text.Trim(); }
            set { titleTextBox.Text = value.Trim(); }
        }

        public string BusyMessage
        {
            get { return messageTextBox.Text.Trim(); }
            set { messageTextBox.Text = value.Trim(); }
        }

        private void InitializeGrid()
        {
            Grid control = (Grid)controlToLock;

            Grid.SetRowSpan(this, control.RowDefinitions.Count + 1);
            Grid.SetColumnSpan(this, control.ColumnDefinitions.Count + 1);
        }

        private void blurElementCollection(bool isBlur)
        {

            var enumerator = elementCollection.GetEnumerator();
            
            while (enumerator.MoveNext())
            {
                var element = (UIElement)enumerator.Current;
                if (element != this)
                    if (isBlur)
                        element.Effect = new BlurEffect();
                    else element.Effect = null;
            }
        }

        private void SetChildrenIsEnable(bool isEnable)
        {
            var enumerator = elementList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var element = enumerator.Current;
                element.IsEnabled = isEnable;
            }
        }

        public void Show()
        {
            SetChildrenIsEnable(false);
            blurElementCollection(true);
            Visibility = System.Windows.Visibility.Visible;

        }

        public void Hide()
        {
            SetChildrenIsEnable(true);
            blurElementCollection(false);
            Visibility = System.Windows.Visibility.Collapsed;
        }
        
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Hide();
        }

    }
}

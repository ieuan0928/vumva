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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BOMBS.UI.Foundation.Wizard.Controls
{
    public partial class StepContentNodes : UserControl
    {
        public enum StateEnum : short
        {
            Active = 0,
            Disabled = -1,
            Focused = 1
        }

        public event EventHandler OnClick;

        //public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(StateEnum), typeof(StepContentNodes), new PropertyMetadata(StateEnum.Default));

        public StepContentNodes()
        {
            InitializeComponent();
        }

        private int myIndex = 0;
        public int NodeIndex
        {
            get { return myIndex; }
            set { myIndex = value; }
        }

        private StateEnum _state = StateEnum.Active;
        public StateEnum State
        {
            get { return _state; }
            set
            {
                _state = value;
                ApplyState();
            }
        }

        public string Title
        {
            get { return nodeTextBlock.Text; }
            set { nodeTextBlock.Text = value; }
        }

        private void ClearTextBlockEvents()
        {
            nodeTextBlock.TextDecorations = null;
            nodeTextBlock.Cursor = Cursors.Arrow;

            nodeTextBlock.MouseEnter -= NodeTextBlock_MouseEnter;
            nodeTextBlock.MouseLeave -= NodeTextBlock_MouseLeave;
            nodeTextBlock.PreviewMouseDown -= NodeTextBlock_PreviewMouseDown;
        }

        private void ApplyState()
        {
            switch (_state)
            {
                case StateEnum.Active:
                    nodeBorder.Background = new SolidColorBrush(Colors.Transparent);
                    nodeBorder.BorderThickness = new Thickness(0, 0, 1, 0);

                    nodeTextBlock.FontWeight = FontWeights.Normal;
                    nodeTextBlock.Cursor = Cursors.Hand;
                    nodeTextBlock.Effect = null;
                    nodeTextBlock.Foreground = new SolidColorBrush(Colors.Black);

                    nodeTextBlock.MouseEnter += NodeTextBlock_MouseEnter;
                    nodeTextBlock.MouseLeave += NodeTextBlock_MouseLeave;
                    nodeTextBlock.PreviewMouseDown += NodeTextBlock_PreviewMouseDown;

                    break;
                case StateEnum.Focused:
                    nodeTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                    nodeTextBlock.FontWeight = FontWeights.Bold;
                    nodeTextBlock.Effect = null;

                    nodeBorder.Background = new SolidColorBrush(Colors.White);
                    nodeBorder.BorderThickness = new Thickness(1, 1, 0, 1);

                    ClearTextBlockEvents();

                    break;
                case StateEnum.Disabled:
                    nodeTextBlock.Foreground = new SolidColorBrush(Color.FromRgb(180, 180, 180));
                    nodeTextBlock.Effect = new DropShadowEffect()
                    {
                        Direction = 240,
                        BlurRadius = 0,
                        ShadowDepth = 1.5,
                        RenderingBias = RenderingBias.Quality,
                        Color = Color.FromArgb(125, 255, 255, 255)
                    };

                    ClearTextBlockEvents();

                    break;

            }
        }

        private void NodeTextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (OnClick != null) OnClick(this, new EventArgs());
        }

        public override void OnApplyTemplate()
        {

            ApplyState();
            base.OnApplyTemplate();
        }

        private void NodeTextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            TextBlock me = (TextBlock)sender;
            me.TextDecorations = null;
        }

        private void NodeTextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            TextBlock me = (TextBlock)sender;
            me.TextDecorations = TextDecorations.Underline;
        }
    }
}

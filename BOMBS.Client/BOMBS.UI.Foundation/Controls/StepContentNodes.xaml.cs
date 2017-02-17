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

namespace BOMBS.UI.Foundation.Controls
{
    public partial class StepContentNodes : UserControl
    {
        public enum StateEnum : short
        {
            Active = 0,
            Disabled = -1,
            Focused = 1
        }

        //public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(StateEnum), typeof(StepContentNodes), new PropertyMetadata(StateEnum.Default));

        public StepContentNodes()
        {
            InitializeComponent();
        }


        private StateEnum _state = StateEnum.Active;
        public StateEnum State
        {
            get { return _state; }
            set { _state = value; }
        }

        public override void OnApplyTemplate()
        {
            switch (State)
            {
                case StateEnum.Active:
                    nodeTextBlock.Cursor = Cursors.Hand;

                    nodeTextBlock.MouseEnter += NodeTextBlock_MouseEnter;
                    nodeTextBlock.MouseLeave += NodeTextBlock_MouseLeave;
                    break;

                case StateEnum.Focused:
                    nodeLabel.Background = new SolidColorBrush(Colors.White);
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
                    break;

            }

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

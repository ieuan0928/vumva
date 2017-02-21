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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BOMBS.UI.Foundation.Wizard.Controls
{
    public partial class StepContent : UserControl
    {
        public StepContent()
        {
            InitializeComponent();
        }

        private IList<Step> steps;
        public IList<Step> Steps
        {
            get { return steps; }
            set { steps = value; }
        }

        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (value < 0 || value >= container.Children.Count - 2) return;

                int currentSelectedIndex = selectedIndex;
                selectedIndex = value;

                selectedIndexChanged(currentSelectedIndex, selectedIndex);
            }
        }

        private void selectedIndexChanged(int previousValue, int newValue)
        {
            (container.Children[previousValue + 1] as StepContentNodes).State = StepContentNodes.StateEnum.Active;
            (container.Children[newValue + 1] as StepContentNodes).State = StepContentNodes.StateEnum.Focused;
        }

        private void AddStepContentNodes(int index, Step stepToAdd, StepContentNodes.StateEnum state)
        {
            int position = index + 1;
            container.RowDefinitions.Insert(position, new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            StepContentNodes newNode = new StepContentNodes() { Title = stepToAdd.Title, State = state, NodeIndex = index };

            newNode.OnClick += NewNode_OnClick;

            container.Children.Insert(position, newNode);
            Grid.SetRow(newNode, position);
        }

        private void NewNode_OnClick(object sender, EventArgs e)
        {
            SelectedIndex = (sender as StepContentNodes).NodeIndex;
        }

        public override void OnApplyTemplate()
        {
            if (steps != null)
            {
                int stepCount = steps.Count;
                if (stepCount > 0)
                {
                    AddStepContentNodes(0, steps[0], StepContentNodes.StateEnum.Focused);

                    for (int i = 1; i < stepCount; i++)
                        AddStepContentNodes(i, steps[i], StepContentNodes.StateEnum.Disabled);

                    Grid.SetRow(bottomBorder, stepCount + 1);
                }
            }

            base.OnApplyTemplate();
        }
    }
}

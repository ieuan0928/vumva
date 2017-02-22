using BOMBS.UI.Foundation.Wizard.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.UI.Foundation.Wizard
{
    public delegate void FocusedStepHandler(object sender, FocusedStepArg e);
    public class FocusedStepArg : EventArgs
    {
        public FocusedStepArg(int StepIndex, Step Step, StepContentNodes Nodes)
        {
            this.StepIndex = StepIndex;
            this.Step = Step;
            this.Nodes = Nodes;
        }

        public int StepIndex { get; set; }

        public Step Step { get; set; }

        public StepContentNodes Nodes { get; set; }
    }
}

using BOMBS.UI.Foundation.Wizard.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace BOMBS.UI.Foundation.Wizard
{
    public class Step
    {
        public Step(string Title, Type TypeOfStep )
        {
            title = Title;
            typeOfStep = TypeOfStep;
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private Type typeOfStep = null;
        public Type TypeOfStep
        {
            get { return typeOfStep; }
            set { typeOfStep = value; }
        }

        private PageBase stepInstance = null;
        public PageBase StepInstance
        {
            get { return stepInstance; }
            set { stepInstance = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.UI.Foundation.Wizard
{
    public class Content
    {
        private Context context = null;
        public Context Context
        {
            get { return context; }
            set { context = value; }
        }

        private IList<Step> steps = new List<Step>();
        public IList<Step> Steps
        {
            get { return steps; }
        }
    }
}

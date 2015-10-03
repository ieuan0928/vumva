using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Common.Handlers
{
    public delegate void FailedHandler(object sender, FailedEventArgs e);

    public class FailedEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
    }
}

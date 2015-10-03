using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Common.Handlers
{
    public delegate void ResultHandler(object sender, ResultArg e);

    public class ResultArg : System.EventArgs
    {
        public object Result { get; set; }
    }
}

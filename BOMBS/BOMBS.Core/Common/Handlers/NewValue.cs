using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Common.Handlers
{
    public delegate void NewValueHandler(object sender, NewValueArgs e);

    public class NewValueArgs : PropertyChangedEventArgs
    {
        public NewValueArgs(string propertyName) : base(propertyName) { }

        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}

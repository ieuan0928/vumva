using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Framework
{
    [AttributeUsage(AttributeTargets.Property)]
    public class BitPropertyAttribute: AllowNullPropertyAttribute 
    {
    }
}

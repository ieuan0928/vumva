using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Log
{
    public class LogSource
    {
        public string LogName;
        public string SourceName;

        public LogSource(string logName, string sourceName)
        {
            LogName = logName;
            SourceName = sourceName;
        }
    }
}

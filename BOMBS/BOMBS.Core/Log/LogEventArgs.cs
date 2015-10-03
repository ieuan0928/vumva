using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BOMBS.Core.Log
{
    public class LogEventArgs : EventArgs
    {
        private string source;
        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        private string logName;
        public string LogName
        {
            get { return logName; }
            set { logName = value; }
        }

        private string message;
        public string Message
        {
            get { return message; }
        }

        private EventLogEntryType logEntryType;
        public EventLogEntryType LogEntryType
        {
            get { return logEntryType; }
        }


        public LogEventArgs(String Message, EventLogEntryType LogEntryType)
        {
            message = Message;
            logEntryType = LogEntryType;
        }
    }
}

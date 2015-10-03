using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BOMBS.Core.Log
{
    public class Logger : ILogger
    {
        internal Logger(string LogName, string Source)
        {
            this.logName = LogName;
            this.source = Source;

            if (!EventLog.SourceExists(this.source)) EventLog.CreateEventSource(this.source, this.logName);

            eventLog = new EventLog() { Source = this.source, Log = this.logName };
        }

        internal Logger(LogSource logSource)
            : this(logSource.LogName, logSource.SourceName)
        {
        }

        public event LogHandler LoggingAction;
        public event LogHandler LoggedAction;

        private EventLog eventLog;

        private string logName = "BOMBS Events";
        public string LogName
        {
            get { return logName; }
            internal set { logName = value; }
        }

        private string source = "BOMBS";
        public string Source
        {
            get { return source; }
            internal set { source = value; }
        }

        private EventLogEntryType logEventEntryType = EventLogEntryType.Information;
        public EventLogEntryType LogEventEntryType
        {
            get { return logEventEntryType; }
            set { logEventEntryType = value; }
        }

        private string message = string.Format("BOMBS Log ({0})", System.DateTime.UtcNow);
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public void LogNow(object sender, string Message, EventLogEntryType LogEventEntryType)
        {
            this.message = Message;
            this.logEventEntryType = LogEventEntryType;
            LogEventArgs args = new LogEventArgs(this.message, this.logEventEntryType) { LogName = this.logName, Source = this.source };

            if (LoggingAction != null) LoggingAction(sender, args);
            eventLog.WriteEntry(this.message, this.logEventEntryType);
            if (LoggedAction != null) LoggedAction(sender, args);
        }
    }
}
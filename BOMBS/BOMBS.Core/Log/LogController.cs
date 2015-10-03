using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOMBS.Core.Log
{
    public class LogController
    {
        private LogController()
        {
            logSources = new List<LogSource>();
            loggerCollection = new Dictionary<LogSource, Logger>();

            defaultSource = CreateLogSource("BOMBS Events", "BOMBS");
        }

        private LogSource defaultSource = null;
        public LogSource DefaultSource
        {
            get { return defaultSource; }
            set { defaultSource = value; }
        }

        private static LogController instance = null;
        public static LogController Instance
        {
            get
            {
                if (instance == null) instance = new LogController();

                return instance;
            }
        }

        private List<LogSource> logSources;
        private Dictionary<LogSource, Logger> loggerCollection;

        private LogSource CreateLogSource(string LogName, string SourceName)
        {
            LogSource result = new LogSource(LogName, SourceName);

            logSources.Add(result);
            loggerCollection.Add(result, new Logger(result));

            return result;
        }

        public Logger GetLogger(string LogName, string SourceName)
        {
            LogSource source = instance.logSources.FirstOrDefault(log => log.SourceName == LogName && log.LogName == SourceName);

            if (source == null) source = instance.CreateLogSource(LogName, SourceName);

            return instance.loggerCollection[source];
        }

        public Logger GetLogger(LogSource logSource)
        {
            if (!instance.loggerCollection.ContainsKey(logSource))
            {
                instance.logSources.Add(logSource);
                instance.loggerCollection.Add(logSource, new Logger(logSource));
            }

            return instance.loggerCollection[logSource];
        }

        public Logger GetDefaultLogger()
        {
            return GetLogger(defaultSource);
        }
    }
}

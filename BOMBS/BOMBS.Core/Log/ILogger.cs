using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BOMBS.Core.Log
{
    public interface ILogger
    {
        event LogHandler LoggingAction;
        event LogHandler LoggedAction;

        string LogName { get; }
        string Source { get; }
        string Message { get; set; }

        EventLogEntryType LogEventEntryType { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace SmartLog
{
    public class LoggerEventArgs : EventArgs
    {
        public IEnumerable<Log> Logs { get; private set; }

        public LoggerEventArgs(IEnumerable<Log> logs)
        {
            Logs = logs;
        }
    }
}
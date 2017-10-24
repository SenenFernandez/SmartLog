using System;

namespace SmartLog
{
    public class Log
    {
        public object Data { get; set; }

        public LogType LogType { get; set; }

        public DateTime Timestamp { get; private set; }

        public Log()
        {
            Timestamp = DateTime.Now;
        }

        public Log(object data, LogType logType) : this()
        {
            Data = data;
            LogType = logType;
        }

        public override string ToString()
        {
            return $"{{'Timestamp': '{Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz")}', 'LogType': '{LogType}', 'Data': '{Data}'}}";
        }
    }
}
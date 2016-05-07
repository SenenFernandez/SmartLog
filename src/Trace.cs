using System;

namespace SmartLog
{
    public static class Trace
    {
        public static string FileName
        {
            get { return Logger.Instance.FileName; }
            set { Logger.Instance.FileName = value; }
        }

        public static string FileExtension
        {
            get { return Logger.Instance.FileExtension; }
            set { Logger.Instance.FileExtension = value; }
        }

        public static string FilePeriod
        {
            get { return Logger.Instance.FilePeriod; }
            set { Logger.Instance.FilePeriod = value; }
        }

        public static void Debug(this string message, params object[] args)
        {
            Logger.Instance.Save(message.FormatEx(args), LogType.Debug);
        }

        public static void Message(this string message, params object[] args)
        {
            Logger.Instance.Save(message.FormatEx(args), LogType.Message);
        }

        public static void Error(this string message, params object[] args)
        {
            Logger.Instance.Save(message.FormatEx(args), LogType.Error);
        }

        public static void Exception(this Exception exception, params object[] args)
        {
            Logger.Instance.Save(exception, LogType.Exception);
        }

        public static void Start()
        {
            Logger.Instance.Initialize();
        }

        public static void Stop()
        {
            Logger.Instance.Uninitialize();
        }
    }
}
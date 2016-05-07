using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLog
{
    public sealed class Logger : Singleton<Logger>, IDisposable
    {
        private ManualResetEvent worker;
        private ConcurrentQueue<Log> queue;

        public string FileName { get; set; }

        public string FileExtension { get; set; } = "sflog";

        public string FilePeriod { get; set; } = "-yyyy-MM-dd";

        public bool IsLogging { get; private set; }

        public event EventHandler<LoggerEventArgs> Logged;

        protected override void DoInitialize()
        {
            if (FileName.IsNullOrEmpty())
            {
                FileName = Environment.GetCommandLineArgs().FirstOrDefault()
                    ?? Process.GetCurrentProcess().MainModule.ModuleName;
            }
            worker = new ManualResetEvent(false);
            queue = new ConcurrentQueue<Log>();
            IsLogging = true;
            Task.Factory.StartNew(() =>
            {
                do
                {
                    worker.WaitOne();
                    Dequeue();
                } while (IsLogging);
            });
        }

        protected override void DoUninitialize()
        {
            IsLogging = false;
            worker.Set();
            worker.Close();
            if (Logged != null)
            {
                foreach (EventHandler<LoggerEventArgs> handler in Logged.GetInvocationList())
                {
                    Logged -= handler;
                }
            }
        }

        public void Dispose()
        {
            Uninitialize();
            if (worker != null)
            {
                worker.Dispose();
                worker = null;
            }
        }

        public void Save<T>(T value, LogType logType)
        {
            Initialize();
            lock (Sync)
            {
                if (IsLogging)
                {
                    var log = new Log(value, logType);
                    queue.Enqueue(log);
                    worker.Set();
                }
            }
        }

        private void Dequeue()
        {
            if (!queue.IsEmpty && IsLogging)
            {
                var list = new List<Log>();
                lock (Sync)
                {
                    while (!queue.IsEmpty && IsLogging)
                    {
                        Log log;
                        queue.TryDequeue(out log);
                        if (log != null)
                        {
                            list.Add(log);
                        }
                    }
                }
                Save(list);
            }
            if (worker != null)
            {
                worker.Reset();
            }
        }

        private void Save(List<Log> values)
        {
            if (values.Count > 0)
            {
                var filePeriod = FilePeriod.IsNullOrEmpty() ? "" : DateTime.Now.ToString(FilePeriod);
                var fileName = "{0}{1}.{2}".FormatEx(FileName, filePeriod, FileExtension);
                string path = Path.GetDirectoryName(fileName);
                if (path == null)
                {
                    return;
                }
                if (path.IsNotEmpty() && !Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var outfile = new StreamWriter(fileName, true))
                {
                    values.ForEach(outfile.WriteLine);
                }
                if (Logged != null)
                {
                    Logged.Invoke(fileName, new LoggerEventArgs(values));
                }
            }
        }
    }
}
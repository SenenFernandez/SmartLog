namespace SmartLog
{
    public class Singleton<T> where T : new()
    {
        private static T instance;

        protected static readonly object Sync = new object();

        public bool IsInitialized { get; private set; }

        protected virtual void DoInitialize()
        {
        }

        protected virtual void DoUninitialize()
        {
        }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (Sync)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }

        public void Initialize()
        {
            if (!IsInitialized)
            {
                lock (Sync)
                {
                    if (!IsInitialized)
                    {
                        DoInitialize();
                        IsInitialized = true;
                    }
                }
            }
        }

        public void Uninitialize()
        {
            if (IsInitialized)
            {
                lock (Sync)
                {
                    if (IsInitialized)
                    {
                        DoUninitialize();
                        IsInitialized = false;
                    }
                }
            }
        }
    }
}
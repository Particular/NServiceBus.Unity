namespace NServiceBus.Unity
{
    using System;
    using System.Threading;

    class SingletonInstanceStore
    {
        private object lockObj = new object();
        bool needsDisposing;
        object value;

        public object GetValue()
        {
            Monitor.Enter(lockObj);
            if (value != null)
            {
                Monitor.Exit(lockObj);
            }
            return value;
        }

        public void SetValue(object newValue)
        {
            value = newValue;
            needsDisposing = newValue is IDisposable;
            TryExit();
        }

        public void Recover()
        {
            TryExit();
        }

        public void Remove()
        {
            if (!needsDisposing)
            {
                value = null;
                return;
            }
            lock (lockObj)
            {
                if (value != null)
                {
                    ((IDisposable)value).Dispose();
                    value = null;
                }
            }
        }

        private void TryExit()
        {
            // Prevent first chance exception when abandoning a lock that has not been entered
            if (Monitor.IsEntered(lockObj))
            {
                try
                {
                    Monitor.Exit(lockObj);
                }
                catch (SynchronizationLockException)
                {
                    // Noop here - we don't hold the lock and that's ok.
                }
            }
        }        
    }
}
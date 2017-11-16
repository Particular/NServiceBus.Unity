namespace NServiceBus.Unity
{
    using System;
    using global::Unity.Exceptions;
    using global::Unity.Lifetime;

    [Janitor.SkipWeaving]
    class SingletonLifetimeManager : LifetimeManager, IRequiresRecovery, IDisposable
    {
        SingletonInstanceStore instanceStore;

        public SingletonLifetimeManager(SingletonInstanceStore instanceStore)
        {
            this.instanceStore = instanceStore;
        }

        public override object GetValue()
        {
            return instanceStore.GetValue();
        }

        public override void SetValue(object newValue)
        {
            instanceStore.SetValue(newValue);
        }

        public override void RemoveValue()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Recover()
        {
            instanceStore.Recover();
        }

        protected virtual void Dispose(bool disposing)
        {
            instanceStore.Remove();
        }
    }
}
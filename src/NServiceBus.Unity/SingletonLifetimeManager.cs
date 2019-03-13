/*namespace NServiceBus.Unity
{
    using System;
    using global::Unity.Exceptions;
    using global::Unity.Lifetime;

    [Janitor.SkipWeaving]
    //TODO: consider removing this implementation in favor of Unity.Lifetime.SingletonLifetimeManager
    class SingletonLifetimeManager : LifetimeManager, IRequiresRecovery, IDisposable
    {
        SingletonInstanceStore instanceStore;
        protected override LifetimeManager OnCreateLifetimeManager()
        {
            return new SingletonLifetimeManager(new SingletonInstanceStore());
        }

        public SingletonLifetimeManager(SingletonInstanceStore instanceStore)
        {
            this.instanceStore = instanceStore;
        }

        public override object GetValue(ILifetimeContainer container = null)
        {
            return instanceStore.GetValue();
        }

        public override void SetValue(object newValue, ILifetimeContainer container = null)
        {
            instanceStore.SetValue(newValue);
        }

        public override void RemoveValue(ILifetimeContainer container = null)
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
}*/
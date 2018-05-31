namespace NServiceBus.Unity
{
    using System;
    using global::Unity.Events;
    using global::Unity.Extension;
    using global::Unity.Lifetime;

    class RegisteringNotificationContainerExtension : UnityContainerExtension
    {
        readonly Action<Type, Type, LifetimeManager> onRegisteringCallback;
        readonly Action<Type, object, LifetimeManager> onRegisteringInstanceCallback;

        public RegisteringNotificationContainerExtension(
            Action<Type, Type, LifetimeManager> onRegisteringCallback,
            Action<Type, object, LifetimeManager> onRegisteringInstanceCallback)
        {
            this.onRegisteringCallback = onRegisteringCallback;
            this.onRegisteringInstanceCallback = onRegisteringInstanceCallback;
        }

        protected override void Initialize()
        {
            Context.Registering += OnContextRegistering;
            Context.RegisteringInstance += OnContextRegisteringInstance;
        }

        public override void Remove()
        {
            Context.Registering -= OnContextRegistering;
            Context.RegisteringInstance -= OnContextRegisteringInstance;
        }

        void OnContextRegistering(object sender, RegisterEventArgs args)
        {
            if (args.Name == null)
            {
                onRegisteringCallback(args.TypeFrom, args.TypeTo, args.LifetimeManager);
            }
        }

        void OnContextRegisteringInstance(object sender, RegisterInstanceEventArgs args)
        {
            if (args.Name == null)
            {
                onRegisteringInstanceCallback(args.RegisteredType, args.Instance, args.LifetimeManager);
            }
        }

    }
}
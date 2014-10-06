namespace NServiceBus.Unity
{
    using System;
    using Microsoft.Practices.Unity;

    class RegisteringNotificationContainerExtension : UnityContainerExtension
    {
        Action<Type, Type, LifetimeManager> onRegisteringCallback;

        public RegisteringNotificationContainerExtension(Action<Type, Type, LifetimeManager> onRegisteringCallback)
        {
            this.onRegisteringCallback = onRegisteringCallback;
        }

        protected override void Initialize()
        {
            Context.Registering += OnContextOnRegistering;
        }

        public override void Remove()
        {
            Context.Registering -= OnContextOnRegistering;
        }

        void OnContextOnRegistering(object sender, RegisterEventArgs args)
        {
            onRegisteringCallback(args.TypeFrom, args.TypeTo, args.LifetimeManager);
        }
    }
}
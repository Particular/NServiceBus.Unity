namespace ObjectBuilder.Unity.AcceptanceTests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;

    class ContainerDecorator : IUnityContainer
    {
        public ContainerDecorator(IUnityContainer decorated)
        {
            this.decorated = decorated;
        }

        public bool Disposed { get; private set; }

        public void Dispose()
        {
            decorated.Dispose();
            Disposed = true;
        }

        public IUnityContainer RegisterType(Type from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return decorated.RegisterType(from, to, name, lifetimeManager, injectionMembers);
        }

        public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
        {
            return decorated.RegisterInstance(t, name, instance, lifetime);
        }

        public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
        {
            return decorated.Resolve(t, name, resolverOverrides);
        }

        public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return decorated.ResolveAll(t, resolverOverrides);
        }

        public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
        {
            return decorated.BuildUp(t, existing, resolverOverrides);
        }

        public void Teardown(object o)
        {
            decorated.Teardown(o);
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return decorated.AddExtension(extension);
        }

        public object Configure(Type configurationInterface)
        {
            return decorated.Configure(configurationInterface);
        }

        public IUnityContainer RemoveAllExtensions()
        {
            return decorated.RemoveAllExtensions();
        }

        public IUnityContainer CreateChildContainer()
        {
            return decorated.CreateChildContainer();
        }

        public IUnityContainer Parent => decorated.Parent;

        public IEnumerable<ContainerRegistration> Registrations => decorated.Registrations;
        private IUnityContainer decorated;
    }
}
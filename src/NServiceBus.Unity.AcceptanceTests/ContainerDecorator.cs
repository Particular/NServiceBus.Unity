namespace ObjectBuilder.Unity.AcceptanceTests
{
    using System;
    using System.Collections.Generic;
    using global::Unity;
    using global::Unity.Extension;
    using global::Unity.Injection;
    using global::Unity.Lifetime;
    using global::Unity.Resolution;

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
        }

        public IUnityContainer AddExtension(UnityContainerExtension extension)
        {
            return decorated.AddExtension(extension);
        }

        public object Configure(Type configurationInterface)
        {
            return decorated.Configure(configurationInterface);
        }

        public IUnityContainer CreateChildContainer()
        {
            return decorated.CreateChildContainer();
        }

        public IUnityContainer RegisterType(Type typeFrom, Type typeTo, string name, ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            return decorated.RegisterType(typeFrom, typeTo, name, lifetimeManager, injectionMembers);
        }

        public IUnityContainer RegisterInstance(Type type, string name, object instance, IInstanceLifetimeManager lifetimeManager)
        {
            return decorated.RegisterInstance(type, name, instance, lifetimeManager);
        }

        public IUnityContainer RegisterFactory(Type type, string name, Func<IUnityContainer, Type, string, object> factory, IFactoryLifetimeManager lifetimeManager)
        {
            return decorated.RegisterFactory(type, name, factory, lifetimeManager);
        }

        public bool IsRegistered(Type type, string name)
        {
            return decorated.IsRegistered(type, name);
        }
        public IUnityContainer Parent => decorated.Parent;

        public IEnumerable<IContainerRegistration> Registrations => decorated.Registrations;
            
        private IUnityContainer decorated;

        
    }
}
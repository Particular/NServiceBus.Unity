namespace NServiceBus.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using global::Unity;
    using global::Unity.Extension;
    using global::Unity.Injection;
    using global::Unity.Lifetime;
    using global::Unity.Registration;
    using global::Unity.Resolution;
    using NUnit.Framework;
    using Unity;

    [TestFixture]
    public class DisposalTests
    {
        //[Test]
        //public void Owned_container_should_be_disposed()
        //{
        //    var fakeContainer = new FakeContainer();

        //    var container = new UnityObjectBuilder(fakeContainer, true);
        //    container.Dispose();

        //    Assert.True(fakeContainer.Disposed);
        //}

        //[Test]
        //public void Externally_owned_container_should_not_be_disposed()
        //{
        //    var fakeContainer = new FakeContainer();

        //    var container = new UnityObjectBuilder(fakeContainer, false);
        //    container.Dispose();

        //    Assert.False(fakeContainer.Disposed);
        //}

        class FakeContainer : IUnityContainer
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }

            public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
            {
                return null;
            }

            public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
            {
                return null;
            }

            public IUnityContainer CreateChildContainer()
            {
                return null;
            }

            public IUnityContainer RegisterType(Type typeFrom, Type typeTo, string name, ITypeLifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            {
                return null;
            }

            public IUnityContainer RegisterInstance(Type type, string name, object instance, IInstanceLifetimeManager lifetimeManager)
            {
                return null;
            }

            public IUnityContainer RegisterFactory(Type type, string name, Func<IUnityContainer, Type, string, object> factory, IFactoryLifetimeManager lifetimeManager)
            {
                return null;
            }

            public bool IsRegistered(Type type, string name)
            {
                throw new NotImplementedException();
            }

            public IUnityContainer Parent { get; }

            IEnumerable<IContainerRegistration> registrations = new List<IContainerRegistration>();

            IEnumerable<IContainerRegistration> IUnityContainer.Registrations => registrations;
        }
    }
}
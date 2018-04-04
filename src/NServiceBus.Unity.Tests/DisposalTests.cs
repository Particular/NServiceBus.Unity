namespace NServiceBus.Unity.Tests
{
    using System;
    using System.Collections.Generic;
    using global::Unity;
    using global::Unity.Extension;
    using global::Unity.Lifetime;
    using global::Unity.Registration;
    using global::Unity.Resolution;
    using NServiceBus.Unity;
    using NUnit.Framework;

    [TestFixture]
    public class DisposalTests
    {
        [Test]
        public void Owned_container_should_be_disposed()
        {
            var fakeContainer = new FakeContainer();

            var container = new UnityObjectBuilder(fakeContainer, true);
            container.Dispose();

            Assert.True(fakeContainer.Disposed);
        }

        [Test]
        public void Externally_owned_container_should_not_be_disposed()
        {
            var fakeContainer = new FakeContainer();

            var container = new UnityObjectBuilder(fakeContainer, false);
            container.Dispose();

            Assert.False(fakeContainer.Disposed);
        }

        class FakeContainer : IUnityContainer
        {
            public bool Disposed { get; private set; }

            public FakeContainer()
            {
                Registrations = new List<ContainerRegistration>();
            }

            public void Dispose()
            {
                Disposed = true;
            }

            public IUnityContainer RegisterType(Type @from, Type to, string name, LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
            {
                return null;
            }

            public IUnityContainer RegisterInstance(Type t, string name, object instance, LifetimeManager lifetime)
            {
                return null;
            }

            public object Resolve(Type t, string name, params ResolverOverride[] resolverOverrides)
            {
                return null;
            }

            public IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
            {
                yield break;
            }

            public object BuildUp(Type t, object existing, string name, params ResolverOverride[] resolverOverrides)
            {
                return null;
            }

            public void Teardown(object o)
            {
            }

            public IUnityContainer AddExtension(UnityContainerExtension extension)
            {
                return null;
            }

            public object Configure(Type configurationInterface)
            {
                return null;
            }

            public IUnityContainer RemoveAllExtensions()
            {
                return null;
            }

            public IUnityContainer CreateChildContainer()
            {
                return null;
            }
            public bool IsRegistered(Type type, string name)
            {
                return false;
            }

            public IUnityContainer Parent { get; }
            public IEnumerable<IContainerRegistration> Registrations { get; }
        }
    }
}
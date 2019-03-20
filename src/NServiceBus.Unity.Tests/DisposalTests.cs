namespace NServiceBus.Unity.Tests
{
    using System;
    using global::Unity;
    using global::Unity.Extension;
    using NUnit.Framework;
    using Unity;

    [TestFixture]
    public class DisposalTests
    {
        [Test]
        public void Owned_container_should_be_disposed()
        {
            var ownedContainer = new UnityContainer();
            var disposableExtension = new DisposableExtension();
            ownedContainer.AddExtension(disposableExtension);

            var container = new UnityObjectBuilder(ownedContainer, true);
            container.Dispose();

            Assert.True(disposableExtension.Disposed);
        }

        [Test]
        public void Externally_owned_container_should_not_be_disposed()
        {
            var externalContainer = new UnityContainer();
            var disposableExtension = new DisposableExtension();
            externalContainer.AddExtension(disposableExtension);

            var container = new UnityObjectBuilder(externalContainer, false);
            container.Dispose();

            Assert.False(disposableExtension.Disposed);
        }

        // Extensions implementing IDisposable are disposed as part of disposing the UnityContainer
        // so we can use this property to assert dispose calls.
        class DisposableExtension : IUnityContainerExtensionConfigurator, IDisposable
        {
            public IUnityContainer Container { get; }

            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }
    }
}
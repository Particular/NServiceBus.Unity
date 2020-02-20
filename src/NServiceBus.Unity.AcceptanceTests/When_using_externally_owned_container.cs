namespace ObjectBuilder.Unity.AcceptanceTests
{
    using System;
    using System.Threading.Tasks;
    using global::Unity;
    using global::Unity.Extension;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_using_externally_owned_container : NServiceBusAcceptanceTest
    {
        [Test]
        public async Task Should_shutdown_properly()
        {
            var container = new UnityContainer();
            var disposableExtension = new DisposableExtension();

            await Scenario.Define<ScenarioContext>()
#pragma warning disable 0618
                .WithEndpoint<Endpoint>(e => e.CustomConfig(config => config.UseContainer<UnityBuilder>(c => c.UseExistingContainer(container))))
#pragma warning restore 0618
                .Done(c => c.EndpointsStarted)
                .Run();

            Assert.IsFalse(disposableExtension.Disposed);
            Assert.DoesNotThrow(() => container.Dispose());

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

        class Endpoint : EndpointConfigurationBuilder
        {
            public Endpoint()
            {
                EndpointSetup<DefaultServer>();
            }
        }
    }
}
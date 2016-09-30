namespace NServiceBus.AcceptanceTests
{
    using Microsoft.Practices.Unity;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_using_externally_owned_container : NServiceBusAcceptanceTest
    {
        [Test]
        public void Should_shutdown_properly()
        {
            Scenario.Define<Context>()
                .WithEndpoint<Endpoint>()
                .Done(c => c.EndpointsStarted)
                .Run();

            Assert.IsFalse(Endpoint.Context.Decorator.Disposed);
            Assert.DoesNotThrow(() => Endpoint.Context.Scope.Dispose());
        }

        class Context : ScenarioContext
        {
            public ContainerDecorator Decorator { get; set; }
            public IUnityContainer Scope { get; set; }
        }

        class Endpoint : EndpointConfigurationBuilder
        {
            public static Context Context { get; set; }
            public Endpoint()
            {
                EndpointSetup<DefaultServer>(config =>
                {
                    Context = new Context();
                    
                    var container = new UnityContainer();
                    var scopeDecorator = new ContainerDecorator(container);

                    Context.Decorator = scopeDecorator;
                    Context.Scope = container;

                    config.UseContainer<UnityBuilder>(c => c.UseExistingContainer(Context.Decorator));
                });
            }
        }
    }
}
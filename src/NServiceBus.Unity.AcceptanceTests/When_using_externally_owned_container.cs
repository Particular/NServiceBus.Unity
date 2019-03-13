namespace ObjectBuilder.Unity.AcceptanceTests
{
    using System.Threading.Tasks;
    using global::Unity;
    using NServiceBus;
    using NServiceBus.AcceptanceTesting;
    using NServiceBus.AcceptanceTests;
    using NServiceBus.AcceptanceTests.EndpointTemplates;
    using NUnit.Framework;

    public class When_using_externally_owned_container : NServiceBusAcceptanceTest
    {
        //[Test]
        //public async Task Should_shutdown_properly()
        //{
        //    var context = await Scenario.Define<Context>()
        //        .WithEndpoint<Endpoint>()
        //        .Done(c => c.EndpointsStarted)
        //        .Run();

        //    Assert.IsFalse(context.Decorator.Disposed);
        //    Assert.DoesNotThrow(() => context.Container.Dispose());
        //}

        class Context : ScenarioContext
        {
            public IUnityContainer Container { get; set; }
            public ContainerDecorator Decorator { get; set; }
        }

        class Endpoint : EndpointConfigurationBuilder
        {
            public Endpoint()
            {
                EndpointSetup<DefaultServer>((config, desc) =>
                {
                    config.SendFailedMessagesTo("error");
                    var container = new UnityContainer();
                    var decorator = new ContainerDecorator(container);

                    config.UseContainer<UnityBuilder>(c => c.UseExistingContainer(decorator));

                    var context = (Context)desc.ScenarioContext;
                    context.Container = container;
                    context.Decorator = decorator;
                });
            }
        }
    }
}
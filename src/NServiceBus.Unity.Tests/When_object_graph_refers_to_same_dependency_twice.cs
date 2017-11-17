namespace NServiceBus.ContainerTests
{
    using global::Unity;
    using global::Unity.Lifetime;
    using NServiceBus.Unity;
    using NUnit.Framework;

    [TestFixture]
    public class When_object_graph_refers_to_same_dependency_twice
    {
        [Test]
        public void It_should_be_created_as_a_single_instance()
        {
            var conainer = new UnityContainer();

            conainer.RegisterType<Root, Root>();
            conainer.RegisterType<ServiceA, ServiceA>(new PerResolveLifetimeManager());
            conainer.RegisterType<ServiceB, ServiceB>();

            using (var builder = new UnityObjectBuilder(conainer))
            {
                var root = (Root)builder.Build(typeof(Root));

                Assert.AreSame(root.ServiceA, root.ServiceB.ServiceA);
            }
        }

        class Root
        {
            public Root(ServiceA serviceA, ServiceB nsbServiceB)
            {
                ServiceA = serviceA;
                ServiceB = nsbServiceB;
            }
            public ServiceA ServiceA { get; }
            public ServiceB ServiceB { get; }
        }

        class ServiceA
        {
        }

        class ServiceB
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public ServiceA ServiceA { get; set; }
        }
    }
}
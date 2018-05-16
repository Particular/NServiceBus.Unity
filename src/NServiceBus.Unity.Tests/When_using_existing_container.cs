namespace NServiceBus.Unity.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using NUnit.Framework;
    using global::Unity;

    [TestFixture]
    public class When_using_existing_container
    {
        [Test]
        public void Interfaces_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<ISomeInterface, SomeClass>();

            var builder = new UnityObjectBuilder(container);

            var result = builder.Build(typeof(ISomeInterface));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Existing_registrations_are_not_registered_twice()
        {
            var container = new UnityContainer();
            container.RegisterType<ISomeInterface, NamedService1>("1");
            container.RegisterType<ISomeInterface, NamedService2>("2");
            container.RegisterType<ISomeInterface, NamedService3>("3");

            var builder = new UnityObjectBuilder(container);

            var result = builder.BuildAll(typeof(ISomeInterface));

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void Existing_container_can_be_used_after_UnityObjectBuilder_is_disposed()
        {
            var container = new UnityContainer();
            container.RegisterType<ISomeInterface, SomeClass>();

            using (new UnityObjectBuilder(container)) { }

            var result = container.Resolve<ISomeInterface>();

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Named_instances_are_not_resolvable_via_Build()
        {
            var container = new UnityContainer();
            container.RegisterType<ISomeInterface, NamedService1>("1");

            var builder = new UnityObjectBuilder(container);

            Assert.Throws<ArgumentException>( () => builder.Build(typeof(ISomeInterface)));
        }

        [Test]
        public void Named_instances_registered_after_wrapping_container_are_not_resolvable_via_Build()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);

            container.RegisterType<ISomeInterface, NamedService1>("1");


            Assert.Throws<ArgumentException>(() => builder.Build(typeof(ISomeInterface)));
        }

        [Test]
        public void Interfaces_registered_in_the_container_after_wrapping_it_with_ObjectBuilder_are_resolvable_via_builder()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);

            container.RegisterType<ISomeInterface, SomeClass>();

            var result = builder.Build(typeof(ISomeInterface));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Abstract_classes_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<AbstractClass, SomeClass>();

            var builder = new UnityObjectBuilder(container);

            var result = builder.Build(typeof(AbstractClass));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Concrete_classes_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<SomeClass>();

            var builder = new UnityObjectBuilder(container);

            var result = builder.Build(typeof(SomeClass));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        [Test]
        public void Existing_instances_registred_in_the_container_can_be_injected_via_property()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);
            builder.Configure(typeof(PropertyInjectionHandler), DependencyLifecycle.InstancePerCall);

            container.RegisterInstance<ISomeInterface>(new SomeClass());

            var result = (PropertyInjectionHandler)builder.Build(typeof(PropertyInjectionHandler));

            Assert.IsNotNull(result.Dependency);
        }


        [Test]
        public void Existing_instances_registred_in_the_container_can_be_injected_via_constructor()
        {
            var container = new UnityContainer();

            var builder = new UnityObjectBuilder(container);
            builder.Configure(typeof(ConstructorInjectionHandler), DependencyLifecycle.InstancePerCall);

            container.RegisterInstance<ISomeInterface>(new SomeClass());

            var result = (ConstructorInjectionHandler)builder.Build(typeof(ConstructorInjectionHandler));

            Assert.IsNotNull(result.Dependency);
        }

        class AbstractClass
        {
        }

        class SomeClass : AbstractClass, ISomeInterface
        {
        }

        class PropertyInjectionHandler : IHandleMessages<object>
        {
            ISomeInterface dependency;

            public ISomeInterface Dependency
            {
                get
                {
                    return dependency;
                }
                // ReSharper disable once UnusedMember.Local
                set
                {
                    if (dependency != null)
                    {
                        throw new Exception("Dependency has already a value");
                    }

                    dependency = value;
                }
            }

            public Task Handle(object message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }

        }

        class ConstructorInjectionHandler : IHandleMessages<object>
        {
            public ConstructorInjectionHandler(ISomeInterface dependency)
            {
                Dependency = dependency;
            }

            public ISomeInterface Dependency { get; }

            public Task Handle(object message, IMessageHandlerContext context)
            {
                return Task.FromResult(0);
            }
        }

        interface ISomeInterface
        {
        }

        class NamedService1 : ISomeInterface
        {
        }

        class NamedService2 : ISomeInterface
        {
        }

        class NamedService3 : ISomeInterface
        {
        }
    }
}
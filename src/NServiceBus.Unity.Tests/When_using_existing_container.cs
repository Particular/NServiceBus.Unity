namespace NServiceBus.Unity.Tests
{
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

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

        class AbstractClass
        {
        }

        class SomeClass : AbstractClass, ISomeInterface
        {
        }

        interface ISomeInterface
        {
        }
    }
}
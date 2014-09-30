namespace NServiceBus.Unity.Tests
{
    using Microsoft.Practices.Unity;
    using NUnit.Framework;

    [TestFixture]
    public class When_using_existing_container
    {
        [Test]
        public void Components_registered_in_plain_container_are_resolvable_via_builder()
        {
            var container = new UnityContainer();
            container.RegisterType<SomeClass>();

            var builder = new UnityObjectBuilder();

            var result = builder.Build(typeof(ISomeInterface));

            Assert.IsInstanceOf<SomeClass>(result);
        }

        class SomeClass : ISomeInterface
        {

        }

        interface ISomeInterface
        {

        }
    }
}
namespace NServiceBus.Unity.Tests
{
    using ContainerTests;
    using NUnit.Framework;

    // Remove this tests once the NServiceBus.ContainerTests.Sources package has been upates to 7.2 as it contains this test
    [TestFixture]
    public class RecursiveTests
    {
        [Test]
        public void Resolving_recursive_types_with_singleton_does_not_stack_overflow()
        {
            using (var builder = TestContainerBuilder.ConstructBuilder())
            {
                builder.RegisterSingleton(typeof(RecursiveComponent), new RecursiveComponent());
                builder.Build(typeof(RecursiveComponent));
            }
        }

        [Test]
        public void Resolving_recursive_types_with_factory_does_not_stack_overflow()
        {
            using (var builder = TestContainerBuilder.ConstructBuilder())
            {
                builder.Configure(() => new RecursiveComponent(), DependencyLifecycle.SingleInstance);
                builder.Build(typeof(RecursiveComponent));
            }
        }
    }

    public class RecursiveComponent
    {
        public RecursiveComponent Instance { get; set; }
    }
}
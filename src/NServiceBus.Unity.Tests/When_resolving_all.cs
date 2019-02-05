namespace NServiceBus.Unity.Tests
{
    using System.Linq;
    using ContainerTests;
    using NUnit.Framework;

    [TestFixture]
    public class When_resolving_all
    {
        [Test]
        public void Singleton_references_returned_by_ResolveAll_and_Resolve_should_be_same_instance()
        {
            using (var builder = TestContainerBuilder.ConstructBuilder())
            {
                builder.Configure(typeof(AnotherSingletonComponent), DependencyLifecycle.SingleInstance);
                builder.Configure(typeof(SingletonComponent), DependencyLifecycle.SingleInstance);

                var resolveResult = builder.Build(typeof(SingletonComponent));
                var resolveAllResult = builder.BuildAll(typeof(ISingletonComponent)).OfType<SingletonComponent>().Single();

                Assert.AreSame(resolveResult, resolveAllResult);
            }
        }
    }
}
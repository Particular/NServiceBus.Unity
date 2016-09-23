namespace NServiceBus.Unity.Tests
{
    using NServiceBus.ContainerTests;
    using NUnit.Framework;

    [SetUpFixture]
    public class SetUpFixture
    {
        public SetUpFixture()
        {
            TestContainerBuilder.ConstructBuilder = () => new UnityObjectBuilder();
        }
    }
}
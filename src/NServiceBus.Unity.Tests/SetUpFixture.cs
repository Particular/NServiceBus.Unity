using NServiceBus.ContainerTests;
using NServiceBus.Unity;
using NUnit.Framework;

[SetUpFixture]
public class SetUpFixture
{
    [SetUp]
    public void Setup()
    {
        TestContainerBuilder.ConstructBuilder = () => new UnityObjectBuilder();
    }

}
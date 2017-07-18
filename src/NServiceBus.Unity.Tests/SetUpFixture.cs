using NServiceBus.ContainerTests;
using NServiceBus.Unity;
using NUnit.Framework;

[SetUpFixture]
public class SetUpFixture
{
    public SetUpFixture()
    {
        TestContainerBuilder.ConstructBuilder = () => new UnityObjectBuilder();
    }
}
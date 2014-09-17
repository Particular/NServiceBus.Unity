using NServiceBus;
using NServiceBus.Unity;
using NUnit.Framework;

[TestFixture]
public class BuildTests
{
    [Test]
    public void ShouldResolveInterfaces()
    {
        var builder = new UnityObjectBuilder();


        builder.Configure(typeof(SomeClass), DependencyLifecycle.InstancePerCall);


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
using NServiceBus;
using NServiceBus.Unity;
using NUnit.Framework;
using System.Linq;

[TestFixture]
public class BuildAllTests
{
    [Test]
    public void ShouldResolveAllInterfaces()
    {
        var builder = new UnityObjectBuilder();


        builder.Configure(typeof(First), DependencyLifecycle.InstancePerCall);

        builder.Configure(typeof(Second), DependencyLifecycle.InstancePerCall);

        var result = builder.BuildAll(typeof(ISomeInterface));

        Assert.AreEqual(2, result.Count());
    }

    class First : ISomeInterface
    {

    }

    class Second : ISomeInterface
    {

    }

    interface ISomeInterface
    {

    }
}
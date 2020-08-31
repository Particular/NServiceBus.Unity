namespace NServiceBus
{
    // An internal type referenced by the API approvals test as it can't reference obsoleted types.
    class UnityInternalType
    {
    }

    /// <summary>
    /// Unity Container
    /// </summary>
    [ObsoleteEx(
        Message = "Unity is no longer supported via the NServiceBus.Unity adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        RemoveInVersion = "12.0.0",
        TreatAsErrorFromVersion = "11.0.0")]
    public class UnityBuilder
    {
    }

    /// <summary>
    /// Extensions for unity specific config
    /// </summary>
    [ObsoleteEx(
        Message = "Unity is no longer supported via the NServiceBus.Unity adapter. NServiceBus directly supports all containers compatible with Microsoft.Extensions.DependencyInjection.Abstractions via the externally managed container mode.",
        RemoveInVersion = "12.0.0",
        TreatAsErrorFromVersion = "11.0.0")]
    public static class UnityConfigExtensions
    {
    }
}
#pragma warning disable 1591
// ReSharper disable UnusedParameter.Global

namespace NServiceBus
{
    using System;
    using Microsoft.Practices.Unity;

    [ObsoleteEx(
        TreatAsErrorFromVersion = "6.0",
        RemoveInVersion = "7.0",
        ReplacementTypeOrMember = "BusConfiguration.UseContainer<UnityBuilder>()"
        )]
    public static class ConfigureUnityBuilder
    {
        [ObsoleteEx(
            TreatAsErrorFromVersion = "6.0",
            RemoveInVersion = "7.0",
            ReplacementTypeOrMember = "BusConfiguration.UseContainer<UnityBuilder>()")]
        public static Configure UnityBuilder(this Configure config)
        {
            throw new NotImplementedException();
        }

        [ObsoleteEx(
            TreatAsErrorFromVersion = "6.0",
            RemoveInVersion = "7.0",
            ReplacementTypeOrMember = "BusConfiguration.UseContainer<UnityBuilder>(b => b.UseExistingContainer(applicationContext))")]
        public static Configure UnityBuilder(this Configure config, IUnityContainer container)
        {
            throw new NotImplementedException();
        }
    }
}
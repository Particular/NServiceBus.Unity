#pragma warning disable 1591
// ReSharper disable UnusedParameter.Global
namespace NServiceBus
{
    using System;
    using Microsoft.Practices.Unity;

    [ObsoleteEx(
        Message = "Replace with Use `configuration.UseContainer<UnityBuilder>()`, where `configuration` is an instance of type `BusConfiguration`.",
        TreatAsErrorFromVersion = "6.0",
        RemoveInVersion = "7.0")]
    public static class ConfigureUnityBuilder
    {
        [ObsoleteEx(
            Message = "Replace with Use `configuration.UseContainer<UnityBuilder>()`, where `configuration` is an instance of type `BusConfiguration`.",
            TreatAsErrorFromVersion = "6.0",
            RemoveInVersion = "7.0")]
        public static Configure UnityBuilder(this Configure config)
        {
            throw new NotImplementedException();
        }

        [ObsoleteEx(
            Message = "Use `configuration.UseContainer<UnityBuilder>(b => b.UseExistingContainer(applicationContext))`, where `configuration` is an instance of type `BusConfiguration`.",
            TreatAsErrorFromVersion = "6.0",
            RemoveInVersion = "7.0")]
        public static Configure UnityBuilder(this Configure config, IUnityContainer container)
        {
            throw new NotImplementedException();
        }
    }
}

﻿[assembly: System.CLSCompliantAttribute(true)]
[assembly: System.Runtime.CompilerServices.InternalsVisibleToAttribute(@"NServiceBus.Unity.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100dde965e6172e019ac82c2639ffe494dd2e7dd16347c34762a05732b492e110f2e4e2e1b5ef2d85c848ccfb671ee20a47c8d1376276708dc30a90ff1121b647ba3b7259a6bc383b2034938ef0e275b58b920375ac605076178123693c6c4f1331661a62eba28c249386855637780e3ff5f23a6d854700eaa6803ef48907513b92")]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.5.2", FrameworkDisplayName=".NET Framework 4.5.2")]
namespace NServiceBus
{
    [ObsoleteExAttribute(Message="NServiceBus.Unity has been deprecated. Using another container is advised.  Pleas" +
        "e see upgrade guide for a list of supported containers.", TreatAsErrorFromVersion="8.0")]
    public class UnityBuilder : NServiceBus.Container.ContainerDefinition
    {
        public UnityBuilder() { }
        [ObsoleteExAttribute(Message="NServiceBus.Unity has been deprecated. Using another container is advised.  Pleas" +
            "e see upgrade guide for a list of supported containers.", TreatAsErrorFromVersion="8.0")]
        public override NServiceBus.ObjectBuilder.Common.IContainer CreateContainer(NServiceBus.Settings.ReadOnlySettings settings) { }
    }
    [ObsoleteExAttribute(Message="NServiceBus.Unity has been deprecated. Using another container is advised.  Pleas" +
        "e see upgrade guide for a list of supported containers.", TreatAsErrorFromVersion="8.0")]
    public class static UnityConfigExtensions
    {
        [ObsoleteExAttribute(Message="NServiceBus.Unity has been deprecated. Using another container is advised.  Pleas" +
            "e see upgrade guide for a list of supported containers.", TreatAsErrorFromVersion="8.0")]
        public static void UseExistingContainer(this NServiceBus.Container.ContainerCustomizations customizations, Microsoft.Practices.Unity.IUnityContainer existingContainer) { }
    }
}
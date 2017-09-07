namespace NServiceBus
{
    using Container;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Extensions for unity specific config
    /// </summary>
    [ObsoleteEx(Message = obsolete.Message,
        TreatAsErrorFromVersion = "9.0")]
    public static class UnityConfigExtensions
    {
        /// <summary>
        /// Use the a pre-configured <see cref="IUnityContainer"/>.
        /// </summary>
        [ObsoleteEx(Message = obsolete.Message,
            TreatAsErrorFromVersion = "9.0")]
        public static void UseExistingContainer(this ContainerCustomizations customizations, IUnityContainer existingContainer)
        {
            customizations.Settings.Set<UnityBuilder.ContainerHolder>(new UnityBuilder.ContainerHolder(existingContainer));
        }
    }
}
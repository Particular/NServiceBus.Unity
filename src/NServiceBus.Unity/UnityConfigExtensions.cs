namespace NServiceBus
{
    using Container;
    using global::Unity;

    /// <summary>
    /// Extensions for unity specific config
    /// </summary>
    public static class UnityConfigExtensions
    {
        /// <summary>
        /// Use the a pre-configured <see cref="IUnityContainer"/>.
        /// </summary>
        public static void UseExistingContainer(this ContainerCustomizations customizations, IUnityContainer existingContainer)
        {
            customizations.Settings.Set<UnityBuilder.ContainerHolder>(new UnityBuilder.ContainerHolder(existingContainer));
        }
    }
}
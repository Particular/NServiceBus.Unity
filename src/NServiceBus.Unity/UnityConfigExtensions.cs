namespace NServiceBus
{
    using Container;
    using global::Unity;

    /// <summary>
    /// Extensions for unity specific config
    /// </summary>
    public static class UnityConfigExtensions
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
                              /// <summary>
                              /// Use the a pre-configured <see cref="IUnityContainer"/>.
                              /// </summary>
        public static void UseExistingContainer(this ContainerCustomizations customizations, IUnityContainer existingContainer)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            customizations.Settings.Set<UnityBuilder.ContainerHolder>(new UnityBuilder.ContainerHolder(existingContainer));
        }
    }
}
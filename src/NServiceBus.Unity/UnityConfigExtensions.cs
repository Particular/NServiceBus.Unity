namespace NServiceBus
{
    using Container;
    using Microsoft.Practices.Unity;

    /// <summary>
    /// Extensions for unity specific config
    /// </summary>
    public static class UnityConfigExtensions
    {
        /// <summary>
        /// Use the a pre-configured unity container.
        /// </summary>
        /// <param name="customizations"></param>
        /// <param name="existingContainer">The existing container to use.</param>
        public static void UseExistingContainer(this ContainerCustomizations customizations, IUnityContainer existingContainer)
        {
            customizations.Settings.Set("ExistingContainer", existingContainer);
        }
    }
}
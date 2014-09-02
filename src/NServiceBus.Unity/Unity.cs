namespace NServiceBus
{
    using Container;
    using Microsoft.Practices.Unity;
    using Settings;
    using Unity;

    /// <summary>
    /// Autofac Container
    /// </summary>
    public class UnityBuilder : ContainerDefinition
    {
        /// <summary>
        ///     Implementers need to new up a new container.
        /// </summary>
        /// <param name="settings">The settings to check if an existing container exists.</param>
        /// <returns>The new container wrapper.</returns>
        public override ObjectBuilder.Common.IContainer CreateContainer(ReadOnlySettings settings)
        {
            IUnityContainer existingContainer;

            if (settings.TryGet("ExistingContainer", out existingContainer))
            {
                return new UnityObjectBuilder(existingContainer);

            }

            return new UnityObjectBuilder();
        }
    }
}
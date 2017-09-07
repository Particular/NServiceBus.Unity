namespace NServiceBus
{
    using Container;
    using Microsoft.Practices.Unity;
    using Settings;
    using Unity;

    /// <summary>
    /// Unity Container
    /// </summary>
    [ObsoleteEx(Message = obsolete.Message,
        TreatAsErrorFromVersion = "9.0")]
    public class UnityBuilder : ContainerDefinition
    {
        /// <summary>
        ///     Implementers need to new up a new container.
        /// </summary>
        /// <param name="settings">The settings to check if an existing container exists.</param>
        /// <returns>The new container wrapper.</returns>
        [ObsoleteEx(Message = obsolete.Message,
            TreatAsErrorFromVersion = "9.0")]
        public override ObjectBuilder.Common.IContainer CreateContainer(ReadOnlySettings settings)
        {
            ContainerHolder containerHolder;

            if (settings.TryGet(out containerHolder))
            {
                return new UnityObjectBuilder(containerHolder.ExistingContainer);

            }

            return new UnityObjectBuilder();
        }

        internal class ContainerHolder
        {
            public ContainerHolder(IUnityContainer container)
            {
                ExistingContainer = container;
            }

            public IUnityContainer ExistingContainer { get; }
        }
    }
}
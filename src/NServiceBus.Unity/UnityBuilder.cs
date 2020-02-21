namespace NServiceBus
{
    using Container;
    using global::Unity;
    using Settings;
    using Unity;

    /// <summary>
    /// Unity Container
    /// </summary>
    [ObsoleteEx(
        Message = "Support for external dependency injection containers is no longer provided by NServiceBus adapters for each container library. Instead, the NServiceBus.Extensions.DependencyInjection library provides the ability to use any container that conforms to the Microsoft.Extensions.DependencyInjection container abstraction.",
        RemoveInVersion = "12.0.0",
        TreatAsErrorFromVersion = "11.0.0")]
    public class UnityBuilder : ContainerDefinition
    {
        /// <summary>
        ///     Implementers need to new up a new container.
        /// </summary>
        /// <param name="settings">The settings to check if an existing container exists.</param>
        /// <returns>The new container wrapper.</returns>
        public override ObjectBuilder.Common.IContainer CreateContainer(ReadOnlySettings settings)
        {
            if (settings.TryGet(out ContainerHolder containerHolder))
            {
                settings.AddStartupDiagnosticsSection("NServiceBus.Unity", new
                {
                    UsingExistingContainer = true
                });

                return new UnityObjectBuilder(containerHolder.ExistingContainer);

            }

            settings.AddStartupDiagnosticsSection("NServiceBus.Unity", new
            {
                UsingExistingContainer = false
            });

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
namespace NServiceBus.Unity
{
    using System.Linq;
    using global::Unity.Builder;
    using global::Unity.Extension;
    using global::Unity.Registration;

    class PropertyInjectionContainerExtension : UnityContainerExtension
    {
        UnityObjectBuilder unityObjectBuilder;
        PropertyInjectionBuilderStrategy strategy;

        public PropertyInjectionContainerExtension(UnityObjectBuilder unityObjectBuilder)
        {
            this.unityObjectBuilder = unityObjectBuilder;
        }

        protected override void Initialize()
        {
            strategy = new PropertyInjectionBuilderStrategy(unityObjectBuilder);
            Context.Strategies.Add(strategy, UnityBuildStage.Initialization);

            var propertyInjectionBuilderStrategies = new[] { strategy };
            foreach (var registration in Context.Lifetime.Container.Registrations.OfType<ContainerRegistration>())
            {
                registration.BuildChain = registration.BuildChain.Concat(propertyInjectionBuilderStrategies).ToArray();
            }
        }

        internal void Stop()
        {
            strategy.Stop();
        }
    }
}

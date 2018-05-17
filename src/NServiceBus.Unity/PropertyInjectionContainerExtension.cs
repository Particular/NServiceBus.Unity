namespace NServiceBus.Unity
{
    using System.Collections.Generic;
    using System.Linq;
    using global::Unity.Builder;
    using global::Unity.Builder.Strategy;
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

            foreach (var registration in Context.Lifetime.Container.Registrations.OfType<ContainerRegistration>())
            {
                if (registration.BuildChain.IsReadOnly)
                {
                    registration.BuildChain = new List<BuilderStrategy>(registration.BuildChain);
                }

                registration.BuildChain.Add(strategy);
            }
        }

        internal void Stop()
        {
            strategy.Stop();
        }
    }
}

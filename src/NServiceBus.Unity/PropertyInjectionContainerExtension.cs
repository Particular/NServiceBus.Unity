namespace NServiceBus.Unity
{
    using System;
    using global::Unity.Builder;
    using global::Unity.Extension;
    using global::Unity.Registration;

    class PropertyInjectionContainerExtension : UnityContainerExtension
    {
        UnityObjectBuilder unityObjectBuilder;
        PropertyInjectionBuilderStrategy propertyInjectionStrategy;

        public PropertyInjectionContainerExtension(UnityObjectBuilder unityObjectBuilder)
        {
            this.unityObjectBuilder = unityObjectBuilder;
        }

        protected override void Initialize()
        {
            propertyInjectionStrategy = new PropertyInjectionBuilderStrategy(unityObjectBuilder);
            Context.Strategies.Add(propertyInjectionStrategy, UnityBuildStage.Initialization);

            foreach (ContainerRegistration registration in Context.Lifetime.Container.Registrations)
            {
                registration.BuildChain.Add(propertyInjectionStrategy);
            }
        }

        public void Stop()
        {
            propertyInjectionStrategy.Stop();
        }
    }
}
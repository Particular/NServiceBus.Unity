namespace NServiceBus.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

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
        }

        public override void Remove()
        {
            propertyInjectionStrategy.Stop();
        }
    }
}
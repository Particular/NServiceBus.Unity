namespace NServiceBus.Unity
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

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
        }

        public void Stop()
        {
            strategy.Stop();
        }
    }
}
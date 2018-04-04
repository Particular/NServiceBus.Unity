namespace NServiceBus.Unity
{
    using global::Unity.Builder;
    using global::Unity.Extension;

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

        internal void Stop()
        {
            strategy.Stop();
        }
    }
}

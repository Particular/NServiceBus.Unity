namespace NServiceBus.Unity
{
    using global::Unity.Builder;
    using global::Unity.Extension;

    class PropertyInjectionContainerExtension : UnityContainerExtension
    {
        UnityObjectBuilder unityObjectBuilder;

        public PropertyInjectionContainerExtension(UnityObjectBuilder unityObjectBuilder)
        {
            this.unityObjectBuilder = unityObjectBuilder;
        }

        protected override void Initialize()
        {
            Context.Strategies.Add(new PropertyInjectionBuilderStrategy(unityObjectBuilder), UnityBuildStage.Initialization);
        }
    }
}
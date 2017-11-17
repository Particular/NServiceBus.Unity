namespace NServiceBus.Unity
{
    using global::Unity.Builder;
    using global::Unity.Builder.Strategy;

    class PropertyInjectionBuilderStrategy : BuilderStrategy
    {
        UnityObjectBuilder unityContainer;

        public PropertyInjectionBuilderStrategy(UnityObjectBuilder unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            var type = context.BuildKey.Type;
            var target = context.Existing;
            if (!type.FullName.StartsWith("Microsoft.Practices"))
            {
                unityContainer.SetProperties(target.GetType(), target, t => context.NewBuildUp(new NamedTypeBuildKey(t)));
            }
        }
    }
}
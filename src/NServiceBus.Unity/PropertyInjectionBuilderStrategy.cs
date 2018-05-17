namespace NServiceBus.Unity
{
    using global::Unity.Builder;
    using global::Unity.Builder.Strategy;

    class PropertyInjectionBuilderStrategy : BuilderStrategy
    {
        UnityObjectBuilder unityContainer;
        bool mustSetProperties = true;

        public PropertyInjectionBuilderStrategy(UnityObjectBuilder unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            if (mustSetProperties)
            {
                var type = context.BuildKey.Type;
                var target = context.Existing;
                if (!type.FullName.StartsWith("Microsoft.Practices") || !type.FullName.StartsWith("Unity."))
                {
                    unityContainer.SetProperties(target.GetType(), target, t => context.NewBuildUp(t, null));
                }
            }
        }

        internal void Stop()
        {
            mustSetProperties = false;
        }
    }
}
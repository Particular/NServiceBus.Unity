namespace NServiceBus.Unity
{
    using Microsoft.Practices.ObjectBuilder2;

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
namespace NServiceBus.Unity
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    class PropertyInjectionBuilderStrategy : BuilderStrategy
    {
        UnityObjectBuilder unityContainer;

        public PropertyInjectionBuilderStrategy(UnityObjectBuilder unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        private IUnityContainer GetUnityFromBuildContext(IBuilderContext context)
        {
            var lifetime = context.Policies.Get<ILifetimePolicy>(NamedTypeBuildKey.Make<IUnityContainer>());
            return lifetime.GetValue() as IUnityContainer;
        }

        private bool IsCorrectContext(IBuilderContext context)
        {
            var container = this.GetUnityFromBuildContext(context);
            return this.unityContainer.IsCorrectContext(container);
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            var type = context.BuildKey.Type;
            var target = context.Existing;
            if (!type.FullName.StartsWith("Microsoft.Practices") && this.IsCorrectContext(context))
            {
                unityContainer.SetProperties(target.GetType(), target);
            }
        }
    }
}
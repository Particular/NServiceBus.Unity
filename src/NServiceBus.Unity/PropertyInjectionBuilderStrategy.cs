namespace NServiceBus.Unity
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;

    class PropertyInjectionBuilderStrategy : BuilderStrategy
    {
        UnityObjectBuilder unityContainer;
        bool mustInjectProperties = true;

        public PropertyInjectionBuilderStrategy(UnityObjectBuilder unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        private IUnityContainer GetUnityFromBuildContext(IBuilderContext context)
        {
            var lifetime = context.Policies.Get<ILifetimePolicy>(NamedTypeBuildKey.Make<IUnityContainer>());
            return lifetime.GetValue() as IUnityContainer;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            if (mustInjectProperties)
            {
                var type = context.BuildKey.Type;
                var target = context.Existing;
                var container = GetUnityFromBuildContext(context);

                if (!type.FullName.StartsWith("Microsoft.Practices"))
                {
                    unityContainer.SetProperties(target.GetType(), target, container);
                }
            }
        }

        public void Stop()
        {
            mustInjectProperties = false;
        }
    }
}
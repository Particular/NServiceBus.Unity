//namespace NServiceBus.Unity
//{
//    using global::Unity.Builder;
//    using global::Unity.Builder.Strategy;
//    using global::Unity.Strategies;

//    class PropertyInjectionBuilderStrategy : BuilderStrategy
//    {
//        UnityObjectBuilder unityContainer;
//        bool mustSetProperties = true;

//        public PropertyInjectionBuilderStrategy(UnityObjectBuilder unityContainer)
//        {
//            this.unityContainer = unityContainer;
//        }

//        public override void PreBuildUp(ref BuilderContext context)
//        {
//            if (mustSetProperties)
//            {
//                var type = context.Type;
//                var target = context.Existing;
//                if (!type.FullName.StartsWith("Microsoft.Practices") || !type.FullName.StartsWith("Unity."))
//                {
//                    context.Resolve()
//                    unityContainer.SetProperties(target.GetType(), target, t => context.NewBuildUp(t, null));
//                }
//            }
//        }
//        internal void Stop()
//        {
//            mustSetProperties = false;
//        }
//    }
//}
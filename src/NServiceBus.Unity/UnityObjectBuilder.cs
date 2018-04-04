namespace NServiceBus.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBus.ObjectBuilder.Common;
    using global::Unity;
    using global::Unity.Injection;
    using global::Unity.Lifetime;

    class UnityObjectBuilder : IContainer
    {
        public UnityObjectBuilder()
            : this(new UnityContainer(), true)
        {
        }
        public UnityObjectBuilder(IUnityContainer container)
            : this(container, false)
        {
        }

        public UnityObjectBuilder(IUnityContainer container, bool owned)
            : this(container, _ => false, owned)
        {
            container.AddExtension(new RegisteringNotificationContainerExtension(
                (from, to, lifetime) => defaultInstances.Add(from),
                (from, to, lifetime) => defaultInstances.Add(from)));

            foreach (var registration in container.Registrations)
            {
                var implementationType = registration.MappedToType;
                //Register the fact that user registered a default instance himself (only if not a named registration)
                if (!implementationType.IsAbstract && !implementationType.IsInterface && registration.Name == null)
                {
                    defaultInstances.Add(registration.RegisteredType);
                }
            }
        }

        UnityObjectBuilder(IUnityContainer container, Func<Type, bool> ancestorsHaveDefaultInstanceOf, bool owned)
        {
            this.owned = owned;
            this.container = container;
            defaultInstances = new DefaultInstances();
            this.ancestorsHaveDefaultInstanceOf = ancestorsHaveDefaultInstanceOf;

            var propertyInjectionExtension = this.container.Configure<PropertyInjectionContainerExtension>();
            if (propertyInjectionExtension == null)
            {
                this.container.AddExtension(new PropertyInjectionContainerExtension(this));
            }
        }

        public void Dispose()
        {
            //Injected at compile time
        }

        void DisposeManaged()
        {
            if (!owned)
            {
                container.Configure<PropertyInjectionContainerExtension>()?.Stop();

                return;
            }

            container?.Dispose();
        }

        public IContainer BuildChildContainer()
        {
            return new UnityObjectBuilder(container.CreateChildContainer(), HasDefaultInstanceOf, true);
        }

        public object Build(Type typeToBuild)
        {
            if (!HasDefaultInstanceOf(typeToBuild))
            {
                throw new ArgumentException(typeToBuild + " is not registered in the container");
            }

            return container.Resolve(typeToBuild);
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            if (HasDefaultInstanceOf(typeToBuild))
            {
                yield return container.Resolve(typeToBuild);
            }
            foreach (var component in container.ResolveAll(typeToBuild))
            {
                yield return component;
            }
        }

        public void Configure(Type concreteComponent, DependencyLifecycle dependencyLifecycle)
        {
            if (HasComponent(concreteComponent))
            {
                return;
            }
            SingletonInstanceStore instanceStore = null;
            RegisterDefaultInstances(concreteComponent, () => GetLifetimeManager(dependencyLifecycle, ref instanceStore));
        }

        public void Configure<T>(Func<T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            var componentType = typeof(T);

            if (HasComponent(componentType))
            {
                return;
            }

            var serviceTypes = GetAllServiceTypesFor(componentType);
            SingletonInstanceStore instanceStore = null;
            foreach (var t in serviceTypes)
            {
                if (HasDefaultInstanceOf(t))
                {
                    container.RegisterType(t, Guid.NewGuid().ToString(), GetLifetimeManager(dependencyLifecycle, ref instanceStore),
                        new InjectionFactory(unityContainer => componentFactory()));
                }
                else
                {
                    container.RegisterType(t, GetLifetimeManager(dependencyLifecycle, ref instanceStore), new InjectionFactory(unityContainer => componentFactory()));
                    defaultInstances.Add(t);
                }
            }
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            defaultInstances.Add(lookupType);
            container.RegisterType(lookupType, new SingletonLifetimeManager(new SingletonInstanceStore()), new InjectionFactory(unityContainer => instance));
        }

        public bool HasComponent(Type componentType)
        {
            return HasDefaultInstanceOf(componentType);
        }

        public void Release(object instance)
        {
        }

        bool HasDefaultInstanceOf(Type typeToBuild)
        {
            return ancestorsHaveDefaultInstanceOf(typeToBuild) || defaultInstances.Contains(typeToBuild);
        }

        void RegisterDefaultInstances(Type concreteComponent, Func<LifetimeManager> lifetimeManagerFactory)
        {
            var serviceTypes = GetAllServiceTypesFor(concreteComponent);

            foreach (var t in serviceTypes)
            {
                RegisterDefaultInstance(concreteComponent, t, lifetimeManagerFactory);
            }
        }

        void RegisterDefaultInstance(Type concreteComponent, Type serviceType, Func<LifetimeManager> lifetimeManagerFactory)
        {
            if (defaultInstances.Contains(serviceType))
            {
                container.RegisterType(serviceType, concreteComponent, Guid.NewGuid().ToString(), lifetimeManagerFactory());
            }
            else
            {
                container.RegisterType(serviceType, concreteComponent, lifetimeManagerFactory());
                defaultInstances.Add(serviceType);
            }
        }

        static IEnumerable<Type> GetAllServiceTypesFor(Type t)
        {
            if (t == null)
            {
                yield break;
            }

            yield return t;
            // ReSharper disable ConditionIsAlwaysTrueOrFalse
            foreach (var nonSystemInterface in t.GetInterfaces().Where(x => x.FullName != null && !x.FullName.StartsWith("System.")))
            {
                yield return nonSystemInterface;
            }
            // ReSharper restore ConditionIsAlwaysTrueOrFalse
        }

        static LifetimeManager GetLifetimeManager(DependencyLifecycle dependencyLifecycle, ref SingletonInstanceStore instanceStore)
        {
            switch (dependencyLifecycle)
            {
                case DependencyLifecycle.InstancePerCall:
                    return new TransientLifetimeManager();
                case DependencyLifecycle.SingleInstance:
                    if (instanceStore == null)
                    {
                        instanceStore = new SingletonInstanceStore();
                    }
                    return new SingletonLifetimeManager(instanceStore);
                case DependencyLifecycle.InstancePerUnitOfWork:
                    return new HierarchicalLifetimeManager();
            }
            throw new ArgumentException("Unhandled lifecycle - " + dependencyLifecycle);
        }

        public void SetProperties(Type type, object target, Func<Type, object> resolveMethod)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                if (HasDefaultInstanceOf(property.PropertyType))
                {
                    property.SetValue(target, resolveMethod(property.PropertyType), null);
                }
            }
        }

        Func<Type, bool> ancestorsHaveDefaultInstanceOf;
        IUnityContainer container;
        DefaultInstances defaultInstances;
        private bool owned;
    }
}

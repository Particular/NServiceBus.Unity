namespace NServiceBus.Unity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using ObjectBuilder.Common;

    class UnityObjectBuilder : IContainer
    {
        IUnityContainer container;
        Dictionary<Type, Dictionary<string, object>> configuredProperties = new Dictionary<Type, Dictionary<string, object>>();
        DefaultInstances defaultInstances;

        public UnityObjectBuilder()
            : this(new UnityContainer())
        {
        }

        public UnityObjectBuilder(IUnityContainer container)
            : this(container, new DefaultInstances())
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

        UnityObjectBuilder(IUnityContainer container, DefaultInstances defaultInstances)
        {
            this.container = container;
            this.defaultInstances = defaultInstances;

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

        public IContainer BuildChildContainer()
        {
            return new UnityObjectBuilder(container.CreateChildContainer(), defaultInstances);
        }

        public object Build(Type typeToBuild)
        {
            if (!defaultInstances.Contains(typeToBuild))
            {
                throw new ArgumentException(typeToBuild + " is not registered in the container");
            }

            return container.Resolve(typeToBuild);
        }

        public IEnumerable<object> BuildAll(Type typeToBuild)
        {
            if (defaultInstances.Contains(typeToBuild))
            {
                yield return container.Resolve(typeToBuild);
            }
            foreach (var component in container.ResolveAll(typeToBuild))
            {
                yield return component;
            }
        }

        internal bool IsCorrectContext(IUnityContainer unityContainer)
        {
            return object.ReferenceEquals(unityContainer, this.container);
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
                if (defaultInstances.Contains(t))
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

        public void ConfigureProperty(Type concreteComponent, string property, object value)
        {
            Dictionary<string, object> properties;
            if (!configuredProperties.TryGetValue(concreteComponent, out properties))
            {
                configuredProperties[concreteComponent] = properties = new Dictionary<string, object>();
            }

            properties[property] = value;
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            defaultInstances.Add(lookupType);
            container.RegisterType(lookupType, new SingletonLifetimeManager(new SingletonInstanceStore()), new InjectionFactory(unityContainer => instance));
        }

        public bool HasComponent(Type componentType)
        {
            return defaultInstances.Contains(componentType);
        }

        public void Release(object instance)
        {
            //Not sure if I need to call this or not!
            container.Teardown(instance);
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

        public void SetProperties(Type type, object target)
        {
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                if (defaultInstances.Contains(property.PropertyType))
                {
                    property.SetValue(target, container.Resolve(property.PropertyType), null);
                }

                Dictionary<string, object> configuredProperty;
                if (configuredProperties.TryGetValue(type, out configuredProperty))
                {
                    object value;
                    if (configuredProperty.TryGetValue(property.Name, out value))
                    {
                        property.SetValue(target, value, null);

                    }
                }
            }
        }
    }
}

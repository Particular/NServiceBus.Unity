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
        Dictionary<Type, List<Tuple<string, object>>> configuredProperties = new Dictionary<Type, List<Tuple<string, object>>>();
        DefaultInstances defaultInstances;
        /// <summary>
        /// Instantiates the class with a new <see cref="UnityContainer"/>.
        /// </summary>
        public UnityObjectBuilder()
            : this(new UnityContainer())
        {
        }

        /// <summary>
        /// Instantiates the class saving the given container.
        /// </summary>
        public UnityObjectBuilder(IUnityContainer container)
            : this(container,new DefaultInstances())
        {
        }
        private UnityObjectBuilder(IUnityContainer container, DefaultInstances defaultInstances)
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

        /// <summary>
        /// Returns a child instance of the container to facilitate deterministic disposal
        /// of all resources built by the child container.
        /// </summary>
        public IContainer BuildChildContainer()
        {
            return new UnityObjectBuilder(container.CreateChildContainer(),defaultInstances);
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
                foreach (var component in container.ResolveAll(typeToBuild))
                {
                    yield return component;
                }
            }
        }

        public void Configure(Type concreteComponent, DependencyLifecycle dependencyLifecycle)
        {
            if (HasComponent(concreteComponent))
            {
                return;
            }

            var interfaces = GetAllServiceTypesFor(concreteComponent);
      
            foreach (var t in interfaces)
            {
                if (defaultInstances.Contains(t))
                {
                    container.RegisterType(t, concreteComponent, Guid.NewGuid().ToString(), GetLifetimeManager(dependencyLifecycle));
                }
                else
                {
                    container.RegisterType(t, concreteComponent, GetLifetimeManager(dependencyLifecycle));
                    defaultInstances.Add(t);
                }
            }
        }

        public void Configure<T>(Func<T> componentFactory, DependencyLifecycle dependencyLifecycle)
        {
            var componentType = typeof (T);

            if (HasComponent(componentType))
            {
                return;
            }

           var interfaces = GetAllServiceTypesFor(componentType);

           foreach (var t in interfaces)
           {
               if (defaultInstances.Contains(t))
               {
                   container.RegisterType(t, Guid.NewGuid().ToString(), GetLifetimeManager(dependencyLifecycle), 
                       new InjectionFactory(unityContainer => componentFactory()));
               }
               else
               {
                   container.RegisterType(t, GetLifetimeManager(dependencyLifecycle), new InjectionFactory(unityContainer => componentFactory()));
                   defaultInstances.Add(t);
               }
           }
        }

        public void ConfigureProperty(Type concreteComponent, string property, object value)
        {
            List<Tuple<string, object>> properties;
            if (!configuredProperties.TryGetValue(concreteComponent, out properties))
            {
                configuredProperties[concreteComponent] = properties = new List<Tuple<string, object>>();
            }

            properties.Add(new Tuple<string, object>(property, value));
        }

        public void RegisterSingleton(Type lookupType, object instance)
        {
            defaultInstances.Add(lookupType);
            container.RegisterInstance(lookupType, instance);
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
                return new List<Type>();
            }

            return new List<Type>(t.GetInterfaces().Where(x => x.FullName != null && !x.FullName.StartsWith("System.")))
                   {
                       t
                   };
        }

        static LifetimeManager GetLifetimeManager(DependencyLifecycle dependencyLifecycle)
        {
            switch (dependencyLifecycle)
            {
                case DependencyLifecycle.InstancePerCall:
                    return new TransientLifetimeManager();
                case DependencyLifecycle.SingleInstance:
                    return new ContainerControlledLifetimeManager();
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

                List<Tuple<string, object>> configuredProperty;
                if (configuredProperties.TryGetValue(type, out configuredProperty))
                {
                    var p = configuredProperty.FirstOrDefault(t => t.Item1 == property.Name);

                    if (p != null)
                    {
                        property.SetValue(target, p.Item2, null);
                    }
                }
            }
        }
    }
}

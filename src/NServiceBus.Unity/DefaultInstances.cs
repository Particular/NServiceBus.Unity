namespace NServiceBus.Unity
{
    using System;
    using System.Collections.Generic;

    class DefaultInstances
    {
        HashSet<Type> typesWithDefaultInstances = new HashSet<Type>();

        public bool Contains(Type type)
        {
            return typesWithDefaultInstances.Contains(type);
        }

        public void Add(Type type)
        {
            typesWithDefaultInstances.Add(type);
        }

        public void Clear()
        {
            typesWithDefaultInstances.Clear();
        }
    }
}
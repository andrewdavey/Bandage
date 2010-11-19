using System;
using System.Collections.Generic;

namespace Bandage
{
    public class DynamicPropertyProvider
    {
        readonly Dictionary<Tuple<Type, string>, DynamicProperty> properties 
            = new Dictionary<Tuple<Type, string>, DynamicProperty>(new KeyComparer());

        public bool TryGetProperty(Type forType, string propertyName, out DynamicProperty property)
        {
            var key = Tuple.Create(forType, propertyName);
            return properties.TryGetValue(key, out property);
        }

        public void Add(DynamicProperty property)
        {
            if (property == null) throw new ArgumentNullException("property");
            properties.Add(Tuple.Create(property.ForType, property.Name), property);
        }

        class KeyComparer : IEqualityComparer<Tuple<Type, string>>
        {
            public bool Equals(Tuple<Type, string> x, Tuple<Type, string> y)
            {
                if (x.Item2 == y.Item2)
                {
                    return x.Item1.IsAssignableFrom(y.Item1);
                }
                return false;
            }

            public int GetHashCode(Tuple<Type, string> obj)
            {
                // "Equal" objects must have the same hashcode, else
                // the Equals method is never called by the dictionary.
                // So we can't use the whole Tuple's hashcode value.
                // It would be different for subtypes.
                return obj.Item2.GetHashCode();
            }
        }

    }
}

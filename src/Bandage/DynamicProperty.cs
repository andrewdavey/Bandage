using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bandage
{
    public class DynamicProperty
    {
        public static DynamicProperty For<T>(string propertyName, Func<T, object> getter)
        {
            return new DynamicProperty
            {
                Name = propertyName,
                Getter = x => getter((T)x),
                ForType = typeof(T)
            };
        }

        public string Name { get; private set; }
        public Func<object, object> Getter { get; private set; }
        public Type ForType { get; private set; }
    }
}

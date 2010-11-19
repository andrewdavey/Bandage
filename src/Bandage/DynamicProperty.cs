using System;

namespace Bandage
{
    public class DynamicProperty
    {
        /// <remarks>Must use the DynamicProperty.For factory method.</remarks>
        private DynamicProperty() { }

        public static DynamicProperty For<T>(string propertyName, Func<T, object> getter)
        {
            return new DynamicProperty
            {
                ForType = typeof(T),
                Name = propertyName,
                getter = x => getter((T)x)
            };
        }

        Func<object, object> getter;

        public Type ForType { get; private set; }
        public string Name { get; private set; }

        public object GetValue(object obj)
        {
            return getter(obj);
        }
    }
}

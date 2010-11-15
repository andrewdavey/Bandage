using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace Bandage
{
    public class BandageViewModel : DynamicObject
    {
        readonly IDictionary<string, object> store = new ExpandoObject();
        readonly IDictionary<Tuple<Type, string>, DynamicProperty> properties = new Dictionary<Tuple<Type, string>, DynamicProperty>();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            store[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (store.TryGetValue(binder.Name, out result))
            {
                if (!(result is Wrapper))
                {
                    result = new Wrapper(result, properties);
                }
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public void Add(DynamicProperty dynamicProperty)
        {
            properties[Tuple.Create(dynamicProperty.ForType, dynamicProperty.Name)] = dynamicProperty;
        }
    }
}

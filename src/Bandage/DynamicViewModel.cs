using System.Collections.Generic;
using System.Dynamic;

namespace Bandage
{
    public class DynamicViewModel : DynamicObject
    {
        readonly IDictionary<string, object> store = new ExpandoObject();
        readonly DynamicPropertyProvider properties = new DynamicPropertyProvider();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            store[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (store.TryGetValue(binder.Name, out result))
            {
                if (!(result is IWrapper))
                {
                    result = Wrapper.Create(result, properties);
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
            properties.Add(dynamicProperty);
        }
    }
}

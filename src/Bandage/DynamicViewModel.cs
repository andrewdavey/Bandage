using System.Dynamic;
using System.Web.Mvc;

namespace Bandage
{
    public class DynamicViewModel : DynamicObject
    {
        public DynamicViewModel()
        {
            viewData = new ViewDataDictionary();
        }

        public DynamicViewModel(ViewDataDictionary viewData)
        {
            this.viewData = viewData;
        }

        readonly ViewDataDictionary viewData;
        readonly DynamicPropertyProvider dynamicProperties = new DynamicPropertyProvider();

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!(value is IWrapper)) 
                value = Wrapper.Create(value, dynamicProperties);

            viewData[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (viewData.TryGetValue(binder.Name, out result))
            {
                if (!(result is IWrapper))
                {
                    result = Wrapper.Create(result, dynamicProperties);
                }
                return true;
            }

            result = null;
            return false;
        }

        public void Add(DynamicProperty dynamicProperty)
        {
            dynamicProperties.Add(dynamicProperty);
        }

        public static implicit operator ViewDataDictionary(DynamicViewModel dvm)
        {
            return dvm.viewData;
        }
    }
}

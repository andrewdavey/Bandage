using System.Web.Mvc;
using Xunit;

namespace Bandage
{
    public class DynamicViewModelTests
    {
        [Fact]
        public void Can_set_property()
        {
            dynamic viewmodel = new DynamicViewModel();
            viewmodel.Test = 1;
            Assert.Equal(1, viewmodel.Test);
        }

        [Fact]
        public void Property_returns_wrapper_object()
        {
            dynamic viewmodel = new DynamicViewModel();
            viewmodel.Test = 1;
            Assert.True(viewmodel.Test is IWrapper);
        }

        [Fact]
        public void Can_implicitly_cast_to_ViewDataDictionary()
        {
            dynamic viewmodel = new DynamicViewModel();
            viewmodel.Test = 1;
            ViewDataDictionary viewData = viewmodel;
            Assert.True(viewData["Test"] is IWrapper);
            dynamic test = viewData["Test"];
            Assert.Equal(1, test);
        }

        [Fact]
        public void Can_create_with_existing_viewdata()
        {
            var viewData = new ViewDataDictionary();
            dynamic viewmodel = new DynamicViewModel(viewData);
            viewmodel.Test = 1;
            Assert.True(viewData["Test"] is IWrapper);
            dynamic test = viewData["Test"];
            Assert.Equal(1, test);
        }
    }
}

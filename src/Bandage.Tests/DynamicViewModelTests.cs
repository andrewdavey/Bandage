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
    }
}

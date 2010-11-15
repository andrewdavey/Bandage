using Xunit;

namespace Bandage
{
    public class BandageViewModelTests
    {
        [Fact]
        public void Can_set_property()
        {
            dynamic viewmodel = new BandageViewModel();
            viewmodel.Test = 1;
            Assert.Equal(1, (viewmodel.Test as Wrapper).Value);
        }
    }
}

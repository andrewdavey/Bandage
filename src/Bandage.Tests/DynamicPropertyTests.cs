using Xunit;

namespace Bandage
{
    public class DynamicProperty_For_SomeClass
    {
        public DynamicProperty_For_SomeClass()
        {
            property = DynamicProperty.For<SomeClass>("Test", c => "value");
        }

        DynamicProperty property;

        [Fact]
        public void PropertyName_is_Test()
        {
            Assert.Equal("Test", property.Name);
        }

        [Fact]
        public void Getter_Returns_value()
        {
            Assert.Equal("value", property.Getter(new SomeClass()));
        }

        [Fact]
        public void ForType_is_SomeClass()
        {
            Assert.Equal(typeof(SomeClass), property.ForType);
        }

        class SomeClass { }
    }
}

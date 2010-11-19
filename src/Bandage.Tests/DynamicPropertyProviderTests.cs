using Xunit;

namespace Bandage
{
    public class DynamicPropertyProviderTests
    {
        [Fact]
        public void Can_get_property_for_subtypes()
        {
            var properties = new DynamicPropertyProvider();
            properties.Add(DynamicProperty.For<Person>("Test", p => 0));

            DynamicProperty property;
            Assert.True(properties.TryGetProperty(typeof(Employee), "Test", out property));
        }
        class Person { }
        class Employee : Person { }
    }
}

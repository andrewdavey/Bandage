using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bandage
{
    public class WrapperTests
    {
        Dictionary<Tuple<Type, string>, DynamicProperty> properties = new Dictionary<Tuple<Type, string>, DynamicProperty>();

        [Fact]
        public void Can_get_property()
        {
            dynamic wrapper = new Wrapper(new { Test = 1 }, properties);
            Assert.Equal(1, (wrapper.Test as Wrapper).Value);
        }

        [Fact]
        public void Can_get_indexed_property()
        {
            dynamic wrapper = new Wrapper(new { Test = new[] { 1, 2, 3 } }, properties);
            Assert.Equal(2, (wrapper.Test[1] as Wrapper).Value);
        }

        [Fact]
        public void Can_invoke_method_on_wrapped_property()
        {
            dynamic wrapped = new Wrapper(new { Test = "123   " }, properties);
            Assert.Equal("123", (wrapped.Test.Trim() as Wrapper).Value);
        }

        [Fact]
        public void Can_use_equality_operator()
        {
            dynamic wrapper = new Wrapper(new { Test = "123" }, properties);
            Assert.True(wrapper.Test == "123");
        }

        [Fact]
        public void Can_use_equality_operator_on_integer()
        {
            dynamic wrapper = new Wrapper(new { Test = 123 }, properties);
            Assert.True(wrapper.Test == 123);
        }

        [Fact]
        public void Can_use_lessthan_operator_on_integer()
        {
            dynamic wrapper = new Wrapper(new { Test = 1 }, properties);
            Assert.True(wrapper.Test < 2);
        }
    }
}

using System;
using System.Collections.Generic;
using Xunit;

namespace Bandage
{
    public class WrapperTests
    {
        Dictionary<Tuple<Type, string>, DynamicProperty> properties = new Dictionary<Tuple<Type, string>, DynamicProperty>();

        [Fact]
        public void Can_get_property()
        {
            dynamic wrapper = Wrapper.Create(new { Test = 1 }, properties);
            Assert.Equal(1, wrapper.Test);
        }

        [Fact]
        public void Can_get_indexed_property()
        {
            dynamic wrapper = Wrapper.Create(new { Test = new[] { 1, 2, 3 } }, properties);
            Assert.Equal(2, wrapper.Test[1]);
        }

        [Fact]
        public void Can_invoke_method_on_wrapped_property()
        {
            dynamic wrapped = Wrapper.Create(new { Test = "123   " }, properties);
            Assert.Equal("123", wrapped.Test.Trim());
        }

        [Fact]
        public void Can_use_equality_operator()
        {
            dynamic wrapper = Wrapper.Create(new { Test = "123" }, properties);
            Assert.True(wrapper.Test == "123");
        }

        [Fact]
        public void Can_use_equality_operator_on_integer()
        {
            dynamic wrapper = Wrapper.Create(new { Test = 123 }, properties);
            Assert.True(wrapper.Test == 123);
        }

        [Fact]
        public void Can_use_lessthan_operator_on_integer()
        {
            dynamic wrapper = Wrapper.Create(new { Test = 1 }, properties);
            Assert.True(wrapper.Test < 2);
        }

        [Fact]
        public void Can_implicitly_convert_for_method_call()
        {
            dynamic wrapper = Wrapper.Create("hello", properties);
            SomeMethod(wrapper.Length);
        }
        static void SomeMethod(int x) { }
    }

}

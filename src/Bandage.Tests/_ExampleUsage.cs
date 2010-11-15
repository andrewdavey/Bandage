using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Bandage
{
    public class _ExampleUsage
    {
        [Fact]
        public void Can_add_dynamic_property_to_viewmodel_child()
        {
            dynamic viewmodel = new DynamicViewModel();
            viewmodel.Customer = new Customer { Id = 1 };
            viewmodel.Add(DynamicProperty.For<Customer>("Url", c => "/customer/" + c.Id));

            Assert.Equal("/customer/1", (viewmodel.Customer.Url as Wrapper).Value);
        }

        class Customer
        {
            public int Id { get; set; }
        }
    }
}

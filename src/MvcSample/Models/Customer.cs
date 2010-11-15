using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcSample.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Customer[] LoadAll()
        {
            return new[] {
                new Customer { Id = 1, Name = "Customer 1" },
                new Customer { Id = 2, Name = "Customer 2" },
                new Customer { Id = 3, Name = "Customer 3" }
            };
        }
    }
}
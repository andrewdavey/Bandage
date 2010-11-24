namespace MvcSample.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Sample data
        public static Product[] LoadAll()
        {
            return new[] {
                new Product { Id = 1, Name = "MacBook Pro" },
                new Product { Id = 2, Name = "Plasma TV" },
                new Product { Id = 3, Name = "Office Chair" }
            };
        }
    }
}
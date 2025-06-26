namespace Northwind_DBFirst.Models
{
    public class VMProducts
    {
        public List<Product> Products { get; set; } = new();
        public int? selectedCategory { get; set; }

        public List<Category> Categories { get; set; } = new();

    }
}

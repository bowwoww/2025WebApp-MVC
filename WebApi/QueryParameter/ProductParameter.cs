namespace WebApi.QueryParameter
{
    public class ProductParameter
    {
        public string? cateId { get; set; }
        public string? cateName { get; set; }
        public string? productName { get; set; }
        public string? description { get; set; }
        public decimal? maxPrice { get; set; } = null;
        public decimal? minPrice { get; set; } = 0;
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Models;

namespace WebApi.Partial
{
    public class ProductMetaData
    {
        [JsonIgnore]
        [ForeignKey("CateID")]
        [InverseProperty("Product")]
        public virtual Category Cate { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Product")]
        public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
    }

    public class CategoryMetaData
    {
        [JsonIgnore]
        [InverseProperty("Cate")]
        public virtual ICollection<Product> Product { get; set; } = new List<Product>();
    }

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product { }


    [MetadataType(typeof(CategoryMetaData))]
    public partial class Category { }

}

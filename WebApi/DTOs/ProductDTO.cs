

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class ProductDTO
    {
        [Key]
        [StringLength(5)]
        public string ProductID { get; set; } = null!;

        [StringLength(40)]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [StringLength(12)]
        public string Picture { get; set; } = null!;

        [StringLength(2)]
        public string CateID { get; set; } = null!;

        public string CateName { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();

        public int OrderCount
        {
            get {
                return OrderDetail?.Count ?? 0;
            }
        }
    }
}

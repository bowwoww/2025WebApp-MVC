using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static WebApi.Validation.ProductValidator;

namespace WebApi.DTOs
{
    public class ProductPostDTO
    {
        [Key]
        [StringLength(5)]
        [Required]
        //第一碼A-Z 第二碼1-9, 3-5碼為流水號
        [RegularExpression(@"^[A-Z][1-9][0-9]{3}$", ErrorMessage = "Bad ID Format")]
        public string ProductID { get; set; } = null!;

        [Required]
        [StringLength(40)]
        [ProductNameValidator]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "money")]
        [ProductPostDTOValidator]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Product Picture")]
        [ProductPostDTOValidator]
        public IFormFile Picture { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[A-Z][1-9]$", ErrorMessage = "Category ID format error")]
        [StringLength(2)]
        public string CateID { get; set; } = null!;
    }
}

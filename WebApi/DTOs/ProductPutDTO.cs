using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static WebApi.Validation.ProductValidator;

namespace WebApi.DTOs
{
    public class ProductPutDTO
    {

        [Column(TypeName = "money")]
        [ProductPostDTOValidator]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Product Picture")]
        [ProductPostDTOValidator]
        public IFormFile? Picture { get; set; }

    }
}
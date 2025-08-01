using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DTOs
{
    public class ProductPostDTO
    {
        [Key]
        [StringLength(5)]
        [Required]
        //第一碼A-Z 第二碼1-9, 3-5碼為流水號
        [RegularExpression(@"^[A-Z][1-9][0-9]{3}$",ErrorMessage ="Bad ID Format")]
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

    public class ProductPostDTOValidator : ValidationAttribute
    {
        
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is ProductPostDTO product)
            {
                if (product.Price < 0)
                {
                    return new ValidationResult("Price cannot be negative.");
                }
                if (product.Picture.Length > 5 * 1024 * 1024) // 5 MB
                {
                    return new ValidationResult("Picture size cannot exceed 5 MB.");
                }
            }
            return ValidationResult.Success;
        }
    }

    public class ProductNameValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? productName = value as string;
            if (string.IsNullOrWhiteSpace(productName))
            {
                return new ValidationResult("Product name cannot be empty.");
            }
            if (productName.Length < 2 || productName.Length > 40)
            {
                return new ValidationResult("Product name must be between 2 and 40 characters.");
            }
            return ValidationResult.Success;
        }
    }
}

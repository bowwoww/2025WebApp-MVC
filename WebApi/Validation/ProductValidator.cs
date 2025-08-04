using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebApi.DTOs;

namespace WebApi.Validation
{
    public class ProductValidator
    {
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
}

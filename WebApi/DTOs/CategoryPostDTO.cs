using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Models;

namespace WebApi.DTOs
{
    public class CategoryPostDTO
    {
        [Key]
        [StringLength(2,ErrorMessage ="分類編號為2碼")]
        [RegularExpression("^[A-Z][1-9]$", ErrorMessage = "請輸入首碼A-Z,尾碼1-9")]
        public string CateID { get; set; } = null!;

        [StringLength(20)]
        [CategoryPostDTOValidator]
        public string CateName { get; set; } = null!;
    }

    public class CategoryPostDTOValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //直接取得service的dbcontext
            var context = validationContext.GetService<GoodStoreContext2>();
            if (context == null)
            {
                throw new InvalidOperationException("Cannot resolve GoodStoreContext2 from ValidationContext.");
            }

            var cateName = value as string;
            if (!string.IsNullOrEmpty(cateName))
            {
                if (context.Category.Any(c => c.CateName == cateName))
                {
                    return new ValidationResult("Category Name already exists.");
                }
            }
            return ValidationResult.Success;
        }
    }
}

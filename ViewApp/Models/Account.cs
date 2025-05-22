using System.ComponentModel.DataAnnotations;

namespace ViewApp.Models
{
    public class Account
    {
        [Display(Name = "Account ID")]
        [Required(ErrorMessage = "請輸入帳號")]
        public required string Id { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "請輸入密碼")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "密碼長度至少4個字元")]
        [MaxLength(10, ErrorMessage = "密碼長度最多10個字元")]
        public required string Password { get; set; }
    }
}

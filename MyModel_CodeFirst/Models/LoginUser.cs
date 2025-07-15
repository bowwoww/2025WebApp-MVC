using System.ComponentModel.DataAnnotations;

namespace MyModel_CodeFirst.Models
{
    public class LoginUser
    {
        [Key]
        [StringLength(10,MinimumLength = 5, ErrorMessage = "帳號長度必須介於5到10個字元")]
        [Required(ErrorMessage = "必填")]
        //第一位元必須為英文字母
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "帳號必須以英文字母開頭，且只能包含英文字母和數字")]
        public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼長度必須介於8到20個字元")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public string ReturnUrl { get; set; } = null!;

        public int Role { get; set; } = 0; // 0:一般使用者, 1:管理員
    }
}

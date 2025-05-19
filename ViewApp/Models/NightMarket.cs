using System.ComponentModel.DataAnnotations;

namespace ViewApp.Models
{
    public class NightMarket
    {
        [Display(Name="編號")]
        [Required(ErrorMessage = "請輸入編號")]
        [RegularExpression("A[0-9]{2}", ErrorMessage = "編號格式錯誤，請輸入大寫A加兩位數字")]

        public string Id { get; set; } = null!;

        [Display(Name = "名稱")]
        [Required(ErrorMessage = "請輸入名稱")]
        public string Name { get; set; } = null!;


        [Display(Name = "地址")]
        [Required(ErrorMessage = "請輸入地址")]
        [StringLength(30, ErrorMessage = "地址長度不能超過30個字")]
        public string Address { get; set; } = null!;
    }
}

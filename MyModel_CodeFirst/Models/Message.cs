using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MyModel_CodeFirst.Models
{
    // Metadata類別用於定義模型的元數據
    //[ModelMetadataType(typeof(MessageMetadata))]
    public partial class Message
    {
        [DisplayName("編號")]
        [StringLength(36, MinimumLength = 36, ErrorMessage = "編號長度必須為36個字元")]
        [Key]
        public string Id { get; set; } = null!;
        [DisplayName("發文者")]
        [Required(ErrorMessage = "發文者為必填欄位")]
        [StringLength(50, ErrorMessage = "發文者名稱不能超過50個字元")]
        public string Sender { get; set; } = string.Empty;
        [DisplayName("主題")]
        [StringLength(100, ErrorMessage = "主題不能超過100個字元")]
        public string Subject { get; set; } = string.Empty;
        [DisplayName("發文日期")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [HiddenInput]
        public DateTime SentDate { get; set; } = DateTime.Now;
        [DisplayName("上傳照片")]
        [StringLength(50)]
        // 上傳照片檔名為編號 + jpg
        public string UploadPhoto { get; set; } = string.Empty;
        [DisplayName("內容")]
        [Required(ErrorMessage = "內容為必填欄位")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; } = string.Empty;
        public virtual List<Response>? Responses { get; set; }
    }

}

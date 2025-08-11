using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MyModel_CodeFirst.Models
{
    //[ModelMetadataType(typeof(ResponseMetadata))]
    public partial class Response
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseId { get; set; }
        [DisplayName("回覆者")]
        [Required(ErrorMessage = "回覆者為必填欄位")]
        [StringLength(50, ErrorMessage = "回覆者名稱不能超過50個字元")]
        public string Sender { get; set; } = null!;
        [DisplayName("回覆日期")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [HiddenInput]
        public DateTime SentDate { get; set; } = DateTime.Now;

        [DisplayName("回覆內容")]
        [Required(ErrorMessage = "回覆內容為必填欄位")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; } = null!;

        [ForeignKey("Message")]
        [Required]
        [HiddenInput]
        public string Id { get; set; } = null!;
        public virtual Message? Message { get; set; }
    }
}

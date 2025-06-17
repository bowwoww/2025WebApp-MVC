using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyModel.Models;

public partial class tStudent
{
    [Display(Name = "Student ID")]
    [Required(ErrorMessage = "請輸入學號")]
    [StringLength(6, ErrorMessage = "學號長度必須為6個字元")]
    [RegularExpression(@"^[1-9]{1}[0-9]{5}$", ErrorMessage = "學號格式錯誤，應為六位數字，前三位為民國年")]
    public string fStuId { get; set; } = null!;
    [Display(Name = "姓名")]
    [Required(ErrorMessage = "請輸入姓名")]
    [StringLength(10, ErrorMessage = "姓名長度不能超過30個字元")]
    public string fName { get; set; } = null!;
    [Display(Name = "E-mail")]
    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
    [StringLength(40, ErrorMessage = "電子郵件長度不能超過40個字元")]
    public string? fEmail { get; set; }
    [Display(Name = "分數")]
    [Range(0, 100, ErrorMessage = "分數必須在0到100之間")]
    public int? fScore { get; set; }
    [Display(Name = "科系代碼")]
    [ForeignKey("Department")]
    public string departID { get; set; } = null!;

    //virtual 僅描述資料庫關聯關係 
    public virtual Department? Department { get; set; }
}

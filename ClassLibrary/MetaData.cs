using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    public class MetaData
    {
    }

    public class EmployeeRoleMetaData
    {
        [RegularExpression(@"[A-Z]",ErrorMessage = "僅可輸入A-Z")]

        public string RoleCode { get; set; } = null!;

        [Display(Name = "角色名稱")]
        [StringLength(15,MinimumLength =2, ErrorMessage = "角色名稱長度不能超過15個字元")]
        public string RoleName { get; set; } = null!;

    }

    [ModelMetadataType(typeof(EmployeeRoleMetaData))]
    public class EmployeeRoleMateData { }

}

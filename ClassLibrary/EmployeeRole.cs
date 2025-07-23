using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary;

public partial class EmployeeRole
{
    [Key]
    [StringLength(1)]
    public string RoleCode { get; set; } = null!;

    [StringLength(15)]
    public string RoleName { get; set; } = null!;

    [InverseProperty("RoleCodeNavigation")]
    public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();
}

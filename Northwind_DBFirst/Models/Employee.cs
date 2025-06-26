using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind_DBFirst.Models;

public partial class Employee
{
    [Key]
    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [StringLength(20)]
    public string LastName { get; set; } = null!;

    [StringLength(10)]
    public string FirstName { get; set; } = null!;

    [StringLength(30)]
    public string Title { get; set; } = null!;

    [StringLength(25)]
    public string TitleOfCourtesy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime BirthDate { get; set; }

    [Column(TypeName = "datetime")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime HireDate { get; set; }

    [StringLength(60)]
    public string Address { get; set; } = null!;

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(24)]
    public string HomePhone { get; set; } = null!;

    [StringLength(4)]
    public string? Extension { get; set; }

    [Column(TypeName = "image")]
    public byte[] Photo { get; set; } = null!;

    public string? Notes { get; set; }

    public int? ReportsTo { get; set; }

    [InverseProperty("ReportsToNavigation")]
    public virtual ICollection<Employee> InverseReportsToNavigation { get; set; } = new List<Employee>();

    [InverseProperty("Employee")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("ReportsTo")]
    [InverseProperty("InverseReportsToNavigation")]
    public virtual Employee? ReportsToNavigation { get; set; }
}

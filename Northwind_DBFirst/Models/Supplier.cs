using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind_DBFirst.Models;

public partial class Supplier
{
    [Key]
    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(30)]
    public string ContactName { get; set; } = null!;

    [StringLength(30)]
    public string ContactTitle { get; set; } = null!;

    [StringLength(60)]
    public string Address { get; set; } = null!;

    [StringLength(10)]
    public string? PostalCode { get; set; }

    [StringLength(24)]
    public string Phone { get; set; } = null!;

    [StringLength(24)]
    public string? Fax { get; set; }

    [InverseProperty("Supplier")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}

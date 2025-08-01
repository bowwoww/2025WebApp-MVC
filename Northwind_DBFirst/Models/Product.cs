﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind_DBFirst.Models;

public partial class Product
{
    [Key]
    [Column("ProductID")]
    [Display(Name = "產品 ID")]
    public int ProductId { get; set; }

    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    [Column("SupplierID")]
    public int SupplierId { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [StringLength(20)]
    public string QuantityPerUnit { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal UnitPrice { get; set; }

    public short UnitsInStock { get; set; }

    public short UnitsOnOrder { get; set; }

    public short ReorderLevel { get; set; }

    public bool Discontinued { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("SupplierId")]
    [InverseProperty("Products")]
    public virtual Supplier Supplier { get; set; } = null!;
}

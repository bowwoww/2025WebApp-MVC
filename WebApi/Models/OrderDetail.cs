using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

[PrimaryKey("OrderID", "ProductID")]
public partial class OrderDetail
{
    [Key]
    [StringLength(12)]
    public string OrderID { get; set; } = null!;

    [Key]
    [StringLength(5)]
    public string ProductID { get; set; } = null!;

    public int Qty { get; set; }

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [ForeignKey("OrderID")]
    [InverseProperty("OrderDetail")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductID")]
    [InverseProperty("OrderDetail")]
    public virtual Product Product { get; set; } = null!;
}

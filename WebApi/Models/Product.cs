using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public partial class Product
{
    [Key]
    [StringLength(5)]
    public string ProductID { get; set; } = null!;

    [StringLength(40)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [StringLength(12)]
    public string Picture { get; set; } = null!;

    [StringLength(2)]
    public string CateID { get; set; } = null!;

    [ForeignKey("CateID")]
    [InverseProperty("Product")]
    public virtual Category Cate { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}

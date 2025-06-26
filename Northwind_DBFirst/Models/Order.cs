using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Northwind_DBFirst.Models;

public partial class Order
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("CustomerID")]
    [StringLength(5)]
    public string CustomerId { get; set; } = null!;

    [Column("EmployeeID")]
    public int EmployeeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RequiredDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ShippedDate { get; set; }

    public int ShipVia { get; set; }

    [Column(TypeName = "money")]
    public decimal? Freight { get; set; }

    [StringLength(40)]
    public string ShipName { get; set; } = null!;

    [StringLength(60)]
    public string ShipAddress { get; set; } = null!;

    [StringLength(10)]
    public string? ShipPostalCode { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual Customer Customer { get; set; } = null!;

    [ForeignKey("EmployeeId")]
    [InverseProperty("Orders")]
    public virtual Employee Employee { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [ForeignKey("ShipVia")]
    [InverseProperty("Orders")]
    public virtual Shipper ShipViaNavigation { get; set; } = null!;
}

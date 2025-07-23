using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public partial class Order
{
    [Key]
    [StringLength(12)]
    public string OrderID { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [StringLength(6)]
    public string MemberID { get; set; } = null!;

    [StringLength(27)]
    public string ContactName { get; set; } = null!;

    [StringLength(60)]
    public string ContactAddress { get; set; } = null!;

    [ForeignKey("MemberID")]
    [InverseProperty("Order")]
    public virtual Member Member { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetail { get; set; } = new List<OrderDetail>();
}

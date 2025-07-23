using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

[Index("Account", Name = "UQ__Member__B0C3AC4646969FD1", IsUnique = true)]
public partial class Member
{
    [Key]
    [StringLength(6)]
    public string MemberID { get; set; } = null!;

    [StringLength(27)]
    public string Name { get; set; } = null!;

    public bool Gender { get; set; }

    public int Point { get; set; }

    [StringLength(12)]
    public string Account { get; set; } = null!;

    [StringLength(64)]
    public string Password { get; set; } = null!;

    [InverseProperty("Member")]
    public virtual ICollection<Order> Order { get; set; } = new List<Order>();
}

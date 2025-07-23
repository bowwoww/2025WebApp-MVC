using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;

namespace WebApi.Models;

public partial class GoodStoreContext : DbContext
{
    public GoodStoreContext(DbContextOptions<GoodStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Category { get; set; }

    public virtual DbSet<Member> Member { get; set; }

    public virtual DbSet<Order> Order { get; set; }

    public virtual DbSet<OrderDetail> OrderDetail { get; set; }

    public virtual DbSet<Product> Product { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CateID).HasName("PK__Category__27638D742755A720");

            entity.Property(e => e.CateID).IsFixedLength();
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberID).HasName("PK__Member__0CF04B38A211F86A");

            entity.Property(e => e.MemberID).IsFixedLength();
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderID).HasName("PK__Order__C3905BAF619CF263");

            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.MemberID).IsFixedLength();
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Member).WithMany(p => p.Order)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__MemberID__4316F928");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => new { e.OrderID, e.ProductID }).HasName("PK__OrderDet__08D097C16ABCE6CB");

            entity.Property(e => e.OrderID).IsFixedLength();
            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.Qty).HasDefaultValue(1);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__440B1D61");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetail)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Produ__44FF419A");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductID).HasName("PK__Product__B40CC6ED6326729A");

            entity.Property(e => e.ProductID).IsFixedLength();
            entity.Property(e => e.CateID).IsFixedLength();

            entity.HasOne(d => d.Cate).WithMany(p => p.Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product__CateID__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

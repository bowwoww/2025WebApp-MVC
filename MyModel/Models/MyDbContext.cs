using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyModel.Models;

public partial class MyDbContext : DbContext
{
    //public MyDbContext(DbContextOptions<MyDbContext> options)
    //    : base(options)
    //{
    //}

    public MyDbContext()
    {
    }

    public virtual DbSet<tStudent> tStudent { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<tStudent>(entity =>
        {
            entity.HasKey(e => e.fStuId).HasName("PK__tStudent__08E5BA9528561B56");

            entity.Property(e => e.fStuId)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.fEmail).HasMaxLength(40);
            entity.Property(e => e.fName).HasMaxLength(30);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Data Source=C501A104;Database=dbStudents;User ID=user;Password=12345678;TrustServerCertificate=True");
        }
    }
}

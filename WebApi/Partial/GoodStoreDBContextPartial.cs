using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;
using WebApi.Models;

namespace WebApi.Models;

public class GoodStoreContext2 : GoodStoreContext
{
    public GoodStoreContext2(DbContextOptions<GoodStoreContext> options)
        : base(options)
    {
    }
    public virtual DbSet<ProductDTO> ProductDTO { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductDTO>(entity =>
            entity.HasNoKey());
        base.OnModelCreating(modelBuilder);
    }

public DbSet<WebApi.Models.Animal> Animal { get; set; } = default!;
}
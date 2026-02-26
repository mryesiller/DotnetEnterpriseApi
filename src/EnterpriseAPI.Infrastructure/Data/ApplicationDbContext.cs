// src/EnterpriseAPI.Infrastructure/Data/ApplicationDbContext.cs
using EnterpriseAPI.Domain.Entities;
using EnterpriseAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseAPI.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity konfigürasyonu için bu çağrı şart!

        // Global query filter - soft delete
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        // Product konfigürasyonu
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Sku).HasMaxLength(20);
            entity.HasIndex(e => e.Sku).IsUnique();
        });

        // Identity tablo isimlerini özelleştirme (opsiyonel)
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("Users");
        });
        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Roles");
        });
        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });
        // ... diğer Identity tabloları
    }
}
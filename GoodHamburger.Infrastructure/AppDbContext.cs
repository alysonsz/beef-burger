using GoodHamburger.Domain;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Order> Orders { get; set; }
    public DbSet<MenuItemEntity> MenuItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);
            entity.Property(e => e.Discount).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.Property(e => e.Status).HasConversion<int>();

            entity.OwnsOne(e => e.Sandwich, sandwich =>
            {
                sandwich.WithOwner().HasForeignKey("OrderId");
                sandwich.Property(s => s.Id).ValueGeneratedNever();
                sandwich.Property(s => s.Name);
                sandwich.Property(s => s.Price).HasPrecision(18, 2);
                sandwich.Property(s => s.Category);
            });

            entity.OwnsOne(e => e.Fries, fries =>
            {
                fries.WithOwner().HasForeignKey("OrderId");
                fries.Property(f => f.Id).ValueGeneratedNever();
                fries.Property(f => f.Name);
                fries.Property(f => f.Price).HasPrecision(18, 2);
                fries.Property(f => f.Category);
            });

            entity.OwnsOne(e => e.Drink, drink =>
            {
                drink.WithOwner().HasForeignKey("OrderId");
                drink.Property(d => d.Id).ValueGeneratedNever();
                drink.Property(d => d.Name);
                drink.Property(d => d.Price).HasPrecision(18, 2);
                drink.Property(d => d.Category);
            });
        });

        modelBuilder.Entity<MenuItemEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Category).HasConversion<int>();
        });
    }
}

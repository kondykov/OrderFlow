using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderFlow.Document.Models;
using OrderFlow.Identity.Models;

namespace OrderFlow.Data;

public sealed class DataContext : IdentityDbContext<User, Role, string>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        var converter = new ValueConverter<OrderStatus, string>(
            v => v.ToString(), // Преобразование enum в строку
            v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v)); // Преобразование строки в enum
        
        modelBuilder.Entity<Order>()
            .Property(o => o.Status)
            .HasConversion(converter);
        
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(e => new { e.LoginProvider, e.ProviderKey });

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Article)
            .IsUnique();
        
        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);

    }
}
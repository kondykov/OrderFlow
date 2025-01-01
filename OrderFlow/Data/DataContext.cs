using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Data.Entities;
using OrderFlow.Models.Identity;

namespace OrderFlow.Data;

public sealed class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<TokenEntity> Tokens { get; set; }
}
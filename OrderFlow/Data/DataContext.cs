using Microsoft.EntityFrameworkCore;
using OrderFlow.Models.Identity;

namespace OrderFlow.Data;

public sealed class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.Migrate();
    }

    public DbSet<ApplicationUser> Users { get; set; }
}
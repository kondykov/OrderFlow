using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Identity.Models;

namespace OrderFlow.Data;

public sealed class DataContext : IdentityDbContext<User, Role, string>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.Migrate();
    }
}
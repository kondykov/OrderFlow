using Microsoft.AspNetCore.Identity;
using OrderFlow.Data.Entities.Identity;
using OrderFlow.Infrastructure.Identity;

namespace OrderFlow.Models.Identity;

public class ApplicationUser : IdentityUser<long>
{
    public string? RefreshToken { get; set; } = "";
    public DateTime RefreshTokenExpiryTime { get; set; }
    
    public int? RoleId { get; set; }
    public Role? Role { get; set; }
}
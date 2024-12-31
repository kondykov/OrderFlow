using Microsoft.AspNetCore.Identity;

namespace OrderFlow.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; } = "";
    public DateTime RefreshTokenExpiryTime { get; set; }

    public int? RoleId { get; set; }
}
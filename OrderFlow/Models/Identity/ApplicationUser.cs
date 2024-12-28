using Microsoft.AspNetCore.Identity;

namespace OrderFlow.Models.Identity;

public class ApplicationUser : IdentityUser<long>
{
    public string? RefreshToken { get; set; } = "";
    public DateTime RefreshTokenExpiryTime { get; set; }
}
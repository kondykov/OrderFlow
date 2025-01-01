using Microsoft.AspNetCore.Identity;

namespace OrderFlow.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string? AccessToken { get; set; } = string.Empty;
}
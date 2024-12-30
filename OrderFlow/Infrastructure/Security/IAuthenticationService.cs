using System.IdentityModel.Tokens.Jwt;
using OrderFlow.Models.Identity;

namespace OrderFlow.Infrastructure.Security;

public interface IAuthenticationService
{
    public JwtSecurityToken? GenerateAccessToken(ApplicationUser user);
    public string GenerateRefreshToken(ApplicationUser user);
    public JwtSecurityToken? Revoke(string refreshToken);
    public ApplicationUser GetUserByAccessToken(string accessToken);
}
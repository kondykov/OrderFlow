using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OrderFlow.Infrastructure.Identity;
using OrderFlow.Infrastructure.Security;
using OrderFlow.Models.Identity;

namespace OrderFlow.Services.Identity;

public class AuthenticationService(IAccountRepository accountRepository) : IAuthenticationService
{
    public JwtSecurityToken? GenerateAccessToken(ApplicationUser user)
    {
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, user.Id.ToString()) };
        return new JwtSecurityToken(
            AuthenticationOptions.Issuer,
            AuthenticationOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
            signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
    }

    public string GenerateRefreshToken(ApplicationUser user)
    {
        var refreshToken = Guid.NewGuid().ToString();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = new DateTime().AddDays(7);

        return refreshToken;
    }

    public JwtSecurityToken? Revoke(string refreshToken)
    {
        var user = accountRepository.FindByRefreshTokenAsync(refreshToken);
        return user == null ? null : GenerateAccessToken(user.Result.Data);
    }

    public ApplicationUser GetUserByAccessToken(string accessToken)
    {
        throw new NotImplementedException();
    }
}
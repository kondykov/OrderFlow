using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OrderFlow.Identity.Models;

namespace OrderFlow.Identity.Services;

public class TokenService(IConfiguration configuration, UserManager<User> manager)
{
    public string GenerateJwtToken(User user)
    {
        if (configuration["Jwt:SecretKey"] == null) throw new Exception("Secret Key is missing.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
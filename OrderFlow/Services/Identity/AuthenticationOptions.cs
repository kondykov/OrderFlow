using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OrderFlow.Services.Identity;

public class AuthenticationOptions(IConfiguration configuration)
{
    public const string Issuer = "MyAuthServer";
    public const string Audience = "MyAuthClient";

    private const string Key =
        "92ad9562517b838911e315b328d737a6e0b46e577cfc7791db302be870917484be517d905e0fcddefd99900a5d0f5b3a";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
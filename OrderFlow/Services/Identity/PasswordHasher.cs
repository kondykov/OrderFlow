using OrderFlow.Data;
using OrderFlow.Infrastructure.Security;

namespace OrderFlow.Services.Identity;

public class PasswordHasher(DataContext dataContext) : IPasswordHasher
{
    private const int Salt = 12;

    public string GenerateHash(string password) => BCrypt.Net.BCrypt.HashPassword(password, Salt);

    public bool VerifyHash(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}
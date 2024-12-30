namespace OrderFlow.Infrastructure.Security;

public interface IPasswordHasher
{
    public string GenerateHash(string password);
    public bool VerifyHash(string password, string passwordHash);
}
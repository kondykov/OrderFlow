using OrderFlow.Data;
using OrderFlow.Infrastructure.Identity;
using OrderFlow.Models.Identity;

namespace OrderFlow.Infrastructure.Persistence.Identity;

public class AccountRepository(DataContext context) : IAccountRepository
{
    public void Save(ApplicationUser user)
    {
        context.Users.Add(user);
    }

    public ApplicationUser FindByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public ApplicationUser FindByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public List<ApplicationUser> GetAll()
    {
        return context.Users.ToList();
    }
}
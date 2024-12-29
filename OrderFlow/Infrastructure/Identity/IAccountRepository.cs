using OrderFlow.Models.Identity;

namespace OrderFlow.Infrastructure.Identity;

public interface IAccountRepository
{
    public void Save(ApplicationUser user);
    public ApplicationUser FindByEmail(string email);
    public ApplicationUser FindByUsername(string username);
    
    public List<ApplicationUser> GetAll();
}
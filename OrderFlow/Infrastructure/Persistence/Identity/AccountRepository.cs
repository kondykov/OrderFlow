using Microsoft.EntityFrameworkCore;
using OrderFlow.Data;
using OrderFlow.Infrastructure.Identity;
using OrderFlow.Models;
using OrderFlow.Models.Identity;

namespace OrderFlow.Infrastructure.Persistence.Identity;

public class AccountRepository(DataContext context) : IAccountRepository
{
    public async Task<OperationResult<ApplicationUser>> CreateAsync(ApplicationUser? user)
    {
        if (await context.Users.AnyAsync(u => u != null && u.UserName == user!.UserName))
            return new OperationResult<ApplicationUser>
            {
                ErrorMessage = "User already exists!"
            };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new OperationResult<ApplicationUser> { Data = user };
    }

    public async Task<OperationResult<ApplicationUser>> FindByEmailAsync(string email)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u!.Email == email);
        return user == null
            ? new OperationResult<ApplicationUser> { ErrorMessage = "User not found!" }
            : new OperationResult<ApplicationUser> { Data = user };
    }

    public async Task<OperationResult<ApplicationUser>> FindByUsernameAsync(string username)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u!.UserName == username);
        return user == null
            ? new OperationResult<ApplicationUser> { ErrorMessage = "User not found!" }
            : new OperationResult<ApplicationUser> { Data = user };
    }

    public Task<OperationResult<ApplicationUser>> FindByRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public async Task<OperationResult<ApplicationUser>> SaveAsync(ApplicationUser user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return new OperationResult<ApplicationUser> { Data = user };
    }

    public Task<OperationResult<List<ApplicationUser?>>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
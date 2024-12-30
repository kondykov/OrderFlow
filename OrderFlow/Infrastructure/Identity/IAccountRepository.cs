using OrderFlow.Data.Entities.Identity;
using OrderFlow.Models;
using OrderFlow.Models.Identity;

namespace OrderFlow.Infrastructure.Identity;

public interface IAccountRepository
{
    public Task<OperationResult<ApplicationUser>> CreateAsync(ApplicationUser? user);
    public Task<OperationResult<ApplicationUser>> FindByEmailAsync(string email);
    public Task<OperationResult<ApplicationUser>> FindByUsernameAsync(string username);
    public Task<OperationResult<ApplicationUser>> FindByRefreshTokenAsync(string refreshToken);
    public Task<OperationResult<ApplicationUser>> SaveAsync(ApplicationUser user);
    public Task<OperationResult<List<ApplicationUser?>>> GetAllAsync();
}
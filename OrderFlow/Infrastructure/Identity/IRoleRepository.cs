using OrderFlow.Data.Entities.Identity;
using OrderFlow.Models;

namespace OrderFlow.Infrastructure.Identity;

public interface IRoleRepository
{
    public Task<OperationResult<Role>> AddRoleAsync(Role role);
    public Task<OperationResult<Role>> FindRoleByCodeAsync(string code);
    public Task<OperationResult<Role>> FindRoleByIdAsync(int id);
    public Task<OperationResult<List<Role>>> GetRolesAsync();
}
using OrderFlow.Data.Entities.Identity;
using OrderFlow.Infrastructure.Identity;
using OrderFlow.Models;

namespace OrderFlow.Infrastructure.Persistence.Identity;

public class RoleRepository : IRoleRepository
{
    public Task<OperationResult<Role>> AddRoleAsync(Role role)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<Role>> FindRoleByCodeAsync(string code)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<Role>> FindRoleByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<OperationResult<List<Role>>> GetRolesAsync()
    {
        throw new NotImplementedException();
    }
}
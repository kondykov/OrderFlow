using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OrderFlow.Identity.Models;

namespace OrderFlow.Identity.Services;

public class UserService(IHttpContextAccessor httpContextAccessor, UserManager<User> manager)
{
    public async Task<User> GetCurrentUser()
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
            throw new UnauthorizedAccessException();
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return await manager.FindByIdAsync(userId);
    }

    public async Task<bool> HasRoleAsync(Role role)
    {
        var user = await GetCurrentUser();
        var roles = await manager.GetRolesAsync(user);
        return roles.Contains(role.ToString());
    }

    public async Task<bool> HasAnyRoleAsync(List<Role> roles)
    {
        foreach (var role in roles)
        {
            var result = await HasRoleAsync(role);
            if (result) return true;
        }
        return false;
    }
    
    public async Task<bool> HasAllRolesAsync(List<Role> roles)
    {
        foreach (var role in roles)
        {
            var result = await HasRoleAsync(role);
            if (!result) return false;
        }
        return true;
    }
}
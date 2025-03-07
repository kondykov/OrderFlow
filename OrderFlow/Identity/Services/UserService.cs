using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using OrderFlow.Identity.Models;

namespace OrderFlow.Identity.Services;

public class UserService(
    IHttpContextAccessor httpContextAccessor,
    UserManager<User> userManager,
    RoleManager<Role> roleManager
)
{
    public async Task<User> GetCurrentUserAsync()
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null || !claimsPrincipal.Identity.IsAuthenticated)
            throw new UnauthorizedAccessException();
        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return await userManager.FindByIdAsync(userId);
    }

    public async Task<bool> HasRoleAsync(Role? role)
    {
        var user = await GetCurrentUserAsync();
        var roles = await userManager.GetRolesAsync(user);
        if (roles.Contains(role!.ToString())) return true;
        role = await roleManager.FindByNameAsync(role.ToString());
        while (role != null)
        {
            if (roles.Contains(role.Name)) return true;
            role = await roleManager.FindByIdAsync(role.ParentRole!);
        }

        return false;
    }

    public async Task<bool> HasRoleAsync(List<Role> roles)
    {
        foreach (var role in roles)
            if (await HasRoleAsync(role))
                return true;
        return false;
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OrderFlow.Models.Identity;
using OrderFlow.Models.Identity.Roles;

namespace OrderFlow.Services;

public static class Initializer
{
    public static async Task InitializeRolesAndUsers(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var roles = new List<ApplicationRole>()
        {
            new User(),
            new Admin(),
        };
        
        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role.ToString());
            if (!roleExist) await roleManager.CreateAsync(role);
        }

        var poweruser = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
        };

        var userPWD = "Admin@123";
        var existingUser = await userManager.FindByEmailAsync("admin@example.com");
    
        if (existingUser == null)
        {
            var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
            if (createPowerUser.Succeeded) await userManager.AddToRoleAsync(poweruser, new Admin().ToString());
        }
    }
}
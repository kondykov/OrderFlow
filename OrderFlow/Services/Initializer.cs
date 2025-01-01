using Microsoft.AspNetCore.Identity;
using OrderFlow.Models.Identity;
using OrderFlow.Models.Identity.Roles;

namespace OrderFlow.Services;

public static class Initializer
{
    public static async Task InitializeRolesAndUsers(IServiceProvider serviceProvider)
    {
        var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roles = ["Users", "Admin", "Terminals"];

        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist) await roleManager.CreateAsync(new ApplicationRole() { Name = role });
        }

        var poweruser = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com"
        };

        var userPWD = "Admin@123";
        var existingUser = await userManager.FindByEmailAsync("admin@example.com");

        if (existingUser == null)
        {
            var createPowerUser = await userManager.CreateAsync(poweruser, userPWD);
            if (createPowerUser.Succeeded) await userManager.AddToRoleAsync(poweruser, new ApplicationRole() { Name = "Admin" }.ToString());
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OrderFlow.Models.Identity;

namespace OrderFlow.Services;

public static class Initializer
{
    public static async Task InitializeRolesAndUsers(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        string[] roleNames = ["Admin", "User"];
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
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
            if (createPowerUser.Succeeded) await userManager.AddToRoleAsync(poweruser, "Admin");
        }
    }
}
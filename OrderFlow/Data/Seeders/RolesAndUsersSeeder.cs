using Microsoft.AspNetCore.Identity;
using OrderFlow.Data.Interfaces;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Models.Roles;

namespace OrderFlow.Data.Seeders;

public class RolesAndUsersSeeder : IDataSeeder
{
    public async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

        var roles = new List<Role>
        {
            new Admin(),
            new Manager(),
            new Employee(),
            new Terminal(),
            new Client(),
        };
        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role.Name);
            if (!roleExist) await roleManager.CreateAsync(role);
        }

        var adminUser = new User
        {
            UserName = "Administrator", Email = "admin@example.com"
        };

        var userExist = await userManager.FindByEmailAsync(adminUser.Email);
        if (userExist == null)
        {
            var createAdminResult = await userManager.CreateAsync(adminUser, "Pa$$w0rd");
            if (createAdminResult.Succeeded) await userManager.AddToRoleAsync(adminUser, new Admin().ToString());
        }
    }
}
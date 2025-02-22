using Microsoft.AspNetCore.Identity;
using OrderFlow.Data;
using OrderFlow.Data.Seeders;
using OrderFlow.Identity.Handlers;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Services;

namespace OrderFlow.Configuration;

public static class ServiceInitializer
{
    public static void Handle(ref WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        
        builder.Services.AddScoped<AuthenticationHandler>();
        
        builder.Services.AddScoped<RolesAndUsersSeeder>();
    }
}
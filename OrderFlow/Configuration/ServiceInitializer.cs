using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using OrderFlow.Data;
using OrderFlow.Data.Seeders;
using OrderFlow.Document.Handlers;
using OrderFlow.Document.Repositories;
using OrderFlow.Handlers;
using OrderFlow.Identity.Handlers;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Services;

namespace OrderFlow.Configuration;

public static class ServiceInitializer
{
    public static void Handle(ref WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<Validator>();
        builder.Services.AddScoped<RolesAndUsersSeeder>();
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();
        
        builder.Services.AddScoped<AuthenticationHandler>();
        builder.Services.AddScoped<OrderItemHandler>();
        builder.Services.AddScoped<ProductHandler>();
        builder.Services.AddScoped<OrderHandler>();

        builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    }
}
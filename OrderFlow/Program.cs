using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OrderFlow.Configuration;
using OrderFlow.Data;
using OrderFlow.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default") ?? string.Empty));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

ServiceInitializer.Handle(ref builder);
SwaggerConfig.Handle(ref builder);
JwtConfig.Handle(ref builder);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<RolesAndUsersSeeder>();
    await seeder.SeedAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// TODO: Для использования данного посредника нужно реализовать отправку письма на почту
// app.UseMiddleware<AccountConfirmationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
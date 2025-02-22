using Microsoft.EntityFrameworkCore;
using OrderFlow.Configuration;
using OrderFlow.Data;
using OrderFlow.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<DataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default") ?? string.Empty));

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
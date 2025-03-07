using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OrderFlow.Models;

namespace OrderFlow.Configuration;

public class JwtConfig(IConfiguration config)
{
    public string Issuer { get; set; } = config["Jwt:Issuer"];
    public string Audience { get; set; } = config["Jwt:Audience"];
    public static void Handle(ref WebApplicationBuilder builder)
    {
        var secretKey = builder.Configuration["Jwt:SecretKey"];
        var key = Encoding.ASCII.GetBytes(secretKey);

        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // Установить true в продакшене
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.Response.ContentType = "application/json";
                        context.Response.Headers.AccessControlAllowOrigin = "*";
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new OperationResult<string>()
                        {
                            Error = "Unauthorized",
                            StatusCode = 401
                        }));
                    },
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["JWTCookie"];
                        return Task.CompletedTask;
                    }
                };
            });
    }
}
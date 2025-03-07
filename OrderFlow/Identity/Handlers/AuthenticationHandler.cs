using Microsoft.AspNetCore.Identity;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Models.Request;
using OrderFlow.Identity.Models.Response;
using OrderFlow.Identity.Services;
using OrderFlow.Models;

namespace OrderFlow.Identity.Handlers;

public class AuthenticationHandler(
    UserManager<User> userManager,
    IPasswordHasher<User> passwordHasher,
    TokenService service)
{
    public async Task<OperationResult<AuthenticationResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return new OperationResult<AuthenticationResponse>
            {
                Error = "Пользователь не найден",
                StatusCode = 404
            };
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return new OperationResult<AuthenticationResponse>
            {
                Error = "Пароль неверен",
                StatusCode = 400
            };

        return new OperationResult<AuthenticationResponse>
        {
            Data = new AuthenticationResponse
            {
                AccessToken = service.GenerateJwtToken(user)
            },
            StatusCode = 200
        };
    }

    public async Task<OperationResult<string>> RegisterAsync(RegisterRequest request)
    {
        var userExists = await userManager.FindByEmailAsync(request.Email);
        if (userExists != null)
            return new OperationResult<string>
            {
                Error = "Такой пользователь уже существует",
                StatusCode = 409
            };

        var user = await userManager.CreateAsync(new User
        {
            Email = request.Email,
            UserName = request.UserName
        }, request.Password);

        if (user.Succeeded)
            return new OperationResult<string>
            {
                Data = "Пользователь зарегистрирован",
                StatusCode = 202
            };

        return new OperationResult<string>
        {
            Error = "Переданы некорректные данный или произошла внутренняя ошибка",
            StatusCode = 400
        };
    }
}
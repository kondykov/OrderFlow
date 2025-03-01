﻿using Microsoft.AspNetCore.Identity;
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
                Error = "User does not exist"
            };
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return new OperationResult<AuthenticationResponse>
            {
                Error = "Invalid credentials"
            };

        return new OperationResult<AuthenticationResponse>
        {
            Data = new AuthenticationResponse
            {
                AccessToken = service.GenerateJwtToken(user)
            }
        };
    }

    public async Task<OperationResult<string>> RegisterAsync(RegisterRequest request)
    {
        var userExists = await userManager.FindByEmailAsync(request.Email);
        if (userExists != null)
            return new OperationResult<string>
            {
                Error = "User already exists"
            };

        var user = await userManager.CreateAsync(new User
        {
            Email = request.Email,
            UserName = request.UserName
        }, request.Password);

        if (user.Succeeded)
            return new OperationResult<string>
            {
                Data = "User created successfully"
            };
        
        return new OperationResult<string>
        {
            Error = "Credentials incorrect or creating user is failed"
        };
    }
}
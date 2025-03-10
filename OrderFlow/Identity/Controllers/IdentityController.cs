﻿using Microsoft.AspNetCore.Mvc;
using OrderFlow.Identity.Handlers;
using OrderFlow.Identity.Models.Request;

namespace OrderFlow.Identity.Controllers;

[Route("identity")]
public class IdentityController(AuthenticationHandler handler) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var operationResult = await handler.LoginAsync(model);
        return StatusCode(operationResult.StatusCode, operationResult);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var operationResult = await handler.RegisterAsync(model);
        return StatusCode(operationResult.StatusCode, operationResult);
    }
}
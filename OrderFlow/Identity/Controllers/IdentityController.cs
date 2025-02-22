using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Identity.Handlers;
using OrderFlow.Identity.Models;
using OrderFlow.Identity.Models.Request;
using OrderFlow.Models;

namespace OrderFlow.Identity.Controllers;

[Route("identity")]
public class IdentityController(AuthenticationHandler handler) : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var operationResult = await handler.LoginAsync(model);
        if (!operationResult.IsSuccessful) return BadRequest(operationResult);
        return Ok(operationResult);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        var operationResult = await handler.RegisterAsync(model);
        if (!operationResult.IsSuccessful) return BadRequest(operationResult);
        return Ok(operationResult);
    }

    [HttpGet("verify")]
    public async Task<IActionResult> Verify()
    {
        return Ok(new OperationResult<string>
        {
            Data = $"True"
        });
    }
}
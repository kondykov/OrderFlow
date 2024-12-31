using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderFlow.Models.Identity;
using OrderFlow.Models.Identity.Messages;
using OrderFlow.Services.Security;

namespace OrderFlow.Controllers.Identity;

[Route("[controller]/[action]")]
public class AccountController(
    RevokedTokenCleanupService revokedTokenCleanupService,
    AuthenticationService authenticationService,
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager,
    IConfiguration configuration
) : Controller
{
    internal static readonly ConcurrentDictionary<string, (bool, DateTime)> RevokedTokens = new();
    public CancellationToken RevokedTokensCts = CancellationToken.None;

    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] AuthRequestModel model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(new { Message = "Invalid request" });
        var user = await userManager.FindByNameAsync(model.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

        var token = authenticationService.GenerateJwtToken(user.Email);

        return Ok(new { Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegRequestModel model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Username
        };

        var result = await userManager.CreateAsync(user, model.Password);
        await userManager.AddToRoleAsync(user, "User");
        if (result.Succeeded) return Ok(new { Message = "User registered successfully!" });
        return Conflict(result.Errors.Select(e => e.Description));
    }

    [HttpPost]
    public async Task<IActionResult> Logout([FromBody] string token)
    {
        if (string.IsNullOrEmpty(token)) return BadRequest("Token is required.");
        RevokedTokens[token] = (true, DateTime.UtcNow);
        _ = revokedTokenCleanupService.StartAsync(RevokedTokensCts);
        return NoContent();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenHeader)) return;
        var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);
        if (RevokedTokens.ContainsKey(token)) context.Result = new ForbidResult();
    }
}
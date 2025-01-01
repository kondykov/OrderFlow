using System.Collections.Concurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderFlow.Models.Identity;
using OrderFlow.Models.Identity.Messages;
using OrderFlow.Models.Identity.Roles;
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
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var user = await userManager.FindByNameAsync(User.Identity!.Name!);
        var token = HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer ", string.Empty);
        RevokedTokens[token] = (true, DateTime.UtcNow);
        _ = revokedTokenCleanupService.StartAsync(RevokedTokensCts);
        return NoContent();
    }

    [HttpPost]
    [Authorize(Roles = (nameof(Admin)))]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleRequestModel model)
    {
        var user = await userManager.GetUserAsync(User);
        var role = await userManager.GetRolesAsync(user!);
        return Ok();
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenHeader)) return;
        var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);
        if (RevokedTokens.ContainsKey(token)) context.Result = new ForbidResult();
    }
}
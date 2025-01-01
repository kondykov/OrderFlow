using System.Collections.Concurrent;
using System.Security.Claims;
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
    ILogger<AccountController> logger,
    IConfiguration configuration
) : Controller
{
    internal static readonly ConcurrentDictionary<string, (bool, DateTime)> RevokedTokens = new();
    private readonly CancellationToken _revokedTokensCts = CancellationToken.None;

    [HttpPost]
    public async Task<IActionResult> Authorize([FromBody] AuthRequestModel model)
    {
        if (!ModelState.IsValid) return UnprocessableEntity(new { Message = "Invalid request" });
        var user = await userManager.FindByNameAsync(model.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

        var accessToken = authenticationService.GenerateJwtToken(user);
        var refreshToken = authenticationService.GenerateRefreshToken();

        user.AccessToken = accessToken;
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(7);
        await userManager.UpdateAsync(user);

        var authResponseModel = new AuthResponseModel
        {
            AccessToken = user.AccessToken,
            AccessTokenExpiresIn = DateTime.UtcNow.AddDays(1),
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiresIn = user.RefreshTokenExpiryTime
        };

        return Ok(authResponseModel);
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

        if (!result.Succeeded) return Conflict(result.Errors.Select(e => e.Description));
        result = await userManager.AddToRoleAsync(user, new User().ToString());
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
        _ = revokedTokenCleanupService.StartAsync(_revokedTokensCts);
        return NoContent();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateRole([FromBody] CreateOrUpdateRoleRequestMessage model)
    {
        var currentUser = await userManager.GetUserAsync(User);
        var currentUserRole = await userManager.GetRolesAsync(currentUser);
        if (!currentUserRole.Contains(new Admin().ToString())) return Forbid();
        
        var role = await roleManager.FindByNameAsync(model.RoleName);
        if (role != null) return Conflict(new { message = "Role already exists!" });
        await roleManager.CreateAsync(new ApplicationRole
        {
            Name = model.RoleName,
            NormalizedName = model.RoleName.ToUpper()
        });

        return Ok(new { Message = "Role created successfully!" });
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> UpdateRole([FromBody] CreateOrUpdateRoleRequestMessage model)
    {
        var role = await roleManager.FindByNameAsync(model.RoleName);
        if (role == null) return BadRequest(new { message = "Role not found" });
        if (role.ToString() == new Admin().ToString() || role.ToString() == new User().ToString()) return Forbid();


        role.Name = model.RoleName;
        role.NormalizedName = model.RoleName.ToUpper();
        await roleManager.UpdateAsync(role);

        return Ok(new { Message = "Role changed successfully!" });
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RemoveRole([FromBody] CreateOrUpdateRoleRequestMessage model)
    {
        var role = await roleManager.FindByNameAsync(model.RoleName);
        if (role == null) return BadRequest(new { message = "Role not found" });
        if (role == new Admin() || role == new User()) return Forbid();

        var usersHasRole = userManager.GetUsersInRoleAsync(role.Name!);


        return Ok(new { Message = "Role changed successfully!" });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> GiveRole([FromBody] ChangeRoleRequestModel model)
    {
        var user = await userManager.FindByEmailAsync(model.UserEmail);
        var role = await roleManager.FindByNameAsync(model.RoleName);

        if (user == null) return BadRequest(new { message = "User not found" });
        if (role == null) return BadRequest(new { Message = "Invalid role" });

        await userManager.RemoveFromRoleAsync(user, (await userManager.GetRolesAsync(user)).ToString() ?? string.Empty);
        await userManager.AddToRoleAsync(user, role.Name!);

        return Ok(new
        {
            Message = "Role added successfully!"
        });
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> RevokeRole([FromBody] ChangeRoleRequestModel model)
    {
        var user = await userManager.FindByEmailAsync(model.UserEmail);
        var role = await roleManager.FindByNameAsync(model.RoleName);

        if (user == null) return BadRequest(new { message = "User not found" });
        if (role == null) return BadRequest(new { Message = "Invalid role" });

        await userManager.RemoveFromRoleAsync(user, (await userManager.GetRolesAsync(user)).ToString() ?? string.Empty);

        return Ok(new
        {
            Message = "Role removed successfully!"
        });
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenHeader)) return;
        var token = tokenHeader.ToString().Replace("Bearer ", string.Empty);
        if (RevokedTokens.ContainsKey(token)) context.Result = new ForbidResult() ;
    }
}
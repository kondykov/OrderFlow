using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using OrderFlow.Data.Entities.Identity;
using OrderFlow.Infrastructure.Identity;
using OrderFlow.Infrastructure.Security;
using OrderFlow.Models;
using OrderFlow.Models.Identity;
using OrderFlow.Models.Identity.Messages.Requests;
using OrderFlow.Models.Identity.Messages.Response;

namespace OrderFlow.Controllers.Identity;

[Route("[controller]")]
public class IdentityController(
    IAuthenticationService authenticationService,
    IAccountRepository accountRepository,
    IRoleRepository roleRepository,
    IPasswordHasher passwordHasher
) : Controller
{
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Authorize([FromBody] RequestAuthMessage message)
    {
        if (!ModelState.IsValid) return BadRequest();
        var userSearchResult = await accountRepository.FindByUsernameAsync(message.Username);
        if (!userSearchResult.IsSuccessful)
            return NotFound(new { Message = "Account not found" });

        var user = userSearchResult.Data;
        Console.WriteLine(user.UserName);
        if (!passwordHasher.VerifyHash(message.Password, user.PasswordHash!))
            return StatusCode(422, new { Message = "Incorrect password" });

        var accessToken = authenticationService.GenerateAccessToken(user);
        var refreshToken = authenticationService.GenerateRefreshToken(user);

        var response = new ResponseAuthMessage
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            AccessTokenExpiresIn = accessToken?.ValidTo.ToString("MM/dd/yyyy HH:mm:ss"),
            RefreshToken = refreshToken,
            RefreshTokenExpiresIn = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy HH:mm:ss")
        };
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1).ToUniversalTime();
        await accountRepository.SaveAsync(user);
        return Ok(response);
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Register([FromBody] RequestRegMessage message)
    {
        if (!ModelState.IsValid) return BadRequest();

        ApplicationUser user = new()
        {
            UserName = message.Username,
            PasswordHash = passwordHasher.GenerateHash(message.Password)
        };

        var uC = await accountRepository.CreateAsync(user);

        if (!uC.IsSuccessful) return Conflict(new { message = uC.ErrorMessage });

        await accountRepository.SaveAsync(user);
        return Ok(new { Message = "User successfully registered!" });
    }

    [HttpPost]
    [Route("AddType")]
    public async Task<IActionResult> Add([FromBody] RequestAddTypeMessage type)
    {
        if (!ModelState.IsValid) return BadRequest(new ResponseMessage("Model is not valid"));
        var roleByCode = await roleRepository.FindRoleByCodeAsync(type.Code);
        if (!roleByCode.IsSuccessful)
            return BadRequest(new ResponseMessage("Account type already exists"));

        await roleRepository.AddRoleAsync(new Role
        {
            Code = type.Code,
            Name = type.Name
        });

        return Ok(new ResponseMessage("Type added successfully"));
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> GetRoles()
    {
        var types = await roleRepository.GetRolesAsync();
        if (!types.IsSuccessful) return NotFound(new ResponseMessage("No roles found"));

        return Ok(types.Data);
    }
}
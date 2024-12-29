using Microsoft.AspNetCore.Mvc;
using OrderFlow.Data;
using OrderFlow.Models.Identity;
using OrderFlow.Services.Validators;

namespace OrderFlow.Controllers.Identity;

[Route("api/[controller]/[action]")]
public class IdentityController(DataContext context) : Controller
{
    [HttpPost]
    public IActionResult Authorize([FromBody] RequestAuthMessage message)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!new EmailValidator().Validate(message.Username))
            return BadRequest();


        return Ok(context.Users.FirstOrDefault(u => u.UserName == message.Username));
    }

    [HttpPost]
    public IActionResult Register([FromBody] RequestAuthMessage message)
    {
        var u = new ApplicationUser
        {
            UserName = message.Username
        };
        context.Users.Add(u);
        context.SaveChanges();
        return Ok(u);
    }
}
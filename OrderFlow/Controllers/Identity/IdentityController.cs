using Microsoft.AspNetCore.Mvc;
using OrderFlow.Models.Identity;
using OrderFlow.Services.Validators;

namespace OrderFlow.Controllers.Identity;

[Route("api/[controller]/[action]")]
public class IdentityController : Controller
{
    [HttpPost]
    public IActionResult Authorize(RequestAuthMessage message)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!new EmailValidator().Validate(message.Username))
            return BadRequest();
        return Ok(message);
    }

    [HttpPost] 
    public IActionResult Register(RequestAuthMessage message)
    {
        return Ok(message);
    }
}
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
        return Ok();
    }

    [HttpPost]
    public IActionResult Register([FromBody] RequestAuthMessage message)
    {
        return Ok();
    }
}
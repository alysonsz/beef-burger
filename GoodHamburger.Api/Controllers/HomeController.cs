using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Home()
    {
        return Ok(new { message = "Welcome to Good Hamburger API" });
    }
}
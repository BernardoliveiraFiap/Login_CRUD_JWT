using Microsoft.AspNetCore.Mvc;

namespace ipoolBackend.Controllers;

[ApiController]
public class RootController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Get()
    {
        return Redirect("/scalar/v1");
    }
}

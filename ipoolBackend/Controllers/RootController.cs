using Microsoft.AspNetCore.Mvc;

namespace ipoolBackend.Controllers;

[ApiController]
public class RootController : ControllerBase
{
    [HttpGet("/")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public IActionResult Get()
    {
        return Redirect("/scalar/v1");
    }
}

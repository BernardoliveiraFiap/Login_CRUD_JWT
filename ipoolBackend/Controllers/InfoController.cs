using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ipoolBackend.DTOs;
using ipoolBackend.Options;

namespace ipoolBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoController : ControllerBase
{
    private readonly AppInfoOptions _options;
    private readonly IWebHostEnvironment _environment;

    public InfoController(IOptions<AppInfoOptions> options, IWebHostEnvironment environment)
    {
        _options = options.Value;
        _environment = environment;
    }

    [HttpGet]
    public ActionResult<ApiInfoResponse> Get()
    {
        var version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0";
        var environmentName = string.IsNullOrWhiteSpace(_options.Environment)
            ? _environment.EnvironmentName
            : _options.Environment;

        return Ok(new ApiInfoResponse(_options.Name, version, environmentName));
    }
}

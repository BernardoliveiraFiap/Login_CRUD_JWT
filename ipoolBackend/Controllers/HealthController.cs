using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ipoolBackend.Data;
using ipoolBackend.DTOs;

namespace ipoolBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public HealthController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<HealthResponse> Get()
    {
        return Ok(new HealthResponse("ok", true, DateTime.UtcNow));
    }

    [HttpGet("db")]
    public async Task<ActionResult<HealthResponse>> Database(CancellationToken cancellationToken)
    {
        var canConnect = await _dbContext.Database.CanConnectAsync(cancellationToken);
        var status = canConnect ? "ok" : "error";
        return Ok(new HealthResponse(status, canConnect, DateTime.UtcNow));
    }
}

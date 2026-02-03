using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ipoolBackend.DTOs;
using ipoolBackend.Services;

namespace ipoolBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAllAsync(cancellationToken);
        return Ok(users);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(currentUserId)
            && Guid.TryParse(currentUserId, out var parsedId)
            && parsedId == id)
        {
            return BadRequest(new ErrorResponse("Você não pode excluir o próprio usuário.", "self_delete"));
        }

        var deleted = await _userService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound(new ErrorResponse("Usuário não encontrado.", "user_not_found"));
        }

        return NoContent();
    }
}

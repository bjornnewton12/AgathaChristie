using AgathaChristie.Application.UseCases.Users.GetCurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgathaChristie.WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]

public sealed class UsersController : ControllerBase
{
    private readonly GetCurrentUserHandler _getCurrentHandler;

    public UsersController(GetCurrentUserHandler getCurrentUserHandler)
    {
        _getCurrentHandler = getCurrentUserHandler;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
            return Unauthorized();

        var result = await _getCurrentHandler.HandleAsync(new GetCurrentUserQuery(userId));

        if (result.Success)
            return Ok(result);

        return NotFound(result.Error);
    }
}
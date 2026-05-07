using AgathaChristie.Application.UseCases.UserBooks.GetUserBooks;
using AgathaChristie.Application.UseCases.UserBooks.UpdateUserBook;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AgathaChristie.WebApi.Controllers;

[ApiController]
[Route("api/userbooks")]
[Authorize]
public sealed class UserBooksController : ControllerBase
{
    private readonly GetUserBooksHandler _getHandler;
    private readonly UpdateUserBookHandler _updateHandler;

    public UserBooksController(GetUserBooksHandler getHandler, UpdateUserBookHandler updateHandler)
    {
        _getHandler = getHandler;
        _updateHandler = updateHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserBooks()
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var result = await _getHandler.HandleAsync(new GetUserBooksQuery(userId.Value));
        return Ok(result);
    }

    [HttpPut("{bookId:guid}")]
    public async Task<IActionResult> UpdateUserBook(Guid bookId, UpdateUserBookRequest request)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var cmd = new UpdateUserBookCommand(userId.Value, bookId, request.IsRead, request.DateRead, request.IsOwned);
        await _updateHandler.HandleAsync(cmd);
        return NoContent();
    }

    private Guid? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out var id) ? id : null;
    }
}

public sealed record UpdateUserBookRequest(bool IsRead, DateTime? DateRead, bool IsOwned);

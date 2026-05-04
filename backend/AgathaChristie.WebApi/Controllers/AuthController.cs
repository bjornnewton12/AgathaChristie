using AgathaChristie.Application.UseCases.Auth.CheckUsername;
using AgathaChristie.Application.UseCases.Auth.LoginUser;
using AgathaChristie.Application.UseCases.Auth.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace AgathaChristie.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly RegisterUserHandler _registerHandler;
    private readonly LoginUserHandler _loginHandler;
    private readonly CheckUsernameHandler _checkUsernameHandler;

    public AuthController(RegisterUserHandler registerHandler, LoginUserHandler loginHandler, CheckUsernameHandler checkUsernameHandler)
    {
        _registerHandler = registerHandler;
        _loginHandler = loginHandler;
        _checkUsernameHandler = checkUsernameHandler;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Username, request.Password);
        var result = await _registerHandler.HandleAsync(command);

        if (result.Success)
            return Ok(result);

        return Conflict(result.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginUserCommand(request.Username, request.Password);
        var result = await _loginHandler.HandleAsync(command);

        if (result.Success)
            return Ok(result);

        return Unauthorized(result.Error);
    }

    [HttpGet("check-username/{username}")]
    public async Task<IActionResult> CheckUsername(string username)
    {
        var exists = await _checkUsernameHandler.HandleAsync(username);
        return Ok(new { exists });
    }

    public sealed record RegisterRequest(string Username, string Password);
    public sealed record LoginRequest(string Username, string Password);
}

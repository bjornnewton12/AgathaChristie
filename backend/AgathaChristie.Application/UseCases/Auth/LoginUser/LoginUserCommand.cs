namespace AgathaChristie.Application.UseCases.Auth.LoginUser;

public sealed record LoginUserCommand(
    string Username,
    string Password
    );

namespace AgathaChristie.Application.UseCases.Auth.RegisterUser;

public sealed record RegisterUserCommand(
    string Username,
    string Password
    );

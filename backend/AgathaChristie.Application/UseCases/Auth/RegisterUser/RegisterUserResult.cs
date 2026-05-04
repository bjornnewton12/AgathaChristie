using AgathaChristie.Application.DTOs;

namespace AgathaChristie.Application.UseCases.Auth.RegisterUser;

public sealed record RegisterUserResult(bool Success, UserDto? User, string? Token, string? Error)
{
    public static RegisterUserResult Ok(UserDto user, string token) =>
        new(true, user, token, null);

    public static RegisterUserResult Fail(string error) =>
        new(false, null, null, error);
}

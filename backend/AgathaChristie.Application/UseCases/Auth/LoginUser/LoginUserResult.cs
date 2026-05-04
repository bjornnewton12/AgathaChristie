using AgathaChristie.Application.DTOs;

namespace AgathaChristie.Application.UseCases.Auth.LoginUser;

public sealed record LoginUserResult(bool Success, UserDto? User, string? Token, string? Error)
{
    public static LoginUserResult Ok(UserDto user, string token) =>
        new(true, user, token, null);

    public static LoginUserResult Fail(string error) =>
        new(false, null, null, error);
}

using AgathaChristie.Application.DTOs;

namespace AgathaChristie.Application.UseCases.Users.GetCurrentUser;

public sealed record GetCurrentUserResult(bool Success, UserDto? User, string? Error)
{
    public static GetCurrentUserResult Ok(UserDto user) =>
        new(true, user, null);

    public static GetCurrentUserResult Fail(string error) =>
        new(false, null, error);
}
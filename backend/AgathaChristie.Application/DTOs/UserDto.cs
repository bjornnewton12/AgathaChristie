namespace AgathaChristie.Application.DTOs;

public sealed record UserDto(
    Guid Id,
    string Username,
    DateTime CreatedAt
    );

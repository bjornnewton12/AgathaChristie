using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.UseCases.Auth.RegisterUser;

public sealed class RegisterUserHandler(IUserRepository userRepository,
IJwtService jwtService)
{
    public async Task<RegisterUserResult> HandleAsync(RegisterUserCommand cmd)
    {
        if (await userRepository.UsernameExistsAsync(cmd.Username))
            return RegisterUserResult.Fail("Username already taken");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = cmd.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(cmd.Password),
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.CreateAsync(user);
        var token = jwtService.GenerateToken(user);
        var userDto = new UserDto(user.Id, user.Username, user.CreatedAt);
        return RegisterUserResult.Ok(userDto, token);
    }
}
using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.UseCases.Auth.LoginUser;

public sealed class LoginUserHandler(IUserRepository userRepository, IJwtService
  jwtService)
{
    public async Task<LoginUserResult> HandleAsync(LoginUserCommand cmd)
    {
        var user = await userRepository.GetByUsernameAsync(cmd.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(cmd.Password, user.PasswordHash))
            return LoginUserResult.Fail("Invalid credentials");

        var token = jwtService.GenerateToken(user);
        var userDto = new UserDto(user.Id, user.Username, user.CreatedAt);
        return LoginUserResult.Ok(userDto, token);
    }
}
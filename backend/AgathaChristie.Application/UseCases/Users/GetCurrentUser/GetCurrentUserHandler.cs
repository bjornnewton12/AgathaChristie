using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.UseCases.Users.GetCurrentUser;

public sealed class GetCurrentUserHandler(IUserRepository userRepository)
{
    public async Task<GetCurrentUserResult> HandleAsync(GetCurrentUserQuery query)
    {
        var user = await userRepository.GetByIdAsync(query.UserId);
        if (user == null)
            return GetCurrentUserResult.Fail("User not found");

        var userDto = new UserDto(user.Id, user.Username, user.CreatedAt);

        return GetCurrentUserResult.Ok(userDto);
    }
}

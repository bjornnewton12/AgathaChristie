using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.UseCases.Auth.CheckUsername;

public class CheckUsernameHandler(IUserRepository userRepository)
{
    public async Task<bool> HandleAsync(string username)
    {
        return await userRepository.UsernameExistsAsync(username);
    }
}

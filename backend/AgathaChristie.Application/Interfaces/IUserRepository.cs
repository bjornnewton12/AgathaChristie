using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task AddAsync(User user);
}
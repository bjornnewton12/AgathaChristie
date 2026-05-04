using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(Guid id);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(Guid id);
}
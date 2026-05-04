using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}

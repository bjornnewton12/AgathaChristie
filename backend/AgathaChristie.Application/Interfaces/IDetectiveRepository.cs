using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IDetectiveRepository
{
    Task<IEnumerable<Detective>> GetAllAsync();
    Task<Detective?> GetByIdAsync(Guid id);
}
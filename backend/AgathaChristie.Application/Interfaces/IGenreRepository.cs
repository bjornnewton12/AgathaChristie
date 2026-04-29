using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<Genre?> GetByIdAsync(Guid id);
}
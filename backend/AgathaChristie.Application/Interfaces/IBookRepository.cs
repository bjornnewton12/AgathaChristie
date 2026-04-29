using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task<Book?> UpdateAsync(Guid id, Book updated, List<Guid> detectiveIds);
}
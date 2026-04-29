using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(Guid id);
    Task<Book> CreateAsync(Book book, List<Guid> detectiveIds);
    Task<Book?> UpdateAsync(Guid id, Book updated, List<Guid> detectiveIds);
    Task<bool> DeleteAsync(Guid id);
}
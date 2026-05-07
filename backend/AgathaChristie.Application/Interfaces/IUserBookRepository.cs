using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IUserBookRepository
{
    Task<IEnumerable<UserBook>> GetAllForUserAsync(Guid userId);
    Task UpsertAsync(Guid userId, Guid bookId, bool isRead, DateTime? dateRead, bool isOwned);
}

using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class UserBookRepository(AppDbContext db) : IUserBookRepository
{
    public async Task<IEnumerable<UserBook>> GetAllForUserAsync(Guid userId) =>
        await db.UserBooks.Where(ub => ub.UserId == userId).ToListAsync();

    public async Task UpsertAsync(Guid userId, Guid bookId, bool isRead, DateTime? dateRead, bool isOwnedEnglish, bool isOwnedSwedish)
    {
        var existing = await db.UserBooks.FirstOrDefaultAsync(ub => ub.UserId == userId && ub.BookId == bookId);

        if (existing is null)
        {
            db.UserBooks.Add(new UserBook
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId,
                IsRead = isRead,
                DateRead = dateRead,
                IsOwnedEnglish = isOwnedEnglish,
                IsOwnedSwedish = isOwnedSwedish
            });
        }
        else
        {
            existing.IsRead = isRead;
            existing.DateRead = dateRead;
            existing.IsOwnedEnglish = isOwnedEnglish;
            existing.IsOwnedSwedish = isOwnedSwedish;
        }

        await db.SaveChangesAsync();
    }
}

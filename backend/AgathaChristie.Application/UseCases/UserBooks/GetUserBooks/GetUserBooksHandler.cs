using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.UseCases.UserBooks.GetUserBooks;

public sealed class GetUserBooksHandler(IUserBookRepository userBookRepository)
{
    public async Task<IEnumerable<UserBookDto>> HandleAsync(GetUserBooksQuery query)
    {
        var userBooks = await userBookRepository.GetAllForUserAsync(query.UserId);
        return userBooks.Select(ub => new UserBookDto(ub.BookId, ub.IsRead, ub.DateRead, ub.IsOwnedEnglish, ub.IsOwnedSwedish));
    }
}

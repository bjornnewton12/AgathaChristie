using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.UseCases.UserBooks.UpdateUserBook;

public sealed class UpdateUserBookHandler(IUserBookRepository userBookRepository)
{
    public async Task HandleAsync(UpdateUserBookCommand cmd)
    {
        await userBookRepository.UpsertAsync(cmd.UserId, cmd.BookId, cmd.IsRead, cmd.DateRead, cmd.IsOwnedEnglish, cmd.IsOwnedSwedish);
    }
}

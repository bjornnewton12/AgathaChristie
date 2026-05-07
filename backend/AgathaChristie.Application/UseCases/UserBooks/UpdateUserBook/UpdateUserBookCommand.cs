namespace AgathaChristie.Application.UseCases.UserBooks.UpdateUserBook;

public sealed record UpdateUserBookCommand(
    Guid UserId,
    Guid BookId,
    bool IsRead,
    DateTime? DateRead,
    bool IsOwned
);

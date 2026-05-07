namespace AgathaChristie.Application.DTOs;

public sealed record UserBookDto(
    Guid BookId,
    bool IsRead,
    DateTime? DateRead,
    bool IsOwned
);

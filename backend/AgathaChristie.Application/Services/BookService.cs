using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<BookResponse>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(MapToResponse);
    }

    public async Task<BookResponse?> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        return book is null ? null : MapToResponse(book);
    }

    public async Task<BookResponse?> UpdateAsync(Guid id, BookRequest request)
    {
        var book = new Domain.Models.Book
        {
            Title = request.Title,
            TitleSwedish = request.TitleSwedish,
            ReleaseYear = request.ReleaseYear,
            IsShortStory = request.IsShortStory,
            Synopsis = request.Synopsis,
            Trivia = request.Trivia,
            GenreId = request.GenreId
        };

        var updated = await _bookRepository.UpdateAsync(id, book, request.DetectiveIds);
        return updated is null ? null : MapToResponse(updated);
    }

    private static BookResponse MapToResponse(Domain.Models.Book book) => new()
    {
        Id = book.Id,
        Title = book.Title,
        TitleSwedish = book.TitleSwedish,
        ReleaseYear = book.ReleaseYear,
        IsShortStory = book.IsShortStory,
        Synopsis = book.Synopsis,
        Trivia = book.Trivia,
        Genre = new GenreResponse { Id = book.Genre.Id, Name = book.Genre.Name },
        Detectives = book.BookDetectives.Select(bd => new DetectiveResponse
        {
            Id = bd.Detective.Id,
            Name = bd.Detective.Name,
            ShortName = bd.Detective.ShortName,
            HexColor = bd.Detective.HexColor
        })
    };
}

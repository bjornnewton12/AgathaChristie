using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;

    public BookRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _db.Books
            .Include(b => b.Genre)
            .Include(b => b.BookDetectives)
                .ThenInclude(bd => bd.Detective)
            .Include(b => b.MovieAdaptations)
            .Include(b => b.TVAdaptations)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        return await _db.Books
            .Include(b => b.Genre)
            .Include(b => b.BookDetectives)
                .ThenInclude(bd => bd.Detective)
            .Include(b => b.MovieAdaptations)
            .Include(b => b.TVAdaptations)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<Book?> UpdateAsync(Guid id, Book updated, List<Guid> detectiveIds)
    {
        var book = await _db.Books
            .Include(b => b.BookDetectives)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book is null) return null;

        book.Title = updated.Title;
        book.TitleSwedish = updated.TitleSwedish;
        book.ReleaseYear = updated.ReleaseYear;
        book.IsShortStory = updated.IsShortStory;
        book.Synopsis = updated.Synopsis;
        book.Trivia = updated.Trivia;
        book.GenreId = updated.GenreId;

        _db.BookDetectives.RemoveRange(book.BookDetectives);
        foreach (var detectiveId in detectiveIds)
            _db.BookDetectives.Add(new BookDetective
            {
                BookId = id,
                DetectiveId = detectiveId
            });

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

}
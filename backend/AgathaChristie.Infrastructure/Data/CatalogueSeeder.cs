using System.Text.Json;
using AgathaChristie.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Data;

public static class CatalogueSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Books.AnyAsync()) return;

        var json = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Data", "catalogue.json"));
        var catalogue = JsonSerializer.Deserialize<CatalogueData>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        await db.Genres.AddRangeAsync(catalogue.Genres);
        await db.Detectives.AddRangeAsync(catalogue.Detectives);
        await db.SaveChangesAsync();

        foreach (var bookDto in catalogue.Books)
        {
            var book = new Book
            {
                Id = bookDto.Id,
                Title = bookDto.Title,
                TitleSwedish = bookDto.TitleSwedish,
                ReleaseYear = bookDto.ReleaseYear,
                IsShortStory = bookDto.IsShortStory,
                GenreId = bookDto.GenreId
            };
            db.Books.Add(book);

            foreach (var detId in bookDto.DetectiveIds)
                db.BookDetectives.Add(new BookDetective { BookId = book.Id, DetectiveId = detId });
        }

        await db.SaveChangesAsync();
    }

    private class CatalogueData
    {
        public List<Genre> Genres { get; set; } = [];
        public List<Detective> Detectives { get; set; } = [];
        public List<BookDto> Books { get; set; } = [];
    }

    private class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? TitleSwedish { get; set; }
        public int ReleaseYear { get; set; }
        public bool IsShortStory { get; set; }
        public Guid GenreId { get; set; }
        public List<Guid> DetectiveIds { get; set; } = [];
    }
}
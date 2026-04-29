namespace AgathaChristie.Domain.Models;

public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleSwedish { get; set; }
    public int ReleaseYear { get; set; }
    public string? Synopsis { get; set; }
    public string? Trivia { get; set; }
    public bool IsShortStory { get; set; }
    public Guid GenreId { get; set; }
    public Genre Genre { get; set; } = null!;
    public ICollection<BookDetective> BookDetectives { get; set; } = [];

}

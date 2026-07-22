namespace AgathaChristie.Domain.Models;

public class MovieAdaptation
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public int TmdbId { get; set; }
}

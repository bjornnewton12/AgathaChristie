namespace AgathaChristie.Domain.Models;

public class TVAdaptation
{
    public Guid Id { get; set; }
    public int TmdbShowId { get; set; }
    public string SeriesName { get; set; } = string.Empty;
    public string? EpisodeTitle { get; set; }
    public int? SeasonNumber { get; set; }
    public int? EpisodeNumber { get; set; }
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
}
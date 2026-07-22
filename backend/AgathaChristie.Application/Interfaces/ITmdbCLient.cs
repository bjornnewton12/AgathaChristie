namespace AgathaChristie.Application.Interfaces;

public interface ITmdbClient
{
    Task<TmdbMovieResult?> GetMovieAsync(int tmdbId);
}

public class TmdbMovieResult
{
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
}
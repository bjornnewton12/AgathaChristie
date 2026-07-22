namespace AgathaChristie.Application.Interfaces;

public interface ITmdbClient
{
    Task<TmdbMovieResult?> GetMovieAsync(int tmdbId);
    Task<TmdbShowResult?> GetShowAsync(int showId);
    Task<TmdbEpisodeResult?> GetEpisodeAsync(int showId, int seasonNumber, int episodeNumber);
    Task<TmdbSeasonResult?> GetSeasonAsync(int showId, int seasonNumber);
}

public class TmdbMovieResult
{
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
}

public class TmdbShowResult
{
    public string Name { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
}

public class TmdbEpisodeResult
{
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
}

public class TmdbSeasonResult
{
    public string? PosterImageUrl { get; set; }
}
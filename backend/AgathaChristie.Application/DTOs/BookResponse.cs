using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.DTOs;

public class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleSwedish { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsShortStory { get; set; }
    public string? Synopsis { get; set; }
    public List<string> Trivia { get; set; } = [];
    public GenreResponse Genre { get; set; } = null!;
    public IEnumerable<DetectiveResponse> Detectives { get; set; } = [];
    public IEnumerable<MovieAdaptationResponse> MovieAdaptations { get; set; } = [];
    public IEnumerable<TVAdaptationResponse> TVAdaptations { get; set; } = [];
}

public class GenreResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class DetectiveResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string HexColor { get; set; } = string.Empty;
}

public class MovieAdaptationResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
}

public class TVAdaptationResponse
{
    public Guid Id { get; set; }
    public string SeriesName { get; set; } = string.Empty;
    public string? EpisodeTitle { get; set; }
    public int? SeasonNumber { get; set; }
    public int? EpisodeNumber { get; set; }
    public int ReleaseYear { get; set; }
    public string? PosterImageUrl { get; set; }
}
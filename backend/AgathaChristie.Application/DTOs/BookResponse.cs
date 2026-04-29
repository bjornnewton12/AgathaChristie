namespace AgathaChristie.Application.DTOs;

public class BookResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? TitleSwedish { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsShortStory { get; set; }
    public string? Synopsis { get; set; }
    public string? Trivia { get; set; }
    public GenreResponse Genre { get; set; } = null!;
    public IEnumerable<DetectiveResponse> Detectives { get; set; } = [];
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
    public string HexColor { get; set; } = string.Empty;
}
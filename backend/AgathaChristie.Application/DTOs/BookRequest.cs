namespace AgathaChristie.Application.DTOs;

public class BookRequest
{
    public string Title { get; set; } = string.Empty;
    public string? TitleSwedish { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsShortStory { get; set; }
    public string? Synopsis { get; set; }
    public string? Trivia { get; set; }
    public Guid GenreId { get; set; }
    public List<Guid> DetectiveIds { get; set; } = [];
  }
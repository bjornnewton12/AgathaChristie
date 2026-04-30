namespace AgathaChristie.Domain.Models;

public class Detective
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ShortName { get; set; }
    public string HexColor { get; set; } = string.Empty;
    public ICollection<BookDetective> BookDetectives { get; set; } = [];
}

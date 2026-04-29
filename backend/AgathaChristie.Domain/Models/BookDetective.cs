namespace AgathaChristie.Domain.Models;

public class BookDetective
{
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public Guid DetectiveId { get; set; }
    public Detective Detective { get; set; } = null!;
}

namespace AgathaChristie.Domain.Models;

public class UserBook
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime? DateRead { get; set; }
    public bool IsOwned { get; set; }
}

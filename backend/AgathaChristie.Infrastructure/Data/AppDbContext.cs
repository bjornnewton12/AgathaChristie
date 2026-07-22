using AgathaChristie.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();
    public DbSet<Detective> Detectives => Set<Detective>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<BookDetective> BookDetectives => Set<BookDetective>();
    public DbSet<MovieAdaptation> MovieAdaptations => Set<MovieAdaptation>();
    public DbSet<TVAdaptation> TVAdaptations => Set<TVAdaptation>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserBook> UserBooks => Set<UserBook>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookDetective>()
            .HasKey(bd => new { bd.BookId, bd.DetectiveId });
    }
}
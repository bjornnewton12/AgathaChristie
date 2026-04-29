using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AppDbContext _db;

    public GenreRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync() =>
        await _db.Genres.ToListAsync();

    public async Task<Genre?> GetByIdAsync(Guid id) =>
        await _db.Genres.FindAsync(id);
}
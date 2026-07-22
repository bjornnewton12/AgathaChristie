using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class MovieAdaptationRepository : IMovieAdaptationRepository
{
    private readonly AppDbContext _db;

    public MovieAdaptationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MovieAdaptation> AddAsync(MovieAdaptation adaptation)
    {
        _db.MovieAdaptations.Add(adaptation);
        await _db.SaveChangesAsync();
        return adaptation;
    }

    public async Task<bool> ExistsAsync(Guid bookId, int tmdbId)
    {
        return await _db.MovieAdaptations.AnyAsync(m => m.BookId == bookId && m.TmdbId == tmdbId);
    }
}

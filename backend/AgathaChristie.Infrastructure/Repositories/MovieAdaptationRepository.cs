using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;

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
}

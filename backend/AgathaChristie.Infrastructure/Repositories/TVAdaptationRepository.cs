using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;
using AgathaChristie.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AgathaChristie.Infrastructure.Repositories;

public class TVAdaptationRepository : ITVAdaptationRepository
{
    private readonly AppDbContext _db;

    public TVAdaptationRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TVAdaptation> AddAsync(TVAdaptation adaptation)
    {
        _db.TVAdaptations.Add(adaptation);
        await _db.SaveChangesAsync();
        return adaptation;
    }

    public async Task<bool> ExistsAsync(Guid bookId, int tmdbShowId, int? seasonNumber, int? episodeNumber)
    {
        return await _db.TVAdaptations.AnyAsync(t =>
            t.BookId == bookId &&
            t.TmdbShowId == tmdbShowId &&
            t.SeasonNumber == seasonNumber &&
            t.EpisodeNumber == episodeNumber);
    }
}

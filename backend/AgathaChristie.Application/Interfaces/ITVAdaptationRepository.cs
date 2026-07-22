using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface ITVAdaptationRepository
{
    Task<TVAdaptation> AddAsync(TVAdaptation adaptation);
    Task<bool> ExistsAsync(Guid bookId, int tmdbShowId, int? seasonNumber, int? episodeNumber);
}

using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Interfaces;

public interface IMovieAdaptationRepository
{
    Task<MovieAdaptation> AddAsync(MovieAdaptation adaptation);
}

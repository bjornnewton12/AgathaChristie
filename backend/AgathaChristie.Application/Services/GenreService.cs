using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;

namespace AgathaChristie.Application.Services;

public class GenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreResponse>> GetAllAsync()
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres.Select(g => new GenreResponse
        {
            Id = g.Id,
            Name = g.Name
        });
    }

    public async Task<GenreResponse?> GetByIdAsync(Guid id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        return genre is null ? null : new GenreResponse
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
}
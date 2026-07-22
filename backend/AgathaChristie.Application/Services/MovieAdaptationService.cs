using System.Text.RegularExpressions;
using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Services;

public class MovieAdaptationService
{
    private readonly IMovieAdaptationRepository _movieAdaptationRepository;
    private readonly ITmdbClient _tmdbClient;

    public MovieAdaptationService(IMovieAdaptationRepository movieAdaptationRepository, ITmdbClient tmdbClient)
    {
        _movieAdaptationRepository = movieAdaptationRepository;
        _tmdbClient = tmdbClient;
    }

    public async Task<MovieAdaptationResponse?> CreateAsync(Guid bookId, MovieAdaptationRequest request)
    {
        var match = Regex.Match(request.TmdbUrl, @"movie/(\d+)");
        if (!match.Success) return null;
        var tmdbId = int.Parse(match.Groups[1].Value);

        var movie = await _tmdbClient.GetMovieAsync(tmdbId);
        if (movie is null) return null;

        var adaptation = new MovieAdaptation
        {
            BookId = bookId,
            TmdbId = tmdbId,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            PosterImageUrl = movie.PosterImageUrl
        };

        var created = await _movieAdaptationRepository.AddAsync(adaptation);

        return new MovieAdaptationResponse
        {
            Id = created.Id,
            Title = created.Title,
            ReleaseYear = created.ReleaseYear,
            PosterImageUrl = created.PosterImageUrl
        };
    }
}
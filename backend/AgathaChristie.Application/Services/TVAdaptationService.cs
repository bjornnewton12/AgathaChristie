using System.Text.RegularExpressions;
using AgathaChristie.Application.DTOs;
using AgathaChristie.Application.Interfaces;
using AgathaChristie.Domain.Models;

namespace AgathaChristie.Application.Services;

public class TVAdaptationService
{
    private readonly ITVAdaptationRepository _tvAdaptationRepository;
    private readonly ITmdbClient _tmdbClient;

    public TVAdaptationService(ITVAdaptationRepository tvAdaptationRepository, ITmdbClient tmdbClient)
    {
        _tvAdaptationRepository = tvAdaptationRepository;
        _tmdbClient = tmdbClient;
    }

    public async Task<TVAdaptationResponse?> CreateAsync(Guid bookId, TVAdaptationRequest request)
    {
        var episodeMatch = Regex.Match(request.TmdbUrl, @"tv/(\d+).*season/(\d+)/episode/(\d+)");
        if (episodeMatch.Success)
        {
            var showId = int.Parse(episodeMatch.Groups[1].Value);
            var seasonNumber = int.Parse(episodeMatch.Groups[2].Value);
            var episodeNumber = int.Parse(episodeMatch.Groups[3].Value);

            if (await _tvAdaptationRepository.ExistsAsync(bookId, showId, seasonNumber, episodeNumber))
                throw new InvalidOperationException("This episode has already been added.");

            var show = await _tmdbClient.GetShowAsync(showId);
            var episode = await _tmdbClient.GetEpisodeAsync(showId, seasonNumber, episodeNumber);
            var season = await _tmdbClient.GetSeasonAsync(showId, seasonNumber);
            if (show is null || episode is null || season is null) return null;

            var adaptation = new TVAdaptation
            {
                BookId = bookId,
                TmdbShowId = showId,
                SeriesName = show.Name,
                EpisodeTitle = episode.Title,
                SeasonNumber = seasonNumber,
                EpisodeNumber = episodeNumber,
                ReleaseYear = episode.ReleaseYear,
                PosterImageUrl = season.PosterImageUrl
            };

            var created = await _tvAdaptationRepository.AddAsync(adaptation);
            return MapToResponse(created);
        }

        var showMatch = Regex.Match(request.TmdbUrl, @"tv/(\d+)");
        if (showMatch.Success)
        {
            var showId = int.Parse(showMatch.Groups[1].Value);

            if (await _tvAdaptationRepository.ExistsAsync(bookId, showId, null, null))
                throw new InvalidOperationException("This show has already been added.");

            var show = await _tmdbClient.GetShowAsync(showId);
            if (show is null) return null;

            var adaptation = new TVAdaptation
            {
                BookId = bookId,
                TmdbShowId = showId,
                SeriesName = show.Name,
                ReleaseYear = show.ReleaseYear,
                PosterImageUrl = show.PosterImageUrl
            };

            var created = await _tvAdaptationRepository.AddAsync(adaptation);
            return MapToResponse(created);
        }

        return null;
    }

    private static TVAdaptationResponse MapToResponse(TVAdaptation t) => new()
    {
        Id = t.Id,
        SeriesName = t.SeriesName,
        EpisodeTitle = t.EpisodeTitle,
        SeasonNumber = t.SeasonNumber,
        EpisodeNumber = t.EpisodeNumber,
        ReleaseYear = t.ReleaseYear,
        PosterImageUrl = t.PosterImageUrl
    };
}

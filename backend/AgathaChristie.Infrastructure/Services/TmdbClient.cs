using System.Text.Json;
using System.Text.Json.Serialization;
using AgathaChristie.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AgathaChristie.Infrastructure.Services;

public class TmdbClient : ITmdbClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TmdbClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Tmdb:ApiKey"]!;
    }

    public async Task<TmdbMovieResult?> GetMovieAsync(int tmdbId)
    {
        var response = await _httpClient.GetAsync($"movie/{tmdbId}?api_key={_apiKey}");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<TmdbMovieApiResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (data is null) return null;

        var releaseYear = DateTime.TryParse(data.ReleaseDate, out var date) ? date.Year : 0;
        var posterUrl = data.PosterPath is null ? null : $"https://image.tmdb.org/t/p/w500{data.PosterPath}";

        return new TmdbMovieResult
        {
            Title = data.Title,
            ReleaseYear = releaseYear,
            PosterImageUrl = posterUrl
        };
    }

    public async Task<TmdbShowResult?> GetShowAsync(int showId)
    {
        var response = await _httpClient.GetAsync($"tv/{showId}?api_key={_apiKey}");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<TmdbShowApiResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (data is null) return null;

        var releaseYear = DateTime.TryParse(data.FirstAirDate, out var date) ? date.Year : 0;
        var posterUrl = data.PosterPath is null ? null : $"https://image.tmdb.org/t/p/w500{data.PosterPath}";

        return new TmdbShowResult
        {
            Name = data.Name,
            ReleaseYear = releaseYear,
            PosterImageUrl = posterUrl
        };
    }

    public async Task<TmdbEpisodeResult?> GetEpisodeAsync(int showId, int seasonNumber, int episodeNumber)
    {
        var response = await _httpClient.GetAsync($"tv/{showId}/season/{seasonNumber}/episode/{episodeNumber}?api_key={_apiKey}");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<TmdbEpisodeApiResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (data is null) return null;

        var releaseYear = DateTime.TryParse(data.AirDate, out var date) ? date.Year : 0;
        var posterUrl = data.StillPath is null ? null : $"https://image.tmdb.org/t/p/w500{data.StillPath}";

        return new TmdbEpisodeResult
        {
            Title = data.Name,
            ReleaseYear = releaseYear,
            PosterImageUrl = posterUrl
        };
    }

    public async Task<TmdbSeasonResult?> GetSeasonAsync(int showId, int seasonNumber)
    {
        var response = await _httpClient.GetAsync($"tv/{showId}/season/{seasonNumber}?api_key={_apiKey}");
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<TmdbSeasonApiResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (data is null) return null;

        var posterUrl = data.PosterPath is null ? null : $"https://image.tmdb.org/t/p/w500{data.PosterPath}";

        return new TmdbSeasonResult
        {
            PosterImageUrl = posterUrl
        };
    }

    private class TmdbMovieApiResponse
    {
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
    }

    private class TmdbShowApiResponse
    {
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("first_air_date")]
        public string? FirstAirDate { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
    }

    private class TmdbEpisodeApiResponse
    {
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("air_date")]
        public string? AirDate { get; set; }

        [JsonPropertyName("still_path")]
        public string? StillPath { get; set; }
    }

    private class TmdbSeasonApiResponse
    {
        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
    }
}
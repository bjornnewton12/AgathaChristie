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

    private class TmdbMovieApiResponse
    {
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("release_date")]
        public string? ReleaseDate { get; set; }

        [JsonPropertyName("poster_path")]
        public string? PosterPath { get; set; }
    }
}
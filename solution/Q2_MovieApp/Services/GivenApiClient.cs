using System.Text.Json;
using Q2_MovieApp.Models;

namespace Q2_MovieApp.Services;

public class GivenApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public GivenApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseUrl = configuration["GivenAPIBaseUrl"]
            ?? throw new InvalidOperationException("GivenAPIBaseUrl is not configured.");
    }

    public Task<List<DirectorResponse>> GetDirectorsAsync(CancellationToken cancellationToken = default) =>
        GetJsonAsync<List<DirectorResponse>>("/api/Directors/GetDirectors", cancellationToken);

    public Task<List<MovieResponse>> GetMoviesAsync(CancellationToken cancellationToken = default) =>
        GetJsonAsync<List<MovieResponse>>("/api/Movies/GetMovies", cancellationToken);

    public Task<List<MovieResponse>> GetMoviesByDirectorIdAsync(int directorId, CancellationToken cancellationToken = default) =>
        GetJsonAsync<List<MovieResponse>>($"/api/Movies/GetMoviesByDirectorId/{directorId}", cancellationToken);

    public async Task DeleteMovieAsync(int movieId, CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Movies/DeleteMovie/{movieId}", cancellationToken);
        // DeleteMovie returns 204 No Content on success; never attempt to read/deserialize a body.
        response.EnsureSuccessStatusCode();
    }

    private async Task<T> GetJsonAsync<T>(string relativeUrl, CancellationToken cancellationToken) where T : new()
    {
        using var response = await _httpClient.GetAsync($"{_baseUrl}{relativeUrl}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        return JsonSerializer.Deserialize<T>(json, JsonOptions) ?? new T();
    }
}

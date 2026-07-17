using Microsoft.Extensions.Configuration;

namespace Q2_MovieApp.Tests;

/// <summary>
/// Shared test helpers for building a GivenApiClient without real network/config dependencies.
/// </summary>
internal static class TestConfig
{
    public static IConfiguration CreateFakeConfiguration(string baseUrl = "http://localhost:5100")
    {
        var configDict = new Dictionary<string, string?>
        {
            ["GivenAPIBaseUrl"] = baseUrl
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(configDict)
            .Build();
    }
}

/// <summary>
/// Fake IHttpClientFactory implementation for testing.
/// Returns the provided HttpClient for all CreateClient calls.
/// </summary>
internal class FakeHttpClientFactory : IHttpClientFactory
{
    private readonly HttpClient _httpClient;

    public FakeHttpClientFactory(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public HttpClient CreateClient(string name = "")
    {
        return _httpClient;
    }
}

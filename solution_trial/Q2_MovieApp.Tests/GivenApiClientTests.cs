using System.Net;
using System.Text.Json;
using Q2_MovieApp.Models;
using Q2_MovieApp.Services;
using static Q2_MovieApp.Tests.TestConfig;

namespace Q2_MovieApp.Tests;

public class GivenApiClientTests
{
    /// <summary>
    /// Helper to create a fake IHttpClientFactory with a stubbed HttpMessageHandler
    /// that returns the provided response for any request.
    /// </summary>
    private static IHttpClientFactory CreateFakeHttpClientFactory(HttpResponseMessage responseMessage)
    {
        var handler = new StubHttpMessageHandler(responseMessage);
        var httpClient = new HttpClient(handler);

        var mockFactory = new FakeHttpClientFactory(httpClient);
        return mockFactory;
    }

    #region GetDirectorsAsync Tests

    [Fact]
    public async Task GetDirectorsAsync_WithValidResponse_DeserializesCorrectly()
    {
        // Arrange
        var cannedJson = """
            [
              {
                "id": 1,
                "fullName": "Steven Spielberg",
                "male": true,
                "dob": "1946-12-18T00:00:00",
                "nationality": "USA",
                "description": "Legendary filmmaker"
              },
              {
                "id": 2,
                "fullName": "Greta Gerwig",
                "male": false,
                "dob": "1983-08-04T00:00:00",
                "nationality": "USA",
                "description": "Contemporary filmmaker"
              }
            ]
            """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetDirectorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var spielberg = result[0];
        Assert.Equal(1, spielberg.Id);
        Assert.Equal("Steven Spielberg", spielberg.FullName);
        Assert.True(spielberg.Male);
        Assert.Equal(new DateTime(1946, 12, 18), spielberg.Dob);
        Assert.Equal("USA", spielberg.Nationality);
        Assert.Equal("Legendary filmmaker", spielberg.Description);

        var gerwig = result[1];
        Assert.Equal(2, gerwig.Id);
        Assert.Equal("Greta Gerwig", gerwig.FullName);
        Assert.False(gerwig.Male);
        Assert.Equal("USA", gerwig.Nationality);
    }

    [Fact]
    public async Task GetDirectorsAsync_CallsCorrectUrl()
    {
        // Arrange
        var cannedJson = "[]";
        var capturedUri = (Uri?)null;

        var handler = new CaptureUriHttpMessageHandler(capturedUri, new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        });

        var httpClient = new HttpClient(handler);
        var mockFactory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration("http://localhost:5100");

        var client = new GivenApiClient(mockFactory, config);

        // Act
        await client.GetDirectorsAsync();

        // Assert
        var expectedUrl = "http://localhost:5100/api/Directors/GetDirectors";
        Assert.NotNull(handler.CapturedUri);
        Assert.Equal(expectedUrl, handler.CapturedUri.ToString());
    }

    [Fact]
    public async Task GetDirectorsAsync_WithEmptyResponse_ReturnsEmptyList()
    {
        // Arrange
        var cannedJson = "[]";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetDirectorsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDirectorsAsync_With404Response_ThrowsHttpRequestException()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetDirectorsAsync());
    }

    #endregion

    #region GetMoviesAsync Tests

    [Fact]
    public async Task GetMoviesAsync_WithValidResponse_DeserializesCorrectly()
    {
        // Arrange
        var cannedJson = """
            [
              {
                "id": 1,
                "title": "Jaws",
                "releaseDate": "1975-06-20T00:00:00",
                "description": "A great thriller",
                "language": "English",
                "producerId": 1,
                "directorId": 1,
                "director": {
                  "id": 1,
                  "fullName": "Steven Spielberg",
                  "male": true,
                  "dob": "1946-12-18T00:00:00",
                  "nationality": "USA",
                  "description": "Legendary filmmaker"
                },
                "stars": [
                  {
                    "id": 1,
                    "fullName": "Jamie Lee Curtis",
                    "male": false,
                    "dob": "1954-11-22T00:00:00",
                    "nationality": "USA",
                    "description": "Accomplished actress"
                  }
                ],
                "genres": [
                  {
                    "id": 1,
                    "title": "Thriller"
                  }
                ]
              }
            ]
            """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetMoviesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);

        var movie = result[0];
        Assert.Equal(1, movie.Id);
        Assert.Equal("Jaws", movie.Title);
        Assert.Equal(new DateTime(1975, 6, 20), movie.ReleaseDate);
        Assert.Equal("A great thriller", movie.Description);
        Assert.Equal("English", movie.Language);
        Assert.Equal(1, movie.ProducerId);
        Assert.Equal(1, movie.DirectorId);
    }

    [Fact]
    public async Task GetMoviesAsync_WithNestedDirector_DeserializesDirectorCorrectly()
    {
        // Arrange
        var cannedJson = """
            [
              {
                "id": 1,
                "title": "Jaws",
                "releaseDate": "1975-06-20T00:00:00",
                "description": "A great thriller",
                "language": "English",
                "producerId": null,
                "directorId": 1,
                "director": {
                  "id": 1,
                  "fullName": "Steven Spielberg",
                  "male": true,
                  "dob": "1946-12-18T00:00:00",
                  "nationality": "USA",
                  "description": "Legendary filmmaker"
                },
                "stars": [],
                "genres": []
              }
            ]
            """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetMoviesAsync();

        // Assert
        var movie = result[0];
        Assert.NotNull(movie.Director);
        Assert.Equal(1, movie.Director.Id);
        Assert.Equal("Steven Spielberg", movie.Director.FullName);
        Assert.True(movie.Director.Male);
        Assert.Equal("USA", movie.Director.Nationality);
    }

    [Fact]
    public async Task GetMoviesAsync_WithStarInfo_PopulatesStarFullNameCorrectly()
    {
        // Arrange - THIS TEST GUARDS AGAINST A REAL BUG: star object should use FullName, not Name
        var cannedJson = """
            [
              {
                "id": 1,
                "title": "Jaws",
                "releaseDate": "1975-06-20T00:00:00",
                "description": "A great thriller",
                "language": "English",
                "producerId": null,
                "directorId": 1,
                "director": {
                  "id": 1,
                  "fullName": "Steven Spielberg",
                  "male": true,
                  "dob": "1946-12-18T00:00:00",
                  "nationality": "USA",
                  "description": "Legendary filmmaker"
                },
                "stars": [
                  {
                    "id": 100,
                    "fullName": "Jamie Lee Curtis",
                    "male": false,
                    "dob": "1954-11-22T00:00:00",
                    "nationality": "USA",
                    "description": "Accomplished actress"
                  },
                  {
                    "id": 101,
                    "fullName": "Roy Scheider",
                    "male": true,
                    "dob": "1932-11-10T00:00:00",
                    "nationality": "USA",
                    "description": "Classic actor"
                  }
                ],
                "genres": []
              }
            ]
            """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetMoviesAsync();

        // Assert
        var movie = result[0];
        Assert.NotNull(movie.Stars);
        Assert.Equal(2, movie.Stars.Count);

        // CRITICAL: Verify Stars[0].FullName is populated (not Name)
        Assert.Equal("Jamie Lee Curtis", movie.Stars[0].FullName);
        Assert.Equal(100, movie.Stars[0].Id);

        Assert.Equal("Roy Scheider", movie.Stars[1].FullName);
        Assert.Equal(101, movie.Stars[1].Id);
    }

    [Fact]
    public async Task GetMoviesAsync_WithGenres_PopulatesGenresCorrectly()
    {
        // Arrange
        var cannedJson = """
            [
              {
                "id": 1,
                "title": "Jaws",
                "releaseDate": "1975-06-20T00:00:00",
                "description": "A great thriller",
                "language": "English",
                "producerId": null,
                "directorId": 1,
                "director": {
                  "id": 1,
                  "fullName": "Steven Spielberg",
                  "male": true,
                  "dob": "1946-12-18T00:00:00",
                  "nationality": "USA",
                  "description": "Legendary filmmaker"
                },
                "stars": [],
                "genres": [
                  {
                    "id": 5,
                    "title": "Thriller"
                  },
                  {
                    "id": 6,
                    "title": "Horror"
                  }
                ]
              }
            ]
            """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetMoviesAsync();

        // Assert
        var movie = result[0];
        Assert.NotNull(movie.Genres);
        Assert.Equal(2, movie.Genres.Count);
        Assert.Equal("Thriller", movie.Genres[0].Title);
        Assert.Equal(5, movie.Genres[0].Id);
        Assert.Equal("Horror", movie.Genres[1].Title);
        Assert.Equal(6, movie.Genres[1].Id);
    }

    [Fact]
    public async Task GetMoviesAsync_CallsCorrectUrl()
    {
        // Arrange
        var cannedJson = "[]";
        var handler = new CaptureUriHttpMessageHandler(null, new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        });

        var httpClient = new HttpClient(handler);
        var mockFactory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration("http://localhost:5100");

        var client = new GivenApiClient(mockFactory, config);

        // Act
        await client.GetMoviesAsync();

        // Assert
        var expectedUrl = "http://localhost:5100/api/Movies/GetMovies";
        Assert.NotNull(handler.CapturedUri);
        Assert.Equal(expectedUrl, handler.CapturedUri.ToString());
    }

    #endregion

    #region GetMoviesByDirectorIdAsync Tests

    [Fact]
    public async Task GetMoviesByDirectorIdAsync_WithValidId_DeserializesCorrectly()
    {
        // Arrange
        var cannedJson = """
            [
              {
                "id": 1,
                "title": "Jaws",
                "releaseDate": "1975-06-20T00:00:00",
                "description": "A great thriller",
                "language": "English",
                "producerId": null,
                "directorId": 1,
                "director": {
                  "id": 1,
                  "fullName": "Steven Spielberg",
                  "male": true,
                  "dob": "1946-12-18T00:00:00",
                  "nationality": "USA",
                  "description": "Legendary filmmaker"
                },
                "stars": [],
                "genres": []
              }
            ]
            """;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetMoviesByDirectorIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Jaws", result[0].Title);
        Assert.Equal(1, result[0].DirectorId);
    }

    [Fact]
    public async Task GetMoviesByDirectorIdAsync_CallsCorrectUrlWithDirectorId()
    {
        // Arrange
        var cannedJson = "[]";
        var handler = new CaptureUriHttpMessageHandler(null, new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        });

        var httpClient = new HttpClient(handler);
        var mockFactory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration("http://localhost:5100");

        var client = new GivenApiClient(mockFactory, config);

        // Act
        await client.GetMoviesByDirectorIdAsync(42);

        // Assert
        var expectedUrl = "http://localhost:5100/api/Movies/GetMoviesByDirectorId/42";
        Assert.NotNull(handler.CapturedUri);
        Assert.Equal(expectedUrl, handler.CapturedUri.ToString());
    }

    [Fact]
    public async Task GetMoviesByDirectorIdAsync_WithDifferentDirectorIds_UsesCorrectUrlPath()
    {
        // Arrange
        var cannedJson = "[]";
        var handler = new CaptureUriHttpMessageHandler(null, new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        });

        var httpClient = new HttpClient(handler);
        var mockFactory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration("http://localhost:5100");

        var client = new GivenApiClient(mockFactory, config);

        // Act
        await client.GetMoviesByDirectorIdAsync(999);

        // Assert
        var expectedUrl = "http://localhost:5100/api/Movies/GetMoviesByDirectorId/999";
        Assert.NotNull(handler.CapturedUri);
        Assert.Equal(expectedUrl, handler.CapturedUri.ToString());
    }

    [Fact]
    public async Task GetMoviesByDirectorIdAsync_WithEmptyResponse_ReturnsEmptyList()
    {
        // Arrange
        var cannedJson = "[]";
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(cannedJson)
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act
        var result = await client.GetMoviesByDirectorIdAsync(999);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region DeleteMovieAsync Tests

    [Fact]
    public async Task DeleteMovieAsync_With204NoContent_SucceedsWithoutThrowingOrReadingBody()
    {
        // Arrange - THIS TEST GUARDS AGAINST A REAL BUG: code that tries to deserialize a 204 response body
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NoContent)
        {
            Content = new StringContent("")  // Empty body, as per givenAPI DeleteMovie endpoint
        };

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act & Assert - should NOT throw, should NOT attempt to read/deserialize body
        await client.DeleteMovieAsync(1);  // Should complete successfully
    }

    [Fact]
    public async Task DeleteMovieAsync_CallsCorrectUrlWithMovieId()
    {
        // Arrange
        var handler = new CaptureMethodAndUriHttpMessageHandler(null, null, new HttpResponseMessage(HttpStatusCode.NoContent)
        {
            Content = new StringContent("")
        });

        var httpClient = new HttpClient(handler);
        var mockFactory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration("http://localhost:5100");

        var client = new GivenApiClient(mockFactory, config);

        // Act
        await client.DeleteMovieAsync(5);

        // Assert
        var expectedUrl = "http://localhost:5100/api/Movies/DeleteMovie/5";
        Assert.NotNull(handler.CapturedUri);
        Assert.Equal(expectedUrl, handler.CapturedUri.ToString());
        Assert.Equal(HttpMethod.Delete, handler.CapturedMethod);
    }

    [Fact]
    public async Task DeleteMovieAsync_UsesDeleteHttpMethod()
    {
        // Arrange
        var handler = new CaptureMethodAndUriHttpMessageHandler(null, null, new HttpResponseMessage(HttpStatusCode.NoContent)
        {
            Content = new StringContent("")
        });

        var httpClient = new HttpClient(handler);
        var mockFactory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(mockFactory, config);

        // Act
        await client.DeleteMovieAsync(1);

        // Assert
        Assert.Equal(HttpMethod.Delete, handler.CapturedMethod);
    }

    [Fact]
    public async Task DeleteMovieAsync_With404Response_ThrowsHttpRequestException()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.DeleteMovieAsync(999));
    }

    [Fact]
    public async Task DeleteMovieAsync_With500Response_ThrowsHttpRequestException()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);

        var httpClientFactory = CreateFakeHttpClientFactory(responseMessage);
        var config = CreateFakeConfiguration();

        var client = new GivenApiClient(httpClientFactory, config);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.DeleteMovieAsync(1));
    }

    #endregion
}

/// <summary>
/// Stub HttpMessageHandler that always returns the provided response.
/// </summary>
internal class StubHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpResponseMessage _responseMessage;

    public StubHttpMessageHandler(HttpResponseMessage responseMessage)
    {
        _responseMessage = responseMessage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_responseMessage);
    }
}

/// <summary>
/// HttpMessageHandler that captures the request URI for verification.
/// </summary>
internal class CaptureUriHttpMessageHandler : HttpMessageHandler
{
    public Uri? CapturedUri { get; private set; }
    private readonly HttpResponseMessage _responseMessage;

    public CaptureUriHttpMessageHandler(Uri? initialUri, HttpResponseMessage responseMessage)
    {
        CapturedUri = initialUri;
        _responseMessage = responseMessage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CapturedUri = request.RequestUri;
        return await Task.FromResult(_responseMessage);
    }
}

/// <summary>
/// HttpMessageHandler that captures both the request method and URI for verification.
/// </summary>
internal class CaptureMethodAndUriHttpMessageHandler : HttpMessageHandler
{
    public Uri? CapturedUri { get; private set; }
    public HttpMethod? CapturedMethod { get; private set; }
    private readonly HttpResponseMessage _responseMessage;

    public CaptureMethodAndUriHttpMessageHandler(Uri? initialUri, HttpMethod? initialMethod, HttpResponseMessage responseMessage)
    {
        CapturedUri = initialUri;
        CapturedMethod = initialMethod;
        _responseMessage = responseMessage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        CapturedUri = request.RequestUri;
        CapturedMethod = request.Method;
        return await Task.FromResult(_responseMessage);
    }
}

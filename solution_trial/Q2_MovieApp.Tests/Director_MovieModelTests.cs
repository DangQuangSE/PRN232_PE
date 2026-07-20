using System.Net;
using Microsoft.AspNetCore.Mvc;
using Q2_MovieApp.Pages.Movies;
using Q2_MovieApp.Services;
using static Q2_MovieApp.Tests.TestConfig;

namespace Q2_MovieApp.Tests;

public class Director_MovieModelTests
{
    private const string DirectorsJson = """
        [
          { "id": 1, "fullName": "David Gordon Green", "male": true, "dob": "1975-04-09T00:00:00", "nationality": "USA", "description": "..." },
          { "id": 2, "fullName": "Aaron Horvath", "male": true, "dob": "1980-08-19T00:00:00", "nationality": "USA", "description": "..." }
        ]
        """;

    private const string AllMoviesJson = """
        [
          {
            "id": 2, "title": "Halloween Ends", "releaseDate": "2022-10-14T00:00:00",
            "description": "...", "language": "English", "producerId": 3, "directorId": 1,
            "director": { "id": 1, "fullName": "David Gordon Green", "male": true, "dob": "1975-04-09T00:00:00", "nationality": "USA", "description": "..." },
            "stars": [
              { "id": 1, "fullName": "Jamie Lee Curtis", "male": false, "dob": "1958-11-22T00:00:00", "nationality": "USA", "description": "..." },
              { "id": 2, "fullName": "Andi Matichak", "male": false, "dob": "1991-01-05T00:00:00", "nationality": "USA", "description": "..." }
            ],
            "genres": []
          },
          {
            "id": 3, "title": "The Super Mario Bros. Movie", "releaseDate": "2023-04-07T00:00:00",
            "description": "...", "language": "English", "producerId": 6, "directorId": 2,
            "director": { "id": 2, "fullName": "Aaron Horvath", "male": true, "dob": "1980-08-19T00:00:00", "nationality": "USA", "description": "..." },
            "stars": [],
            "genres": []
          }
        ]
        """;

    private const string DirectorFilteredMoviesJson = """
        [
          {
            "id": 3, "title": "The Super Mario Bros. Movie", "releaseDate": "2023-04-07T00:00:00",
            "description": "...", "language": "English", "producerId": 6, "directorId": 2,
            "director": { "id": 2, "fullName": "Aaron Horvath", "male": true, "dob": "1980-08-19T00:00:00", "nationality": "USA", "description": "..." },
            "stars": [],
            "genres": []
          }
        ]
        """;

    private class RoutingHttpMessageHandler : HttpMessageHandler
    {
        public List<string> RequestedUrls { get; } = new();

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var url = request.RequestUri!.ToString();
            RequestedUrls.Add(url);

            string body;
            if (url.Contains("/api/Directors/GetDirectors"))
            {
                body = DirectorsJson;
            }
            else if (url.Contains("/api/Movies/GetMoviesByDirectorId/"))
            {
                body = DirectorFilteredMoviesJson;
            }
            else if (url.Contains("/api/Movies/GetMovies"))
            {
                body = AllMoviesJson;
            }
            else if (request.Method == HttpMethod.Delete)
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent)
                {
                    Content = new StringContent("")
                });
            }
            else
            {
                body = "[]";
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(body)
            });
        }
    }

    private static (Director_MovieModel Model, RoutingHttpMessageHandler Handler) CreateModel()
    {
        var handler = new RoutingHttpMessageHandler();
        var httpClient = new HttpClient(handler);
        var factory = new FakeHttpClientFactory(httpClient);
        var config = CreateFakeConfiguration();
        var apiClient = new GivenApiClient(factory, config);
        var model = new Director_MovieModel(apiClient);
        return (model, handler);
    }

    [Fact]
    public async Task OnGetAsync_WithNoDirectorId_LoadsAllMoviesAndAllDirectors()
    {
        var (model, handler) = CreateModel();
        model.DirectorId = null;

        await model.OnGetAsync();

        Assert.Equal(2, model.Directors.Count);
        Assert.Equal(2, model.Movies.Count);
        Assert.Contains(handler.RequestedUrls, u => u.EndsWith("/api/Movies/GetMovies"));
        Assert.DoesNotContain(handler.RequestedUrls, u => u.Contains("GetMoviesByDirectorId"));
    }

    [Fact]
    public async Task OnGetAsync_WithDirectorId_LoadsOnlyThatDirectorsMovies()
    {
        var (model, handler) = CreateModel();
        model.DirectorId = 2;

        await model.OnGetAsync();

        Assert.Single(model.Movies);
        Assert.Equal("The Super Mario Bros. Movie", model.Movies[0].Title);
        Assert.Contains(handler.RequestedUrls, u => u.EndsWith("/api/Movies/GetMoviesByDirectorId/2"));
        Assert.DoesNotContain(handler.RequestedUrls, u => u.EndsWith("/api/Movies/GetMovies"));
    }

    [Fact]
    public async Task OnGetAsync_PopulatesStarFullNameCorrectly()
    {
        var (model, _) = CreateModel();
        model.DirectorId = null;

        await model.OnGetAsync();

        var halloweenEnds = model.Movies.Single(m => m.Id == 2);
        Assert.Equal(2, halloweenEnds.Stars.Count);
        Assert.Equal("Jamie Lee Curtis", halloweenEnds.Stars[0].FullName);
        Assert.Equal("Andi Matichak", halloweenEnds.Stars[1].FullName);
    }

    [Fact]
    public async Task OnGetDeleteAsync_CallsDeleteAndRedirectsToPage()
    {
        var (model, handler) = CreateModel();

        var result = await model.OnGetDeleteAsync(9);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.Contains(handler.RequestedUrls, u => u.EndsWith("/api/Movies/DeleteMovie/9"));
    }
}

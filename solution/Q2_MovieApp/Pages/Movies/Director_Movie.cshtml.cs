using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2_MovieApp.Models;
using Q2_MovieApp.Services;

namespace Q2_MovieApp.Pages.Movies;

public class Director_MovieModel : PageModel
{
    private readonly GivenApiClient _client;

    public Director_MovieModel(GivenApiClient client)
    {
        _client = client;
    }

    [BindProperty(SupportsGet = true)]
    public int? DirectorId { get; set; }

    public List<DirectorResponse> Directors { get; set; } = new();
    public List<MovieResponse> Movies { get; set; } = new();

    public async Task OnGetAsync()
    {
        var directorsTask = _client.GetDirectorsAsync();
        var moviesTask = DirectorId.HasValue
            ? _client.GetMoviesByDirectorIdAsync(DirectorId.Value)
            : _client.GetMoviesAsync();

        await Task.WhenAll(directorsTask, moviesTask);
        Directors = directorsTask.Result;
        Movies = moviesTask.Result;
    }

    public async Task<IActionResult> OnGetDeleteAsync(int id)
    {
        await _client.DeleteMovieAsync(id);
        return RedirectToPage();
    }
}

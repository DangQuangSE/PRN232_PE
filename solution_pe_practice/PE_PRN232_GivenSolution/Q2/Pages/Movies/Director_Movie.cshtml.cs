using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models.Dtos;

namespace Q2.Pages.Movies
{
    public class Director_MovieModel : PageModel
    {
        public List<MovieViewModel> Movies { get; set; } = [];
        public List<DirectorViewModel> Directors { get; set; } = [];
        public int? SelectedDirectorId { get; set; }
        public async Task OnGetAsync(int? directorId)
        {
            using HttpClient client = new HttpClient();
            SelectedDirectorId = directorId;

            string directorUrl = Utilities.GetAbsoluteUrl("api/Directors/GetDirectors");
            Directors = await client.GetFromJsonAsync<List<DirectorViewModel>>(directorUrl) ?? [];
            if (directorId == null)
            {
                string movieUrl = Utilities.GetAbsoluteUrl("api/Movies/GetMovies");
                Movies = await client.GetFromJsonAsync<List<MovieViewModel>>(movieUrl) ?? [];
            }
            else
            {
                string movieUrl = Utilities.GetAbsoluteUrl($"api/Movies/GetMoviesByDirectorId/{directorId}");
                Movies = await client.GetFromJsonAsync<List<MovieViewModel>>(movieUrl) ?? [];
            }
        }
        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            using HttpClient client = new HttpClient();
            string deleteUrl = Utilities.GetAbsoluteUrl($"api/Movies/DeleteMovie/{id}");
            HttpResponseMessage response = await client.DeleteAsync(deleteUrl);
            response.EnsureSuccessStatusCode();
            return Redirect("/Movies/Director_Movie");

        }
    }
}

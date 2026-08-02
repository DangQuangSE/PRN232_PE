using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Q2.Models.Dtos;

namespace Q2.Pages.Movies
{
    [IgnoreAntiforgeryToken]
    public class Director_MovieModel : PageModel
    {
        public List<MovieDto> Movies { get; set; } = [];
        public List<DirectorDto> Directors { get; set; } = [];

        [BindProperty]
        public UpdateMovieDto EditMovie { get; set; } = new();

        public bool IsEditing { get; set; }

        public string? EditError { get; set; }
        public string? DeleteError { get; set; }
        public string? SearchError { get; set; }
        public string? SearchMessage { get; set; }

        // Preserve submitted search criteria so the form stays filled in after searching.
        public string? Title { get; set; }
        public string? Language { get; set; }
        public int? DirectorId { get; set; }
        public int? FromYear { get; set; }
        public int? ToYear { get; set; }

        private string GetGivenAPIBaseURL()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string baseURL = config["GivenAPIBaseUrl"]!;
            return baseURL;
        }

        public async Task OnGetAsync(int? directorId, int? editId,
            string? title, string? language, int? fromYear, int? toYear)
        {
            using HttpClient client = new HttpClient();
            string baseURL = GetGivenAPIBaseURL();

            await LoadDirectors(client);

            Title = title;
            Language = language;
            DirectorId = directorId;
            FromYear = fromYear;
            ToYear = toYear;

            bool hasOtherCriteria = !string.IsNullOrWhiteSpace(title)
                || !string.IsNullOrWhiteSpace(language)
                || fromYear != null
                || toYear != null;

            if (hasOtherCriteria)
            {
                if (fromYear != null && toYear != null && fromYear > toYear)
                {
                    SearchError = "From year must not exceed to year.";
                }
                else
                {
                    var query = new List<string>();
                    if (!string.IsNullOrWhiteSpace(title)) query.Add($"title={Uri.EscapeDataString(title)}");
                    if (!string.IsNullOrWhiteSpace(language)) query.Add($"language={Uri.EscapeDataString(language)}");
                    if (directorId != null) query.Add($"directorId={directorId}");
                    if (fromYear != null) query.Add($"fromYear={fromYear}");
                    if (toYear != null) query.Add($"toYear={toYear}");

                    string url = $"{baseURL}/api/Movies/SearchMovies?" + string.Join("&", query);
                    Movies = await client.GetFromJsonAsync<List<MovieDto>>(url) ?? [];

                    if (Movies.Count == 0)
                        SearchMessage = "No movies found.";
                }
            }
            else if (directorId != null)
            {
                string url = $"{baseURL}/api/Movies/GetMoviesByDirectorId/{directorId}";
                Movies = await client.GetFromJsonAsync<List<MovieDto>>(url) ?? [];
            }
            else
            {
                string url = $"{baseURL}/api/Movies/GetMovies";
                Movies = await client.GetFromJsonAsync<List<MovieDto>>(url) ?? [];
            }

            if (editId != null)
            {
                string url = $"{baseURL}/api/Movies/GetMovieById/{editId}";
                MovieDto? movie = await client.GetFromJsonAsync<MovieDto>(url);

                if (movie != null)
                {
                    IsEditing = true;
                    EditMovie = new UpdateMovieDto
                    {
                        Id = movie.Id,
                        Title = movie.Title,
                        ReleaseDate = movie.ReleaseDate,
                        Description = movie.Description,
                        Language = movie.Language,
                        DirectorId = movie.Director?.Id
                    };
                }
            }
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            using HttpClient client = new HttpClient();
            string baseURL = GetGivenAPIBaseURL();
            string url = $"{baseURL}/api/Movies/UpdateMovie/{EditMovie.Id}";

            HttpResponseMessage response = await client.PutAsJsonAsync(url, EditMovie);

            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Movies/Director_Movie");
            }

            IsEditing = true;
            EditError = "Update movie failed.";
            await LoadDirectors(client);
            Movies = await client.GetFromJsonAsync<List<MovieDto>>($"{baseURL}/api/Movies/GetMovies") ?? [];
            return Page();
        }

        public async Task<IActionResult> OnGetDeleteAsync(int id)
        {
            using HttpClient client = new HttpClient();
            string baseURL = GetGivenAPIBaseURL();
            string url = $"{baseURL}/api/Movies/DeleteMovie/{id}";

            HttpResponseMessage response = await client.DeleteAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                DeleteError = "Delete movie failed.";
                await LoadDirectors(client);
                Movies = await client.GetFromJsonAsync<List<MovieDto>>($"{baseURL}/api/Movies/GetMovies") ?? [];
                return Page();
            }

            return Redirect("/Movies/Director_Movie");
        }

        private async Task LoadDirectors(HttpClient client)
        {
            string baseURL = GetGivenAPIBaseURL();
            string url = $"{baseURL}/api/Directors/GetDirectors";
            Directors = await client.GetFromJsonAsync<List<DirectorDto>>(url) ?? [];
        }
    }
}

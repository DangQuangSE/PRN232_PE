using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Q2.Models.Dtos;

namespace Q2.Pages.Movies
{
    [IgnoreAntiforgeryToken]
    public class Create_MovieModel : PageModel
    {
        public List<DirectorDto> Directors { get; set; } = [];

        [BindProperty]
        public CreateMovieDto NewMovie { get; set; } = new();

        public string? CreateError { get; set; }

        private string GetGivenAPIBaseURL()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            string baseURL = config["GivenAPIBaseUrl"]!;
            return baseURL;
        }

        public async Task OnGetAsync()
        {
            using HttpClient client = new HttpClient();
            await LoadDirectors(client);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using HttpClient client = new HttpClient();
            string baseURL = GetGivenAPIBaseURL();
            string url = $"{baseURL}/api/Movies/CreateMovie";

            HttpResponseMessage response = await client.PostAsJsonAsync(url, NewMovie);

            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Movies/Director_Movie");
            }

            CreateError = "Create movie failed.";
            await LoadDirectors(client);
            return Page();
        }

        private async Task LoadDirectors(HttpClient client)
        {
            string baseURL = GetGivenAPIBaseURL();
            string url = $"{baseURL}/api/Directors/GetDirectors";
            Directors = await client.GetFromJsonAsync<List<DirectorDto>>(url) ?? [];
        }
    }
}

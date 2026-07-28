using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models.Dtos;

namespace Q2.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class Author_BookModel : PageModel
    {
        public List<BookModelView> Books { get; set; } = [];
        public List<AuthorModelView> Authors { get; set; } = [];
        public int? SelectedAuthorId { get; set; }
        [BindProperty]
        public CreateBookRequestQ2 Input { get; set; } = new();
        public string? CreateError { get; set; }
        public async Task OnGetAsync(int? authorId)
        {
            using HttpClient client = new HttpClient();
            string getAllAuthors = Utilities.GetAbsoluteUrl("api/Authors/GetAuthors");
            Authors = await client.GetFromJsonAsync<List<AuthorModelView>>(getAllAuthors) ?? [];
            SelectedAuthorId = authorId;
            if (authorId == null)
            {
                string getAllBooks = Utilities.GetAbsoluteUrl("api/Books/GetBooks");
                Books = await client.GetFromJsonAsync<List<BookModelView>>(getAllBooks) ?? [];
            }
            else
            {
                string getBooksById = Utilities.GetAbsoluteUrl($"api/Books/GetBooksByAuthorId/{authorId}");
                Books = await client.GetFromJsonAsync<List<BookModelView>>(getBooksById) ?? [];
            }

        }
        public async Task<IActionResult> OnPostAsync()
        {
            using HttpClient client = new HttpClient();
            string createUrl = Utilities.GetAbsoluteUrl("api/Books/CreateBook");
            HttpResponseMessage response = await client.PostAsJsonAsync(createUrl, Input);
            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Books/Author_Book");
            }
            CreateError = await response.Content.ReadAsStringAsync();
            await OnGetAsync(null);
            return Page();
        }

    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q2.Models.Dtos;

namespace Q2.Pages
{
    [IgnoreAntiforgeryToken]
    public class Designer_ProductModel : PageModel
    {
        public List<ProductDto> Products { get; set; } = [];
        public List<DesignerDto> Designers { get; set; } = [];

        [BindProperty]
        public UpdateProductDto EditProduct { get; set; } = new();

        public bool IsEditing { get; set; }

        public string? EditError { get; set; }

        public async Task OnGetAsync(int? id, int? editId)
        {
            using HttpClient client = new HttpClient();

            await LoadDesigners(client);
            await LoadProducts(client, id);

            if (editId != null)
            {
                string url = Utilities.GetAbsoluteUrl(
                    $"api/Products/GetProductById/{editId}");

                ProductDto? product =
                    await client.GetFromJsonAsync<ProductDto>(url);

                if (product != null)
                {
                    IsEditing = true;

                    EditProduct = new UpdateProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        LaunchDate = product.LaunchDate,
                        Description = product.Description,
                        Material = product.Material,
                        DesignerId = product.Designer?.Id
                    };
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using HttpClient client = new HttpClient();

            string url = Utilities.GetAbsoluteUrl(
                $"api/Products/UpdateProduct/{EditProduct.Id}");

            var body = new UpdateProductRequest
            {
              Name = EditProduct.Name,
              LaunchDate = EditProduct.LaunchDate,
              Description= EditProduct.Description,
              Material = EditProduct.Material,
              DesignerId = EditProduct.DesignerId
            };

            HttpResponseMessage response =
                await client.PutAsJsonAsync(url, body);

            if (response.IsSuccessStatusCode)
            {
                return Redirect("/Products/Designer_Product");
            }

            IsEditing = true;
            EditError = "Update product failed.";

            await LoadDesigners(client);
            await LoadProducts(client, null);

            return Page();
        }

        private async Task LoadDesigners(HttpClient client)
        {
            string url = Utilities.GetAbsoluteUrl(
                "api/Designers/GetDesigners");

            Designers =
                await client.GetFromJsonAsync<List<DesignerDto>>(url) ?? [];
        }

        private async Task LoadProducts(HttpClient client, int? designerId)
        {
            string path = designerId == null
                ? "api/Products/GetProducts"
                : $"api/Products/GetProductsByDesignerId/{designerId}";

            string url = Utilities.GetAbsoluteUrl(path);

            Products =
                await client.GetFromJsonAsync<List<ProductDto>>(url) ?? [];
        }
    }
}
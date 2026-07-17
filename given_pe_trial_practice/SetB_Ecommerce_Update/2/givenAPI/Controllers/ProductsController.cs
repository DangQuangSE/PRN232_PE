// Controllers/ProductsController.cs
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;
using givenAPI.Models.Responses;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Helper method to convert Product to ProductResponse
        private ProductResponse ProductToResponse(Product p)
        {
            var designers = DataInitializer.Designers;
            var reviewers = DataInitializer.Reviewers;
            var tags = DataInitializer.Tags;
            var productReviewers = DataInitializer.ProductReviewers;
            var productTags = DataInitializer.ProductTags;

            var designer = designers.FirstOrDefault(d => d.Id == p.DesignerId);

            return new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                LaunchDate = p.LaunchDate,
                Description = p.Description,
                Material = p.Material,
                DesignerId = p.DesignerId,

                Designer = designer is null ? null : new ProductResponse.DesignerInfo
                {
                    Id = designer.Id,
                    FullName = designer.FullName,
                    Male = designer.Male,
                    Dob = designer.Dob,
                    Nationality = designer.Nationality,
                    Description = designer.Description
                },

                Reviewers = productReviewers
                    .Where(pr => pr.ProductId == p.Id)
                    .Select(pr =>
                    {
                        var r = reviewers.First(rev => rev.Id == pr.ReviewerId);
                        return new ProductResponse.ReviewerInfo
                        {
                            Id = r.Id,
                            FullName = r.FullName,
                            Male = r.Male,
                            Dob = r.Dob,
                            Description = r.Description,
                            Nationality = r.Nationality
                        };
                    })
                    .ToList(),

                Tags = productTags
                    .Where(pt => pt.ProductId == p.Id)
                    .Select(pt =>
                    {
                        var t = tags.First(tag => tag.Id == pt.TagId);
                        return new ProductResponse.TagInfo
                        {
                            Id = t.Id,
                            Title = t.Title
                        };
                    })
                    .ToList()
            };
        }

        // GET: api/Products/GetProducts
        [HttpGet("GetProducts")]
        public ActionResult<List<ProductResponse>> GetProducts()
        {
            var products = DataInitializer.Products;
            var result = products.Select(p => ProductToResponse(p)).ToList();
            return Ok(result);
        }

        // GET: api/Products/GetProductsByDesignerId/{designerId}
        [HttpGet("GetProductsByDesignerId/{designerId}")]
        public ActionResult<List<ProductResponse>> GetProductsByDesignerId(int designerId)
        {
            var products = DataInitializer.Products.Where(p => p.DesignerId == designerId);
            var result = products.Select(p => ProductToResponse(p)).ToList();
            return Ok(result);
        }

        // GET: api/Products/GetProductById/{id}
        [HttpGet("GetProductById/{id}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            var product = DataInitializer.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { Message = $"Product with Id = {id} not found." });

            var result = ProductToResponse(product);
            return Ok(result);
        }

        // PUT: api/Products/UpdateProduct/{id}
        [HttpPut("UpdateProduct/{id}")]
        public ActionResult<ProductResponse> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            var product = DataInitializer.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound(new { Message = $"Product with Id = {id} not found." });

            // Update the product fields
            product.Name = request.Name;
            product.LaunchDate = request.LaunchDate;
            product.Description = request.Description;
            product.Material = request.Material;
            product.DesignerId = request.DesignerId;

            var result = ProductToResponse(product);
            return Ok(result);
        }
    }

    // DTO for update request
    public class UpdateProductRequest
    {
        public string Name { get; set; } = null!;
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string Material { get; set; } = null!;
        public int? DesignerId { get; set; }
    }
}

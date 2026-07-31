using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Mapping;
using Q1.Models;
using Q1.Models.Dtos;

namespace Q1.Controllers
{
    [ApiController]
    [Route("/api/author")]

    public class AuthorController : ControllerBase
    {
        private readonly PE_Practice_LibraryAContext _context;
        public AuthorController(PE_Practice_LibraryAContext context)
        {
            _context = context;
        }
        [HttpGet("getauthors/{nationality}/{gender}")]
        public async Task<ActionResult<List<AuthorDto>>> GetAuthors(string nationality, string gender)
        {
            bool isMale = gender.Equals("male", StringComparison.OrdinalIgnoreCase);
            var authors = await _context.Authors.
                Where(a => a.Nationality == nationality && a.Male == isMale)
                .ToListAsync();
            return Ok(authors.Select(a => a.ToAuthorDto()).ToList());
        }
        [HttpGet("/getauthor/{id}")]
        public async Task<ActionResult<AuthorWithBookDTO>> GetAuthorById(int id)
        {
            var author = await _context.Authors
                .Include(a => a.Books).ThenInclude(b => b.Publisher)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return NotFound();
            return Ok(author.ToAuthorWithBookDto());
        }
        [HttpPost("/create")]
        public async Task<IActionResult> Create(CreateAuthorRequest request)
        {
            try 
            {
                var author = new Author
                {
                    FullName = request.FullName,
                    Male = request.Male,
                    Dob = request.Dob,
                    Nationality = request.Nationality,
                    Description = request.Description
                };
                _context.Authors.Add(author);
                int rowAdded = _context.SaveChanges();
                return Ok(rowAdded);
            }
            catch
            {
                return Conflict("There is an error while adding.");
            }
        }
    }
}

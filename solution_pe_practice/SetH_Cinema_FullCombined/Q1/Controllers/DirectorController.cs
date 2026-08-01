using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Mapping;
using Q1.Models;
using Q1.Models.Dtos;

namespace Q1.Controllers
{
    [ApiController]
    [Route("api/director")]
    public class DirectorController : ControllerBase
    {
        private readonly PE_Practice_CinemaHContext _context;
        public DirectorController(PE_Practice_CinemaHContext context)
        {
            _context = context;
        }

        [HttpGet("getdirectors/{nationality}/{gender}")]
        public async Task<ActionResult<List<DirectorDto>>> GetDirectors(string nationality, string gender)
        {
            bool isMale = gender.Equals("male", StringComparison.OrdinalIgnoreCase);

            var directors = await _context.Directors
                .Where(d => d.Nationality.ToLower() == nationality.ToLower() && d.Male == isMale)
                .ToListAsync();
            return Ok(directors.Select(d => d.ToDirectorDto()).ToList());
        }

        [HttpGet("getdirector/{id}")]
        public async Task<ActionResult<DirectorWithMoviesDto>> GetDirectorById(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movies).ThenInclude(m => m.Producer)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (director == null) return NotFound();
            return Ok(director.ToDirectorWithMoviesDto());
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateDirectorRequest request)
        {
            try
            {
                var director = new Director
                {
                    FullName = request.FullName,
                    Male = request.Male,
                    Dob = request.Dob,
                    Nationality = request.Nationality,
                    Description = request.Description,
                };
                _context.Directors.Add(director);
                int rowsAdded = await _context.SaveChangesAsync();
                return Ok(rowsAdded);
            }
            catch
            {
                return Conflict("There is an error while adding.");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, UpdateDirectorRequest request)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director == null) return NotFound("Director not found.");

            try
            {
                director.FullName = request.FullName;
                director.Male = request.Male;
                director.Dob = request.Dob;
                director.Nationality = request.Nationality;
                director.Description = request.Description;

                int rowsAffected = await _context.SaveChangesAsync();
                return Ok(rowsAffected);
            }
            catch
            {
                return Conflict("There is an error while updating.");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var director = await _context.Directors
                .Include(d => d.Movies)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (director == null) return NotFound("Director not found.");

            if (director.Movies.Any())
                return Conflict("Cannot delete a director having movies.");

            try
            {
                _context.Directors.Remove(director);
                int rowsAffected = await _context.SaveChangesAsync();
                return Ok(rowsAffected);
            }
            catch
            {
                return Conflict("There is an error while deleting.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<DirectorDto>>> Search(
            string? name, string? nationality, string? gender, DateOnly? fromDob, DateOnly? toDob)
        {
            if (!string.IsNullOrEmpty(gender)
                && !gender.Equals("male", StringComparison.OrdinalIgnoreCase)
                && !gender.Equals("female", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Invalid gender.");
            }

            var query = _context.Directors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(d => d.FullName.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(nationality))
                query = query.Where(d => d.Nationality.ToLower() == nationality.ToLower());

            if (!string.IsNullOrEmpty(gender))
            {
                bool isMale = gender.Equals("male", StringComparison.OrdinalIgnoreCase);
                query = query.Where(d => d.Male == isMale);
            }

            if (fromDob.HasValue)
                query = query.Where(d => d.Dob >= fromDob.Value);

            if (toDob.HasValue)
                query = query.Where(d => d.Dob <= toDob.Value);

            var directors = await query.OrderBy(d => d.FullName).ToListAsync();
            return Ok(directors.Select(d => d.ToDirectorDto()).ToList());
        }
    }
}

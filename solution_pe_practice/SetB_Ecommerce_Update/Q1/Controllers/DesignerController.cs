using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Mapping;
using Q1.Models;
using Q1.Models.Dtos;

namespace Q1.Controllers
{
    [ApiController]
    [Route("/api/designer")]
    public class DesignerController : ControllerBase
    {
        private readonly PE_Practice_EcommerceBContext _context;
        public DesignerController(PE_Practice_EcommerceBContext context)
        {
            _context = context;
        }
        [HttpGet("getdesigners/{nationality}/{gender}")]
        public async Task<ActionResult<List<DesignerDto>>> GetDesigners(string nationality, string gender)
        {
            bool isMale = gender.Equals("Male", StringComparison.OrdinalIgnoreCase);
            var designers = await _context.Designers
                .Where(d => d.Nationality == nationality && d.Male == isMale)
                .ToListAsync();
            return Ok(designers.Select(d => d.ToDesignerDto()).ToList());
        }
        [HttpGet("getdesigner/{id}")]
        public async Task<ActionResult<DesignerWithProductDto>> GetDesignerById(int id)
        {
            var desinger = await _context.Designers
                .Include(d => d.Products).ThenInclude(p => p.Manufacturer)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (desinger == null) return NotFound();
            return Ok(desinger.ToDesignerWithProductDto());
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateDesignerRequest request)
        {
            try
            {
                var designer = new Designer
                {
                    FullName = request.FullName,
                    Male = request.Male,
                    Dob = request.Dob,
                    Nationality = request.Nationality,
                    Description = request.Description
                };
                _context.Designers.Add(designer);
                int rowAdded = await _context.SaveChangesAsync();
                return Ok(rowAdded);
            }
            catch {
                return Conflict("There is an error while adding.");
            }
        }
    }
}

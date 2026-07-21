using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1.Mapping;
using Q1.Models;
using Q1.Models.Dtos;
using System.Globalization;

namespace Q1.Controllers
{
    [ApiController]
    [Route("api/director")]
    public class DirectorController : ControllerBase
    {
        private readonly PE_PRN_Fall22B1Context _context;
        private readonly IMapper _autoMapper;
        public DirectorController(PE_PRN_Fall22B1Context context, IMapper autoMapper)
        {
            _context = context;
            _autoMapper = autoMapper;
        }
        [HttpGet("getdirectors/{nationality}/{gender}")]
        public async Task<ActionResult<List<DirectorDto>>> GetDirectors(string nationality, string gender)
        {
            bool isMale = gender.Equals("male", StringComparison.OrdinalIgnoreCase);

            var directors = await _context.Directors
                .Where(d => d.Nationality.ToLower() == nationality.ToLower() && d.Male == isMale)
                .ToListAsync();
            return Ok(_autoMapper.Map<List<DirectorDto>>(directors));
        }
        [HttpGet("getdirector/{id}")]
        public async Task<ActionResult<DirectorWithMoviesDto>> GetDirectorById(int id)
        {
            var director = await _context.Directors
             .Include(d => d.Movies).ThenInclude(m => m.Producer)
             .FirstOrDefaultAsync(d => d.Id == id);
            if (director == null) return NotFound();
            return Ok(_autoMapper.Map<DirectorWithMoviesDto>(director));
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
                return Conflict("here is an error while adding.");
            }
        }
        //[HttpPost("create-batch")] 
        //public async Task<IActionResult> CreateBatch([FromBody] List<CreateDirectorRequest> requests)
        //{
        //    try
        //    {
        //        var directors = requests.Select(r => new Director
        //        {
        //            FullName = r.FullName,
        //            Male = r.Male,
        //            Dob = r.Dob,
        //            Nationality = r.Nationality,
        //            Description = r.Description
        //        });

        //        _context.Directors.AddRange(directors);
        //        int rowsAdded = await _context.SaveChangesAsync();
        //        return Ok(rowsAdded);
        //    }
        //    catch
        //    {
        //        return Conflict("There is an error while adding.");
        //    }
        //}

    }
}

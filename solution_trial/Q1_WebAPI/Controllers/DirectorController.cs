using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_WebAPI.Data;
using Q1_WebAPI.Models;
using Q1_WebAPI.Models.Dtos;

namespace Q1_WebAPI.Controllers;

[ApiController]
[Route("api/director")]
public class DirectorController : ControllerBase
{
    private readonly AppDbContext _context;

    public DirectorController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("getdirectors/{nationality}/{gender}")]
    public IActionResult GetDirectors(string nationality, string gender)
    {
        var isMale = gender.Equals("male", StringComparison.OrdinalIgnoreCase);

        var directors = _context.Directors
            .Where(d => d.Nationality.ToLower() == nationality.ToLower() && d.Male == isMale)
            .Select(d => DirectorDto.FromEntity(d))
            .ToList();

        return Ok(directors);
    }

    [HttpGet("getdirector/{id}")]
    public IActionResult GetDirector(int id)
    {
        var director = _context.Directors
            .Include(d => d.Movies)
                .ThenInclude(m => m.Producer)
            .FirstOrDefault(d => d.Id == id);

        if (director == null)
        {
            return NotFound();
        }

        return Ok(DirectorWithMoviesDto.FromEntity(director));
    }

    [HttpPost("create")]
    public IActionResult CreateDirector(CreateDirectorRequest request)
    {
        try
        {
            var director = new Director
            {
                FullName = request.FullName,
                Male = request.Male,
                Dob = request.Dob,
                Nationality = request.Nationality,
                Description = request.Description
            };

            _context.Directors.Add(director);
            var recordsAdded = _context.SaveChanges();

            return Ok(recordsAdded);
        }
        catch (Exception)
        {
            return new ContentResult
            {
                StatusCode = StatusCodes.Status409Conflict,
                Content = "There is an error while adding.",
                ContentType = "text/plain"
            };
        }
    }
}

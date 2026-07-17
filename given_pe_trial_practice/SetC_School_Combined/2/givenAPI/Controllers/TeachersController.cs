using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        // GET: api/Teachers/GetTeachers
        [HttpGet("GetTeachers")]
        public IActionResult GetTeachers()
        {
            var teachers = DataInitializer.Teachers;
            return Ok(teachers);
        }
    }
}

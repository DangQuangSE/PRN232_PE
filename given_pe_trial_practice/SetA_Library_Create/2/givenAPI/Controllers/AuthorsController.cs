using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        // GET: api/Authors/GetAuthors
        [HttpGet("GetAuthors")]
        public IActionResult GetAuthors()
        {
            var authors = DataInitializer.Authors;
            return Ok(authors);
        }
    }
}

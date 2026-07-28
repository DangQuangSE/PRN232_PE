using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignersController : ControllerBase
    {
        // GET: api/Designers/GetDesigners
        [HttpGet("GetDesigners")]
        public IActionResult GetDesigners()
        {
            var designers = DataInitializer.Designers;
            return Ok(designers);
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace FIXITNOWWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from FixItNow API!");
        }
    }
}

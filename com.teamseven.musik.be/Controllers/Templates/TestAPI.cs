using Microsoft.AspNetCore.Mvc;

namespace com.teamseven.musik.be.Controllers.Templates
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestAPI : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHello()
        {
            return Ok("Hello, welcome to TestAPI!");
        }
    }
}

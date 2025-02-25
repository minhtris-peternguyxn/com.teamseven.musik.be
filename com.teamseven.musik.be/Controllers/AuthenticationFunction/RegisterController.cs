using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Services.Authentication;

namespace com.teamseven.musik.be.Controllers.AuthenticationFunction
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly RegisterService _registerService;

        public RegisterController(RegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var (isSuccess, message) = await _registerService.RegisterUserAsync(registerRequest.Email, registerRequest.Password, registerRequest.Name, registerRequest.Address);
            if (isSuccess)
            {
                return Ok(new { message });
            }
            else
            {
                return BadRequest(new { message });
            }
        }
    }
}

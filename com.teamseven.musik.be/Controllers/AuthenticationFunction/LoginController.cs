using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body cannot be null." });
            }

            try
            {
                string token = await _loginService.ValidateUserAsync(request.Email, request.Password);
                return Ok(new { token, message = "Login successful." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "User not found." });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid password." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }

        [HttpPost("test")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public IActionResult Test()
        {
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
            Console.WriteLine("User Claims: " + string.Join(", ", claims));
            return Ok("Success");
        }

        [HttpPost("test-no-policy")]
        [Authorize] // Chỉ cần xác thực, không cần policy
        public IActionResult TestNoPolicy()
        {
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}");
            Console.WriteLine("User Claims: " + string.Join(", ", claims));
            return Ok("Success");
        }

    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
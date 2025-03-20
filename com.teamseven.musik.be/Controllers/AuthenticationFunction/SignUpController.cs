using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers
{
    [Route("api/signup")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly IAuthService _authService;
        private readonly ILoginService _loginService;

        public SignUpController(
            IRegisterService registerService,
            IAuthService authService,
            ILoginService loginService)
        {
            _registerService = registerService ?? throw new ArgumentNullException(nameof(registerService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _loginService = loginService ?? throw new ArgumentNullException(nameof(loginService));
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body cannot be null." });
            }

            try
            {
                await _registerService.RegisterUserAsync(request.Email, request.Password, request.Name, request.Img);

                var user = await _loginService.ValidateUserAsync(request.Email, request.Password);
                if (user == null)
                {
                    return StatusCode(500, new { message = "Failed to retrieve user after registration." });
                }

                //// Tạo token JWT
                //string token = _authService.GenerateJwtToken(user);
                return Ok("Registration successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {

                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khác (database, email service, v.v.)
                return StatusCode(500, new { message = "Internal server error.", error = ex.Message });
            }
        }
        [HttpPost("/change-role")]
        public async Task<IActionResult> ChangeRole(int userId, string roleName, string supersecretkey)
        {
            try
            {
                await _registerService.ChangeUserRole(userId, roleName, supersecretkey);
                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, new { error = e.Message });
            }
        }
    }



    // DTO cho yêu cầu đăng ký
    public class SignUpRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Img { get; set; }
    }
}
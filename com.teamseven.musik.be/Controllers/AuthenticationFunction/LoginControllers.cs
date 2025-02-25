using com.teamseven.musik.be.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Services.Authentication;

namespace com.teamseven.musik.be.Controllers.AuthenticationFunction
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public LoginController(LoginService loginService, IConfiguration configuration, TokenService tokenService)
        {
            _loginService = loginService;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var (user, statusCode) = await _loginService.ValidateUserAsync(loginRequest.Email, loginRequest.Password);

            if (statusCode == 200)
            {
                var token = _tokenService.GenerateJwtToken(user);
                return Ok(new { token });
            }
            else if (statusCode == 404)
            {
                return NotFound(new { message = "User not found." });
            }
            else if (statusCode == 401)
            {
                return Unauthorized(new { message = "Invalid password." });
            }
            else
            {
                return StatusCode(statusCode, new { message = "An unexpected error occurred." });
            }
        }



    }
}

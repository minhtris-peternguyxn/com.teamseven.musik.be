using com.teamseven.musik.be.Models.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Authentication;

namespace com.teamseven.musik.be.Controllers.AuthenticationFunction.ExternalLoginController
{
    [Route("api/login-oauth2")]
    [ApiController]
    public class ExternalLoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly RegisterService _registerService;
        private readonly IConfiguration _configuration;

        public ExternalLoginController(IUserRepository userRepository, TokenService tokenService, RegisterService registerService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _registerService = registerService;
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpGet("signin-google")]
        public IActionResult SignInWithGoogle()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, "Google");
        }

        [HttpPost("google-response")]
        [RequestSizeLimit(1_048_576)]
        public async Task<IActionResult> GoogleResponse([FromBody] GoogleTokenRequest request)
        {
            try
            {
                Console.WriteLine("Logging Google...");
                Console.WriteLine("Received credential: " + request.credential);
                // Xác thực token với Google
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _configuration["Authentication:Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(request.credential, settings);

                var user = new User
                {
                    Email = payload.Email,
                    Name = payload.Name,
                    ImgLink = payload.Picture
                };

                // Kiểm tra xem người dùng đã tồn tại trong database chưa
                var existingUser = await _userRepository.GetByEmailAsync(user.Email);
                if (existingUser == null)
                {
                    // Nếu chưa tồn tại, lưu vào database
                    await _registerService.RegisterUserAsync(user.Email, "GOOGLE_METHOD", user.Name, user.ImgLink);
                    existingUser = await _userRepository.GetByEmailAsync(user.Email);
                }

                // Tạo token
                var token = _tokenService.GenerateJwtToken(existingUser);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return BadRequest("Lỗi xác thực Google.");
            }
        }
    }
}

using com.teamseven.musik.be.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using com.teamseven.musik.be.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace com.teamseven.musik.be.Services.Authentication
{
    public class AuthService: IAuthService
    {

        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jsonToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var identity = new ClaimsIdentity(jsonToken?.Claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
        public bool IsUserInRole(string authHeader, string role)
        {
            try
            {
                var tokenString = authHeader.Substring("Bearer ".Length).Trim();
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(tokenString);
                var roleClaim = token.Claims.FirstOrDefault(c => c.Type == "role")?.Value?.ToLower();

                return roleClaim == role.ToLower();
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public IActionResult ValidateAuthorizationHeader(Microsoft.AspNetCore.Http.IHeaderDictionary headers)
        //{
        //    if (!headers.TryGetValue("Authorization", out var authHeader) ||
        //        string.IsNullOrWhiteSpace(authHeader) ||
        //        !authHeader.ToString().StartsWith("Bearer "))
        //    {
        //        return new UnauthorizedObjectResult(new { message = "Missing or invalid Authorization header." });
        //    }

        //    if (!IsUserInRole(authHeader, "admin"))
        //    {
        //        return new ForbidResult();
        //    }

        //    return null;
        //}
        //public bool IsUserInRole(string token, string role)
        //{
        //    try
        //    {
        //        var principal = GetPrincipalFromExpiredToken(token);
        //        var roleClaim = principal?.FindFirst(ClaimTypes.Role);
        //        if (roleClaim != null && roleClaim.Value == role)
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        public bool IsUserInPlan(string token, string plan)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(token);
                var planClaim = principal?.FindFirst("AccountType"); // Lấy claim 

                if (planClaim != null && planClaim.Value == plan)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GenerateJwtToken(User user)
        {
            if(user == null) throw new ArgumentNullException("user is null, cannot generate token");
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException(nameof(jwtKey), "JWT key is missing in configuration.");
            }

            // Tạo các claims với thông tin người dùng
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),            // Email
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique ID
        new Claim("userid", user.UserId.ToString()),      // User ID
        new Claim("fullname", user.Name),                            // Name
        new Claim("email", user.Email),                          // Email
        new Claim("img", user.ImgLink ?? ""),                                 // img 
        new Claim("role", user.Role ?? "User"),                  // Role (Admin, User...)
        new Claim("AccountType", user.AccountType ?? "Free")
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(300000),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

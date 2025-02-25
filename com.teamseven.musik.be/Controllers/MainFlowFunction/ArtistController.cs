using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.QueryDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/artist")]
    public class ArtistController : ControllerBase
    {
        private readonly SingerService _singService;
        private readonly IArtistRepository _repo;

        public ArtistController(IArtistRepository repo, SingerService singService)
        {
            _repo = repo;
            _singService = singService;
        }

        [HttpPost]
        public async Task<IActionResult> AddArtist([FromBody] SingerInfoDataTransfer artist)
        {
            if (artist == null)
            {
                return BadRequest("Artist cannot be null.");
            }

            if (!Request.Headers.TryGetValue("Authorization", out var authHeader) ||
                string.IsNullOrWhiteSpace(authHeader) ||
                !authHeader.ToString().StartsWith("Bearer "))
            {
                return Unauthorized(new { message = "Missing or invalid Authorization header." });
            }

            var tokenString = authHeader.ToString().Substring("Bearer ".Length).Trim();
            var tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token;

            try
            {
                token = tokenHandler.ReadJwtToken(tokenString);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Invalid token: {ex.Message}" });
            }

            var userRole = token.Claims.FirstOrDefault(c => c.Type == "role")?.Value?.ToLower();

            if (userRole != "admin")
            {
                return Forbid();
            }

            try
            {
                var artistEntity = _singService.ConvertArtist(artist);
                await _singService.CreateNewArtistAsync(artistEntity);
                return Ok(new { message = "Artist added successfully." });
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Artist conversion failed: Artist data is null." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }

        // READ: Get all artists
        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            var artists = await _repo.ListAllArtists();
            return Ok(artists);
        }

        // READ: Get artist by ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetArtistById(int id)
        {
            var artist = await _singService.GetArtistInfoAsync(id);
            if (artist == null)
                return NotFound(new { message = "Artist not found." });

            return Ok(artist);
        }
    }
}

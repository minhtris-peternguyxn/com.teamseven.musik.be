using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Services.QueryDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/track")]
    public class TrackController : ControllerBase
    {
        private readonly TrackService _trackService;

        public TrackController(TrackService trackService)
        {
            _trackService = trackService;
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> AddTrack([FromBody] TrackMusicRequest track)
        {
            if (track == null)
            {
                return BadRequest("Track information cannot be null.");
            }

            Console.WriteLine("Create track...");

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
                await _trackService.AddTrackAsync(track);
                return Ok(new { message = "Track added successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("get-tracks-by-ids")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByIds(List<int> trackIds)
        {

            if (trackIds == null || trackIds.Count == 0)
            {
                return BadRequest("ID list must not empty");
            }

            var tracks = await _trackService.GetTracksByIdsAsync(trackIds);
            return Ok(tracks);
        }
        // READ: Get all tracks
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTracks()
        {
            var tracks = await _trackService.GetAllTracksAsync();
            return Ok(tracks);
        }

        // READ: Get track by ID
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTrackById(int id)
        {
            var track = await _trackService.GetTrackByIdAsync(id);
            if (track == null)
                return NotFound("Track not found.");
            return Ok(track);
        }

        // READ: Get tracks by album
        [HttpGet("album/{albumId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByAlbum(int albumId)
        {
            var tracks = await _trackService.ListTracksByAlbumAsync(albumId);
            return Ok(tracks);
        }

        // READ: Get tracks by artist
        [HttpGet("artist/{artistId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByArtist(int artistId)
        {
            var tracks = await _trackService.ListTracksByArtistAsync(artistId);
            return Ok(tracks);
        }

        // READ: Get tracks by genre
        [HttpGet("genre/{genreId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByGenre(int genreId)
        {
            var tracks = await _trackService.ListTracksByGenreAsync(genreId);
            return Ok(tracks);
        }

        // READ: Get tracks added in the last 7 days
        [HttpGet("recent7days")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksLast7Days()
        {
            var tracks = await _trackService.ListTracksLast7DaysAsync();
            return Ok(tracks);
        }

        // UPDATE
        [HttpPut]
        public async Task<IActionResult> UpdateTrack([FromBody] TrackMusicRequest track)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            await _trackService.UpdateTrackAsync(_trackService.ConvertToTrackEntity(track), userRole);
            return Ok("Track updated successfully.");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            await _trackService.DeleteTrackAsync(id, userRole);
            return Ok("Track deleted successfully.");
        }
    }
}

using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Services.Authentication;
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
        private readonly AuthService _authService;

        public TrackController(TrackService trackService, AuthService authService)
        {
            _trackService = trackService;

        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> AddTrack([FromBody] TrackDataTransfer track)
        {
            if (track == null)
            {
                return BadRequest("Track information cannot be null.");
            }

            //Console.WriteLine("Create track...");

            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
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
        public async Task<IActionResult> UpdateTrack([FromBody] TrackDataTransfer track)
        {
            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }
            await _trackService.UpdateTrackAsync(track);
            return Ok("Track updated successfully.");
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }

            await _trackService.DeleteTrackAsync(id, "admin");
            return Ok("Track deleted successfully.");
        }

        [HttpPost("add-track-to-album")]
        public async Task<IActionResult> AddTrackToAlbum(TrackAlbum trackAlbum)
        {
            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }
            await _trackService.AddTrackToAlbumAsync(trackAlbum);
            return Ok("Add successfully");
        }

        [HttpPost("add-track-to-genre")]
        public async Task<IActionResult> AddTrackToGenre(TrackGenre trackGenre)
        {
            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }
            await _trackService.AddTrackToGenreAsync(trackGenre);
            return Ok("Add to genre successfully");
        }

        [HttpPost("remove-track-from-album")]
        public async Task<IActionResult> RemoveTrackFromAlbum(int trackId, int albumId)
        {
            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }
            await _trackService.RemoveTrackFromAlbumAsync(trackId, albumId);
            return Ok("Removed from album successfully");
        }

        [HttpPost("remove-track-from-genre")]
        public async Task<IActionResult> RemoveTrackFromGenre(int trackId, int genreId)
        {
            var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            if (authorizationResult != null)
            {
                return authorizationResult;
            }
            await _trackService.RemoveTrackFromGenreAsync(trackId, genreId);
            return Ok("Removed from genre successfully");
        }

    }
}

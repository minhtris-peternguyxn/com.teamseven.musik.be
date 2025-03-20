using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;
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
        private readonly ITrackService _trackService;
        public TrackController(ITrackService trackService)
        {
            _trackService = trackService;

        }

        // CREATE
        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddTrack([FromBody] TrackCreateRequest track)
        {
            if (track == null)
            {
                return BadRequest("Track information cannot be null.");
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
        [HttpGet("get-track-by-album/{albumId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByAlbum(int albumId)
        {
            var tracks = await _trackService.ListTracksByAlbumAsync(albumId);
            return Ok(tracks);
        }

        // READ: Get tracks by artist
        [HttpGet("get-track-by-artist/{artistId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByArtist(int artistId)
        {
            var tracks = await _trackService.ListTracksByArtistAsync(artistId);
            return Ok(tracks);
        }

        // READ: Get tracks by genre
        [HttpGet("get-track-by-genre/{genreId}")]
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
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> UpdateTrack([FromBody] TrackDataTransfer track)
        {
            await _trackService.UpdateTrackAsync(track);
            return Ok("Track updated successfully.");
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            await _trackService.DeleteTrackAsync(id, "admin");
            return Ok("Track deleted successfully.");
        }

        [HttpPost("add-track-to-album")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddTrackToAlbum(TrackAlbum trackAlbum)
        {
            await _trackService.AddTrackToAlbumAsync(trackAlbum);
            return Ok("Add successfully");
        }

        [HttpPost("add-track-to-genre")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddTrackToGenre(TrackGenre trackGenre)
        {
            await _trackService.AddTrackToGenreAsync(trackGenre);
            return Ok("Add to genre successfully");
        }

        [HttpPost("remove-track-from-album")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> RemoveTrackFromAlbum(int trackId, int albumId)
        {
            await _trackService.RemoveTrackFromAlbumAsync(trackId, albumId);
            return Ok("Removed from album successfully");
        }

        [HttpPost("remove-track-from-genre")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> RemoveTrackFromGenre(int trackId, int genreId)
        {
            await _trackService.RemoveTrackFromGenreAsync(trackId, genreId);
            return Ok("Removed from genre successfully");
        }

    }
}

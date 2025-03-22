using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (track == null) return BadRequest("Track information cannot be null.");

            try
            {
                await _trackService.AddTrackAsync(track);
                return Ok(new { message = "Track added successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("get-tracks-by-ids")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByIds([FromBody] List<int> trackIds)
        {
            if (trackIds == null || trackIds.Count == 0) return BadRequest("ID list must not be empty.");

            try
            {
                var tracks = await _trackService.GetTracksByIdsAsync(trackIds);
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // READ
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTracks()
        {
            try
            {
                var tracks = await _trackService.GetAllTracksAsync();
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTrackById(int id)
        {
            try
            {
                var track = await _trackService.GetTrackByIdAsync(id);
                return track != null ? Ok(track) : NotFound("Track not found.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("get-track-by-album/{albumId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByAlbum(int albumId)
        {
            try
            {
                var tracks = await _trackService.ListTracksByAlbumAsync(albumId);
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("get-track-by-artist/{artistId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByArtist(int artistId)
        {
            try
            {
                var tracks = await _trackService.ListTracksByArtistAsync(artistId);
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("get-track-by-genre/{genreId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksByGenre(int genreId)
        {
            try
            {
                var tracks = await _trackService.ListTracksByGenreAsync(genreId);
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("recent7days")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksLast7Days()
        {
            try
            {
                var tracks = await _trackService.ListTracksLast7DaysAsync();
                return Ok(tracks);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // UPDATE
        [HttpPut]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> UpdateTrack([FromBody] TrackUpdateRequest track)
        {
            try
            {
                await _trackService.UpdateTrackAsync(track);
                return Ok("Track updated successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> DeleteTrack(int id)
        {
            try
            {
                await _trackService.DeleteTrackAsync(id);
                return Ok("Track deleted successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("add-track-to-album")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddTrackToAlbum([FromBody] TrackAlbum trackAlbum)
        {
            try
            {
                await _trackService.AddTrackToAlbumAsync(trackAlbum);
                return Ok("Added to album successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("add-track-to-genre")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddTrackToGenre([FromBody] TrackGenre trackGenre)
        {
            try
            {
                await _trackService.AddTrackToGenreAsync(trackGenre);
                return Ok("Added to genre successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("remove-track-from-album")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> RemoveTrackFromAlbum(int trackId, int albumId)
        {
            try
            {
                await _trackService.RemoveTrackFromAlbumAsync(trackId, albumId);
                return Ok("Removed from album successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("remove-track-from-genre")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> RemoveTrackFromGenre(int trackId, int genreId)
        {
            try
            {
                await _trackService.RemoveTrackFromGenreAsync(trackId, genreId);
                return Ok("Removed from genre successfully.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        // Exception Handler Method
        private IActionResult HandleException(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
            {
                return Unauthorized(new { message = ex.Message });
            }
            else if (ex is KeyNotFoundException)
            {
                return NotFound(new { message = ex.Message });
            }
            else
            {
                return BadRequest(new { message = $"An error occurred: {ex.Message}" });
            }
        }
    }
}

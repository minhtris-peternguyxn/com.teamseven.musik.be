﻿using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly ITrackService _trackService;

        public AlbumController(IAlbumService albumService, ITrackService trackService)
        {
            _albumService = albumService;
            _trackService = trackService;
        }

        // GET: GET ALL ALBUMS
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAlbums()
        {
            var albums = await _albumService.GetAllAlbumsAsync();
            return albums != null ? Ok(albums) : NotFound(new { message = "No albums found." });
        }

        // GET: GET ALBUMS BY ID
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlbumById(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            return album != null ? Ok(album) : NotFound(new { message = "Album not found." });
        }

        // GET: GET ALBUMS BY NAME
        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlbumByName(string name)
        {
            var albums = await _albumService.GetAlbumByNameAsync(name);
            return albums != null && albums.Any() ? Ok(albums) : NotFound(new { message = "No albums found with the specified name." });
        }

        // GET: GET ARTISTS BY ALBUM ID
        [HttpGet("{id:int}/artists")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlbumArtists(int id)
        {
            var artists = await _albumService.GetArtistsInAlbumAsync(id);
            return artists != null && artists.Any() ? Ok(artists) : NotFound(new { message = "No artists found for this album." });
        }

        // GET: GET TRACKS BY ALBUM ID
        [HttpGet("{id:int}/tracks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTracksInAlbum(int id)
        {
            var tracks = await _trackService.ListTracksByAlbumAsync(id);
            return tracks != null && tracks.Any() ? Ok(tracks) : NotFound(new { message = "No tracks found for this album." });
        }

        // POST: CREATE NEW ALBUM
        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> CreateAlbum([FromBody] AlbumRequest album)
        {
            try
            {
                await _albumService.CreateNewAlbum(album);
                return Ok("Album created successfully.");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // PUT: UPDATE ALBUM
        [HttpPut("{id:int}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] Album album)
        {
            try
            {
                await _albumService.UpdateAlbumAsync(album);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the album.", error = ex.Message });
            }
        }

        // POST: ADD ARTISTS TO ALBUM
        [HttpPost("{id:int}/artists")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddArtistsToAlbum(int id, [FromBody] List<int> artists)
        {
            try
            {
                await _albumService.AddArtistToAlbum(artists, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding artists to the album.", error = ex.Message });
            }
        }

        // POST: ADD TRACKS TO ALBUM
        [HttpPost("{id:int}/tracks")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddTracksToAlbum(int id, [FromBody] List<int> tracks)
        {
            try
            {
                foreach (var track in tracks)
                {
                    await _albumService.AddTracksToAlbum(tracks, id);
                }    
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while adding tracks to the album.", error = ex.Message });
            }
        }

        // DELETE: DELETE ALBUM
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            try
            {
                await _albumService.DeleteAlbum(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the album.", error = ex.Message });
            }
        }

        // DELETE: DELETE ALBUM ARTIST
        [HttpDelete("{id:int}/artists")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> DeleteAlbumArtist(int id, [FromBody] AlbumArtist albumArtist)
        {
            try
            {
                await _albumService.DeleteAlbumArtistAsync(albumArtist);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while removing the artist from the album.", error = ex.Message });
            }
        }

        // DELETE: DELETE TRACK FROM ALBUM
        [HttpDelete("{id:int}/tracks")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> DeleteTrackFromAlbum([FromBody] TrackAlbum trackAlbum)
        {
            try
            {
                await _trackService.RemoveTrackFromAlbumAsync(trackAlbum.TrackId, trackAlbum.AlbumId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while removing the track from the album.", error = ex.Message });
            }
        }
    }
}
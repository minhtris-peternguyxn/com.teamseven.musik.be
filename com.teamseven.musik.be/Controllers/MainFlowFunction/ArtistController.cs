﻿using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;
using com.teamseven.musik.be.Services.QueryDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/artist")]
    public class ArtistController : ControllerBase
    {
        private readonly ISingerService _singService;
        private readonly IArtistRepository _repo;
        public ArtistController(IArtistRepository repo, ISingerService singService)
        {

            _repo = repo;
            _singService = singService;
        }

        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")] //staff role to access
        public async Task<IActionResult> AddArtist([FromBody] ArtistRequest artist)
        {
            if (artist == null)
            {
                return BadRequest("Artist cannot be null.");
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

        [HttpPut]
        [Authorize(Policy = "SaleStaffPolicy")] //staff role to access
        public async Task<IActionResult> ModifyArtist([FromBody] Artist artist)
        {
            if (artist == null)
            {
                return BadRequest("Artist cannot be null.");
            }

            try
            {
                await _singService.UpdateArtistAsync(artist);
                return Ok(new { message = "Artist updated successfully." });
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

        // READ: Get artist by name
        [HttpGet("name/{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListArtistByName(string name)
        {
            var artist = await _singService.GetArtistByNameAsync(name);
            if (artist == null)
                return NotFound(new { message = "Artist not found." });

            return Ok(artist);
        }
    }
}

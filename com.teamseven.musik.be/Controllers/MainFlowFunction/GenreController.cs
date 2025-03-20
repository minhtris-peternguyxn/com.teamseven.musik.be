using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;
using com.teamseven.musik.be.Services.QueryDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _service;

        public GenreController(IGenreService service)
        {
            _service = service;
        }

        // READ: Get all genres
        [HttpGet]
        public async Task<IActionResult> GetAllGenre()
        {
            var genres = await _service.ListAllGenre();
            return Ok(genres);
        }

        // POST: Add new genre
        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> AddNewGenre([FromBody] GenreDataTransfer data)
        {
            await _service.AddGenreAsync(data);
            return Ok("Add new genre successfully");
        }

        // GET: Get one genre
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOneGenre(int id)
        {
            var genre = await _service.GetOneGenre(id);
            if (genre == null)
            {
                return NotFound(new { message = "Genre not found." });
            }

            return Ok(genre);
        }

        // DELETE: Remove genre
        [HttpDelete("{id}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> RemoveGenre(int id)
        {
            //var authorizationResult = _authService.ValidateAuthorizationHeader(Request.Headers);
            //if (authorizationResult != null)
            //{
            //    return authorizationResult;
            //}

            var genre = await _service.GetOneGenre(id);
            if (genre == null)
            {
                return NotFound(new { message = "Genre not found." });
            }

            await _service.RemoveGenre(id);
            return Ok(new { message = "Genre removed successfully." });
        }
    }
}

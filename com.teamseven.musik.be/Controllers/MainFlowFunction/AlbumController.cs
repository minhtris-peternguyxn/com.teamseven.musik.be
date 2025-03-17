using com.teamseven.musik.be.Models.DataTranfers.Album;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;

        public AlbumController(IAlbumService albumService)
        {
            _albumService = albumService;
        }

        //GET: GET ALL ALBUMS
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAlbums()
        {
            var albums = await _albumService.GetAllAlbumsAsync();
            return albums != null ? Ok(albums) : NotFound(new { message = "No albums found." });
        }
        //GET: GET ALBUMS BY ID
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlbumById(int id)
        {
            var album = await _albumService.GetAlbumByIdAsync(id);
            return album != null ? Ok(album) : NotFound(new { message = "Album not found." });
        }

        //GET: GET ALBUMS BY NAME
        [HttpGet("{name:string}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlbumByName(string name)
        {
            var albums = await _albumService.GetAlbumByNameAsync(name);
            return albums != null ? Ok(albums) : NotFound(new { message = "Album not found." });
        }

        [HttpPost]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> CreateAlbum([FromBody] AlbumRequest album)
        {
            if (album == null)
            {
                return BadRequest(new { message = "Invalid album data." });
            }

            try
            {
                await _albumService.CreateNewAlbum(album);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = "SaleStaffPolicy")]
        public async Task<IActionResult> UpdateAlbum([FromBody] Album album)
        {
            try
            {
                await _albumService.UpdateAlbum(album); return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

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
                return StatusCode(500, ex.Message);
            }
        }
    }
}

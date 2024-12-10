using com.teamseven.musik.be.Entities;
using com.teamseven.musik.be.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Controllers
{
    [ApiController]
    [Route("api/albums")]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumController(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAlbums()
        {
            var albums = await _albumRepository.GetAllAlbumsAsync();
            return albums != null ? Ok(albums) : NotFound(new { message = "No albums found." });
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlbumById(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            return album != null ? Ok(album) : NotFound(new { message = "Album not found." });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlbum([FromBody] Album album)
        {
            if (album == null)
            {
                return BadRequest(new { message = "Invalid album data." });
            }

            album.CreatedDate = DateTime.Now;
            await _albumRepository.AddAlbumAsync(album);
            return CreatedAtAction(nameof(GetAlbumById), new { id = album.AlbumId }, album);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromBody] Album album)
        {
            if (id != album.AlbumId)
            {
                return BadRequest(new { message = "Album ID mismatch." });
            }

            var existingAlbum = await _albumRepository.GetAlbumByIdAsync(id);
            if (existingAlbum == null)
            {
                return NotFound(new { message = "Album not found." });
            }

            existingAlbum.AlbumName = album.AlbumName;
            existingAlbum.ReleaseDate = album.ReleaseDate;
            await _albumRepository.UpdateAlbumAsync(existingAlbum);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null)
            {
                return NotFound(new { message = "Album not found." });
            }

            await _albumRepository.DeleteAlbumAsync(id);
            return NoContent();
        }
    }
}

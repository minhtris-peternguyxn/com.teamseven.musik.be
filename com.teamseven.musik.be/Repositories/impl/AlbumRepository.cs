using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly MusikDbContext _context;
        private readonly IAlbumArtistRepository _albumArtistRepository;

        public AlbumRepository(MusikDbContext context, IAlbumArtistRepository albumArtistRepository)
        {
            _context = context;
            _albumArtistRepository = albumArtistRepository;
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            var albums = await _context.Albums.ToListAsync();

            return albums;
        }

        public async Task<Album?> GetAlbumByIdAsync(int albumId)
        {
            var album = await _context.Albums.FirstOrDefaultAsync(a => a.AlbumId == albumId);
            
            return album;
        }
        public async Task<IEnumerable<Album>?> GetAlbumByNameAsync(string name)
        {
            var albums = await _context.Albums
                .Where(a => a.AlbumName.Contains(name))
                .ToListAsync();

            return albums;
        }


        public async Task AddAlbumAsync(Album album)
        {
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlbumAsync(Album album)
        {
            _context.Albums.Update(album);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlbumAsync(int albumId)
        {
            var album = await _context.Albums.FindAsync(albumId);
            if (album != null)
            {
                _context.Albums.Remove(album);
                await _context.SaveChangesAsync();
            }
        }
    }
}
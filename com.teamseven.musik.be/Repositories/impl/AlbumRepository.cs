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
        private readonly NormalizationService _normalizationService;

        public AlbumRepository(MusikDbContext context, IAlbumArtistRepository albumArtistRepository, NormalizationService normalizationService)
        {
            _context = context;
            _albumArtistRepository = albumArtistRepository;
            _normalizationService = normalizationService;
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
            var normalizedName = _normalizationService.RemoveDiacritics(name);

            var albums = await _context.Albums
                .Where(a => a.NormalizedName.Contains(normalizedName))
                .ToListAsync();

            return albums;
        }


        public async Task AddAlbumAsync(Album album)
        {
            album.NormalizedName = _normalizationService.RemoveDiacritics(album.AlbumName);
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAlbumAsync(Album album)
        {
            album.NormalizedName = _normalizationService.RemoveDiacritics(album.AlbumName);
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
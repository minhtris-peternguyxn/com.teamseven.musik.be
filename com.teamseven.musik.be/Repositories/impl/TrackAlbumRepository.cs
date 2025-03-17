using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class TrackAlbumRepository : ITrackAlbumRepository
    {
        private readonly MusikDbContext _context;

        public TrackAlbumRepository(MusikDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddTrackAlbumAsync(TrackAlbum trackAlbum)
        {
            await _context.TrackAlbums.AddAsync(trackAlbum);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrackAlbumAsync(int trackId, int albumId)
        {
                _context.TrackAlbums.Remove(new TrackAlbum(trackId,albumId));
                await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrackAlbum>> GetAllTrackAlbumsAsync()
        {
            return await _context.TrackAlbums.ToListAsync();
        }

        public async Task Update(TrackAlbum trackAlbum)
        {
            _context.TrackAlbums.Update(trackAlbum);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TrackAlbum>> GetTrackAlbumsAsync(int trackId)
        {
            return await _context.TrackAlbums
                .Where(tt => tt.TrackId == trackId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrackAlbum>> GetTrackAlbumsByTrackIdAsync(int trackId)
        {
           return await _context.TrackAlbums.Where(a => a.TrackId == trackId).ToListAsync();
        }

        public async Task<TrackAlbum> GetTrackAlbumExistAsync(TrackAlbum trackAlbum)
        {
            return await _context.TrackAlbums
                .FirstOrDefaultAsync(a => a.TrackId == trackAlbum.TrackId && a.AlbumId == trackAlbum.AlbumId);
        }
    }
}

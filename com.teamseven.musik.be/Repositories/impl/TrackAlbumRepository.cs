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
            if (trackAlbum == null)
                throw new ArgumentNullException(nameof(trackAlbum), "TrackAlbum cannot be null.");

            // check exist condition
            var trackExists = await _context.Tracks
                .AnyAsync(t => t.TrackId == trackAlbum.TrackId);
            if (!trackExists)
                throw new KeyNotFoundException($"Track with TrackId {trackAlbum.TrackId} not found.");

            var albumExists = await _context.Albums
                .AnyAsync(a => a.AlbumId == trackAlbum.AlbumId);
            if (!albumExists)
                throw new KeyNotFoundException($"Album with AlbumId {trackAlbum.AlbumId} not found.");

            // check duplicate
            var exists = await _context.TrackAlbums
                .AnyAsync(ta => ta.TrackId == trackAlbum.TrackId && ta.AlbumId == trackAlbum.AlbumId);
            if (exists)
                return;

            await _context.TrackAlbums.AddAsync(trackAlbum);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveTrackAlbumAsync(int trackId, int albumId)
        {
            var trackAlbum = await _context.TrackAlbums
                .FirstOrDefaultAsync(ta => ta.TrackId == trackId && ta.AlbumId == albumId);
            if (trackAlbum != null)
            {
                _context.TrackAlbums.Remove(trackAlbum);
                await _context.SaveChangesAsync();
            }
        }
        //public async Task<IEnumerable<TrackAlbum>> GetAllTrackAlbumsAsync()
        //{
        //    return await _context.TrackAlbums.ToListAsync();
        //}

        //public async Task<IEnumerable<TrackAlbum>> GetTrackAlbumsAsync(int trackId)
        //{
        //    return await _context.TrackAlbums
        //        .Where(tt => tt.TrackId == trackId)
        //        .ToListAsync();
        //}

        //public async Task<IEnumerable<TrackAlbum>> GetTrackAlbumsByTrackIdAsync(int trackId)
        //{
        //   return await _context.TrackAlbums.Where(a => a.TrackId == trackId).ToListAsync();
        //}

        //public async Task<TrackAlbum> GetTrackAlbumExistAsync(TrackAlbum trackAlbum)
        //{
        //    return await _context.TrackAlbums
        //        .FirstOrDefaultAsync(a => a.TrackId == trackAlbum.TrackId && a.AlbumId == trackAlbum.AlbumId);
        //}

        public async Task<IEnumerable<Track>?> GetTrackFromAlbumAsync(int albumId)
        {
            var albumExists = await _context.Albums.AnyAsync(a => a.AlbumId == albumId);
            if (!albumExists)
                throw new KeyNotFoundException($"Album with AlbumId {albumId} not found.");

            return await _context.TrackAlbums
                .Where(ta => ta.AlbumId == albumId)
                .Join(
                    _context.Tracks,
                    ta => ta.TrackId,
                    t => t.TrackId,
                    (ta, t) => t
                )
                .ToListAsync();
        }
    }
}

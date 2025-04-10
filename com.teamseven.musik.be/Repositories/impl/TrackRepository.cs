//using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Controllers;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class TrackRepository : ITrackRepository
    {
        private readonly MusikDbContext _context;
        private readonly NormalizationService _normalizationService;

        public TrackRepository(MusikDbContext context, NormalizationService normalizationService)
        {
            _context = context;
            _normalizationService = normalizationService;
        }


        public async Task AddTrackAsync(Track track)
        {
            //add missing information
            track.CreatedDate = DateTime.Now;
            track.TotalLikes = 0;
            track.TotalViews = 0;

            track.NormalizedName = _normalizationService.RemoveDiacritics(track.TrackName);
            _context.Tracks.Add(track);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTrackAsync(int id)
        {
            var track = await _context.Tracks.FindAsync(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Track>> GetAllTracksAsync()
        {
            return await _context.Tracks.ToListAsync();
        }

        public async Task<Track> GetByIdAsync(int id) => await _context.Tracks.FindAsync(id);

        public async Task<IEnumerable<TrackAlbum>> ListTrackByAlbum(int albumId)
        {
            return await _context.TrackAlbums
                .Where(t => t.AlbumId == albumId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TrackArtist>> ListTrackByArtist(int artist)
        {
            return await _context.TrackArtists
                  .Where(t => t.ArtistId == artist)
                  .ToListAsync();
        }

        public async Task<IEnumerable<TrackGenre>> ListTrackByGenere(int genere)
        {
            return await _context.TrackGenres
                 .Where(t => t.GenreId == genere)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Track>> ListTrackLast7Days()
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);

            return await _context.Tracks
                .Where(t => t.CreatedDate >= sevenDaysAgo)
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>> ListTracksByIdsAsync(List<int> ids)
        {
            return await _context.Tracks
        .Where(t => ids.Contains(t.TrackId))
        .ToListAsync();
        }

        public async Task UpdateTrackAsync(Track track)
        {
            _context.Tracks.Update(track);
            await _context.SaveChangesAsync();
        }
    }
}

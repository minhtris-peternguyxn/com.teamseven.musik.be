using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class TrackGenreRepository : ITrackGenreRepository
    {
        private readonly MusikDbContext _context;

        public TrackGenreRepository(MusikDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddTrackGenreAsync(TrackGenre trackGenre)
        {
            if (trackGenre == null)
                throw new ArgumentNullException(nameof(trackGenre));
            var track = await _context.Tracks
                       .AnyAsync(t => t.TrackId == trackGenre.TrackId);
            var genre = await _context.Genres
                        .AnyAsync(g => g.GenreId == trackGenre.GenreId);
            if(!track) throw new KeyNotFoundException($"Track with TrackId {trackGenre.TrackId} not found.");
            if (!genre) throw new KeyNotFoundException($"Genre with GenreId {trackGenre.GenreId} not found.");

            await _context.TrackGenres.AddAsync(trackGenre);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrackGenreAsync(int trackId, int genreId)
        {
            var trackGenre = await _context.TrackGenres.FindAsync(trackId, genreId);
            if (trackGenre != null)
            {
                _context.TrackGenres.Remove(trackGenre);
                await _context.SaveChangesAsync();
            } else throw new KeyNotFoundException("Not found.");
        }

        public async Task<IEnumerable<TrackGenre>> GetAllTrackGenresAsync()
        {
            return await _context.TrackGenres.ToListAsync();
        }

        public async Task<IEnumerable<Track>> GetTracksInGenreAsync(int genreId)
        {
            // check exist
            var genreExists = await _context.Genres.AnyAsync(g => g.GenreId == genreId);
            if (!genreExists)
                throw new KeyNotFoundException($"Genre with GenreId {genreId} not found.");

            // collect tracks from trackgenre
            var tracks = await _context.TrackGenres
                .Where(tg => tg.GenreId == genreId)
                .Join(
                    _context.Tracks,
                    tg => tg.TrackId,
                    t => t.TrackId,
                    (tg, t) => t
                )
                .ToListAsync();

            return tracks;
        }
    }
}

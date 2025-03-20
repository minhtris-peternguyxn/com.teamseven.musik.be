using com.teamseven.musik.be.Models;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
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
            await _context.TrackGenres.AddAsync(trackGenre);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrackGenreAsync(int trackId, int genreId)
        {
             _context.TrackGenres.Remove(new TrackGenre(trackId, genreId));
                await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<TrackGenre>> GetAllTrackGenresAsync()
        {
            return await _context.TrackGenres.ToListAsync();
        }

        public async Task Update(TrackGenre trackGenre)
        {
            _context.TrackGenres.Update(trackGenre);
            await _context.SaveChangesAsync();
        }
    }
}

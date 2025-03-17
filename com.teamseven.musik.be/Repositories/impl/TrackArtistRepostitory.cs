using com.teamseven.musik.be.Models;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class TrackArtistRepository : ITrackArtistRepository
    {
        private readonly MusikDbContext _context;

        public TrackArtistRepository(MusikDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddTrackArtistAsync(TrackArtist trackArtist)
        {
            await _context.TrackArtists.AddAsync(trackArtist);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTrackArtistAsync(int trackId, int artistId)
        {

            var trackArtist = await _context.TrackArtists
    .FirstOrDefaultAsync(ta => ta.TrackId == trackId && ta.ArtistId == artistId);

            if (trackArtist != null)
            {
                _context.TrackArtists.Remove(trackArtist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TrackArtist>> GetAllTrackArtistsAsync()
        {
            return await _context.TrackArtists.ToListAsync();
        }

        public async Task Update(TrackArtist trackArtist)
        {
            _context.TrackArtists.Update(trackArtist);
            await _context.SaveChangesAsync();
        }
    }
}


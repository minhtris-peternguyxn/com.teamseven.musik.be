using com.teamseven.musik.be.Models;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class PlaylistTrackRepository : IPlaylistTrackRepository
    {
        private readonly MusikDbContext _context;

        public PlaylistTrackRepository(MusikDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddPlaylistTrackAsync(PlaylistTrack playlistTrack)
        {
            await _context.PlaylistTracks.AddAsync(playlistTrack);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlaylistTrack>> GetAllPlaylistTracksAsync()
        {
            return await _context.PlaylistTracks.ToListAsync();
        }

        public async Task<PlaylistTrack?> GetPlaylistTrackAsync(int playlistId, int trackId)
        {
            return await _context.PlaylistTracks
                                 .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);
        }

        public async Task RemovePlaylistTrackAsync(int playlistId, int trackId)
        {
            var playlistTrack = await GetPlaylistTrackAsync(playlistId, trackId);
            if (playlistTrack != null)
            {
                _context.PlaylistTracks.Remove(playlistTrack);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(PlaylistTrack playlistTrack)
        {
            _context.PlaylistTracks.Update(playlistTrack);
            await _context.SaveChangesAsync();
        }
    }
}

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
            if (playlistTrack == null)
                throw new ArgumentNullException(nameof(playlistTrack), "PlaylistTrack cannot be null.");

            // check conditions
            var playlistExists = await _context.Playlists
                .AnyAsync(p => p.PlaylistId == playlistTrack.PlaylistId);
            if (!playlistExists)
                throw new KeyNotFoundException($"Playlist with PlaylistId {playlistTrack.PlaylistId} not found.");

            var trackExists = await _context.Tracks
                .AnyAsync(t => t.TrackId == playlistTrack.TrackId);
            if (!trackExists)
                throw new KeyNotFoundException($"Track with TrackId {playlistTrack.TrackId} not found.");

            // check duplicate
            var exists = await _context.PlaylistTracks
                .AnyAsync(pt => pt.PlaylistId == playlistTrack.PlaylistId && pt.TrackId == playlistTrack.TrackId);
            if (exists)
                return;

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
            var playlistTrack = await _context.PlaylistTracks
                .FirstOrDefaultAsync(pt => pt.PlaylistId == playlistId && pt.TrackId == trackId);
            if (playlistTrack != null)
            {
                _context.PlaylistTracks.Remove(playlistTrack);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Track>?> GetAllTrackInPlaylist(int playlistId)
        {
            // check exist
            var playlistExists = await _context.Playlists.AnyAsync(p => p.PlaylistId == playlistId);
            if (!playlistExists)
                throw new KeyNotFoundException($"Playlist with PlaylistId {playlistId} not found.");

            return await _context.PlaylistTracks
                .Where(pt => pt.PlaylistId == playlistId)
                .Join(
                    _context.Tracks,
                    pt => pt.TrackId,
                    t => t.TrackId,
                    (pt, t) => t
                )
                .ToListAsync();
        }
    }
}

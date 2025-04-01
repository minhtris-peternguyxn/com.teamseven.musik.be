using com.teamseven.musik.be.Models;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
            if (trackArtist == null)
                throw new ArgumentNullException(nameof(trackArtist), "TrackArtist cannot be null.");

            // check track exist
            var trackExists = await _context.Tracks
                .AnyAsync(t => t.TrackId == trackArtist.TrackId);
            if (!trackExists)
                throw new KeyNotFoundException($"Track with TrackId {trackArtist.TrackId} not found.");

            // check artist exist
            var artistExists = await _context.Artists
                .AnyAsync(a => a.ArtistId == trackArtist.ArtistId);
            if (!artistExists)
                throw new KeyNotFoundException($"Artist with ArtistId {trackArtist.ArtistId} not found.");

            // check duplicate
            var exists = await _context.TrackArtists
                .AnyAsync(ta => ta.TrackId == trackArtist.TrackId && ta.ArtistId == trackArtist.ArtistId);
            if (exists)
                return;//always return code 200

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
            else throw new KeyNotFoundException($"TrackArtist with TrackId {trackId} and ArtistId {artistId} not found.");
        }

        public async Task<IEnumerable<TrackArtist>> GetAllTrackArtistsAsync()
        {
            return await _context.TrackArtists.ToListAsync();
        }

        public async Task<IEnumerable<Artist>?> FindArtistInTrack(int trackId)
        {
            //check exist
            var trackExists = await _context.Tracks.AnyAsync(t => t.TrackId == trackId);
            if (!trackExists)
                throw new KeyNotFoundException($"Track with TrackId {trackId} not found.");

            return await _context.TrackArtists
                .Where(ta => ta.TrackId == trackId)
                .Join(
                    _context.Artists,
                    ta => ta.ArtistId,
                    a => a.ArtistId,
                    (ta, a) => a
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<Track>?> FindTrackInArtist(int artistId)
        {
            var artistExists = await _context.Artists.AnyAsync(a => a.ArtistId == artistId);
            if (!artistExists)
                throw new KeyNotFoundException($"Artist with ArtistId {artistId} not found.");

            return await _context.TrackArtists
                .Where(ta => ta.ArtistId == artistId)
                .Join(
                    _context.Tracks,
                    ta => ta.TrackId,
                    t => t.TrackId,
                    (ta, t) => t
                )
                .ToListAsync();
        }

        public async Task<TrackArtist?> FindTrackArtistAsync(int trackId, int artistId)
        {
            return await _context.TrackArtists
                .FirstOrDefaultAsync(ta => ta.TrackId == trackId && ta.ArtistId == artistId);
        }

        public async Task<IEnumerable<string>?> FindArtistNameInTrack(int trackId)
        {
            var trackExists = await _context.Tracks.AnyAsync(t => t.TrackId == trackId);
            if (!trackExists)
                throw new KeyNotFoundException($"Track with TrackId {trackId} not found.");

            var artistNames = await _context.Tracks
                .Where(t => t.TrackId == trackId)
                .SelectMany(t => t.TrackArtists)
                .Select(ta => ta.Artist.ArtistName)
                .Distinct()
                .ToListAsync();

            return artistNames.Any() ? artistNames : null;
        }
    }
}


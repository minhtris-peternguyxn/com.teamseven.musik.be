using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class AlbumArtistRepository : IAlbumArtistRepository
    {
        private readonly MusikDbContext _context;

        public AlbumArtistRepository(MusikDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAlbumArtistAsync(AlbumArtist albumArtist)
        {
            if (albumArtist == null)
                throw new ArgumentNullException(nameof(albumArtist), "AlbumArtist cannot be null.");

            // check exist
            var albumExists = await _context.Albums
                .AnyAsync(a => a.AlbumId == albumArtist.AlbumId);
            if (!albumExists)
                throw new KeyNotFoundException($"Album with AlbumId {albumArtist.AlbumId} not found.");

            var artistExists = await _context.Artists
                .AnyAsync(a => a.ArtistId == albumArtist.ArtistId);
            if (!artistExists)
                throw new KeyNotFoundException($"Artist with ArtistId {albumArtist.ArtistId} not found.");

            // check duplicate
            var exists = await _context.AlbumArtists
                .AnyAsync(aa => aa.AlbumId == albumArtist.AlbumId && aa.ArtistId == albumArtist.ArtistId);
            if (exists)
                return;

            await _context.AlbumArtists.AddAsync(albumArtist);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AlbumArtist>> GetAllAlbumArtistsAsync()
        {
            return await _context.AlbumArtists.ToListAsync();
        }

        public async Task<AlbumArtist?> GetAlbumArtistAsync(int albumId, int artistId)
        {
            return await _context.AlbumArtists
                .FirstOrDefaultAsync(aa => aa.AlbumId == albumId && aa.ArtistId == artistId);
        }

        public async Task RemoveAlbumArtistAsync(int albumId, int artistId)
        {
            var albumArtist = await _context.AlbumArtists
                .FirstOrDefaultAsync(aa => aa.AlbumId == albumId && aa.ArtistId == artistId);
            if (albumArtist != null)
            {
                _context.AlbumArtists.Remove(albumArtist);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Artist>?> GetArtistsInAlbumAsync(int albumId)
        {
            // Kiểm tra Album tồn tại
            var albumExists = await _context.Albums.AnyAsync(a => a.AlbumId == albumId);
            if (!albumExists)
                throw new KeyNotFoundException($"Album with AlbumId {albumId} not found.");

            return await _context.AlbumArtists
                .Where(aa => aa.AlbumId == albumId)
                .Join(
                    _context.Artists,
                    aa => aa.ArtistId,
                    a => a.ArtistId,
                    (aa, a) => a
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>?> GetAlbumWithArtistIdAsync(int artistId)
        {
            // Kiểm tra Artist tồn tại
            var artistExists = await _context.Artists.AnyAsync(a => a.ArtistId == artistId);
            if (!artistExists)
                throw new KeyNotFoundException($"Artist with ArtistId {artistId} not found.");

            return await _context.AlbumArtists
                .Where(aa => aa.ArtistId == artistId)
                .Join(
                    _context.Albums,
                    aa => aa.AlbumId,
                    a => a.AlbumId,
                    (aa, a) => a
                )
                .ToListAsync();
        }

        public async Task<IEnumerable<int>?> GetArtistIdsInAlbumAsync(int albumId)
        {
            // Kiểm tra Album tồn tại
            var albumExists = await _context.Albums.AnyAsync(a => a.AlbumId == albumId);
            if (!albumExists)
                throw new KeyNotFoundException($"Album with AlbumId {albumId} not found.");

            // truy van list int
            return await _context.AlbumArtists
                .Where(aa => aa.AlbumId == albumId)
                .Select(aa => aa.ArtistId)
                .ToListAsync();
        }

    }
}
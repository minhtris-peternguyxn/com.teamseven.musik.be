﻿//using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MusikDbContext _context;

        public ArtistRepository(MusikDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddArtistAsync(Artist artist)
        {
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task<Artist?> GetArtistAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        public async Task<Artist?> GetArtistByNameAsync(string name)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => a.ArtistName == name);
        }

        public async Task UpdateArtistAsync(Artist artist)
        {
            _context.Artists.Update(artist);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Artist>> ListAllArtists()
        {
            return await _context.Artists.ToListAsync();
        }
        public async Task<bool> ArtistExistAsync(int id)
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Database context is not initialized.");
            }
            try
            {
                return await _context.Artists.AnyAsync(a => a.ArtistId == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking artist existence for ID {id}.", ex);
            }
        }
    }
}

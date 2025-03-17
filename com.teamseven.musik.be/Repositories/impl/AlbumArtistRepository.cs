using com.teamseven.musik.be.Models;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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
                _context.AlbumArtists.Remove(new AlbumArtist(albumId,artistId));
                await _context.SaveChangesAsync();
            
        }
        public async Task Update(AlbumArtist albumArtist)
        {
            _context.AlbumArtists.Update(albumArtist);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AlbumArtist>> GetArtistsInAlbumAsync(int albumId)
        {
           var albumArtists = await _context.AlbumArtists.Where(a => a.AlbumId == albumId).ToListAsync();
            return albumArtists;
        }

        public async Task<IEnumerable<AlbumArtist>> GetAlbumWithArtistIdAsync(int artistId)
        {
            var albumArtists = await _context.AlbumArtists.Where(a => a.ArtistId == artistId).ToListAsync();
            return albumArtists;
        }


        //public async Task<IEnumerable<int>> GetArtistsInAlbumAsync(int albumId)
        //{
        //    List<AlbumArtist> albumArtists =  await _context.AlbumArtists.Where(tt => tt.AlbumId == albumId).ToListAsync();
        //    List<int> results = new List<int>();
        //    for(int i = 0; i < albumArtists.Count; i++)
        //    {
        //        var artist = albumArtists[i];
        //        results.Add(artist.AlbumId);
        //    }    
        //    return results;

        //}
    }
}
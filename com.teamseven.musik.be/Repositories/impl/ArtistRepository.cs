//using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Net.WebSockets;
using System.Text;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly MusikDbContext _context;
        private readonly NormalizationService _normalizationService;

        public ArtistRepository(MusikDbContext context, NormalizationService normalizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _normalizationService = normalizationService ?? throw new ArgumentNullException(nameof(_normalizationService));
        }

        public async Task AddArtistAsync(Artist artist)
        {
            artist.NormalizedName =  _normalizationService.RemoveDiacritics(artist.ArtistName);
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();
        }

        public async Task<Artist?> GetArtistAsync(int id)
        {
            return await _context.Artists.FindAsync(id);
        }

        //public async Task<IEnumerable<Artist>?> GetArtistByNameAsync(string name)
        //{
        //    return await _context.Artists
        //        .Where(a => a.ArtistName.Contains(name))
        //        .ToListAsync();
        //}

        public async Task UpdateArtistAsync(Artist artist)
        {
            artist.NormalizedName = _normalizationService.RemoveDiacritics(artist.ArtistName);
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

        public async Task<string> GetArtistNameByIdAsync(int id)
        {
            var artist = await _context?.Artists.FirstOrDefaultAsync(a => a.ArtistId == id);
            return artist?.ArtistName ?? string.Empty;
        }

        public async Task<IEnumerable<Artist>?> GetArtistByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new List<Artist>();

            string normalizedInput = _normalizationService.RemoveDiacritics(name.ToLower());


            return await _context.Artists
                .Where(a => a.NormalizedName.Contains(normalizedInput))
                .ToListAsync();
        }
        public async Task<IEnumerable<Artist>?> GetArtistByNameAsyncForConditions(string name)
        {
            if (string.IsNullOrEmpty(name))
                return new List<Artist>();

            string normalizedInput = _normalizationService.RemoveDiacritics(name.ToLower());


            return await _context.Artists
                .Where(a => a.NormalizedName == normalizedInput)
                .ToListAsync();
        }

        //// Hàm loại bỏ dấu
        //private string RemoveDiacritics(string text)
        //{
        //    if (string.IsNullOrEmpty(text))
        //        return text;

        //    string normalizedString = text.Normalize(NormalizationForm.FormD);
        //    StringBuilder stringBuilder = new StringBuilder();

        //    foreach (char c in normalizedString)
        //    {
        //        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
        //            stringBuilder.Append(c);
        //    }

        //    return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        //}
    }
}

//using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class GenreRepository : IGenreRepository
    {
        private MusikDbContext _context;
        private NormalizationService _normalizationService;
        public GenreRepository(MusikDbContext context, NormalizationService normalizationService )
        {
            _context = context;
            _normalizationService = normalizationService;
        }

        public Task AddGenreAsync(Genre genre)
        {
            genre.NormalizedName = _normalizationService.RemoveDiacritics(genre.GenreName);
            _context.Genres.Add(genre);
            return _context.SaveChangesAsync();
        }

        public async Task<bool> CheckGenresExitstAsync(string name)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(gen => gen.GenreName == name);
            return genre != null;
        }

        public async Task DeleteGenreAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<Genre>> GetAllGenreAsync()
        {
            return await _context.Genres.ToListAsync();
        }


        public async Task<Genre?> GetGenreAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }


        public async Task<IEnumerable<Genre>> GetGenresByNameAsync(string name)
        {
            var normalizedName = _normalizationService.RemoveDiacritics(name);

            return await _context.Genres.Where(a => a.NormalizedName.Contains(normalizedName)).ToListAsync();
        }

        //public async Task<Genre> GetGerneByNameAsync(string name)
        //{
        //    return await _context.Genres
        //        .Where(a => a.GenreName == name)
        //        .FirstOrDefault();
        //}

        public async Task UpdateGenreAsync(Genre genre)
        {
            genre.NormalizedName = _normalizationService.RemoveDiacritics(genre.GenreName);
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }
    }
}

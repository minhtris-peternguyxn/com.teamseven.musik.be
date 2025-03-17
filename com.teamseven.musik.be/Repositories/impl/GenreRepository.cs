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
        public GenreRepository(MusikDbContext context)
        {
            _context = context;
        }

        public Task AddGenreAsync(Genre genre)
        {
            _context.Genres.Add(genre);
            return _context.SaveChangesAsync();
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




        public async Task<Genre> GetGenreAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<IEnumerable<Genre>> GetGenresByNameAsync(string name)
        {
            return await _context.Genres.Where(a => a.GenreName.Contains(name)).ToListAsync();
        }

        //public async Task<Genre> GetGerneByNameAsync(string name)
        //{
        //    return await _context.Genres
        //        .Where(a => a.GenreName == name)
        //        .FirstOrDefault();
        //}

        public async Task UpdateGenreAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }
    }
}

using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;

namespace com.teamseven.musik.be.Repositories.impl
{
    public class GenreRepository : IGenreRepository
    {
        private MusikDbContext _context;
        public Task AddGenreAsync(Genre genre)
        {
            _context.Genre.Add(genre);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Genre>> GetAllGenreAsync()
        => _context.Genre.ToList();

 
            

        public async Task<Genre> GetGenreAsync(int id)
        {
           return await _context.Genre.FindAsync(id);
        }

        public async Task<Genre> GetGerneByNameAsync(string name)
        {
            return  _context.Genre
                .Where(a => a.GenreName == name)
                .FirstOrDefault();
        }

        public async Task UpdateGenreAsync(Genre genre)
        {
            var existingGenre = _context.Genre.FirstOrDefault();
            if (existingGenre == null)
            {
                throw new KeyNotFoundException("Genre not found");
            }
            _context.Genre.Update(existingGenre);
            await _context.SaveChangesAsync();
        }
    }
}

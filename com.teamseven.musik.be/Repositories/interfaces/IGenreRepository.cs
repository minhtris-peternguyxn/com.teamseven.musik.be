using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface IGenreRepository
    {
        Task<Genre> GetGenreAsync(int id);
        Task<Genre> GetGerneByNameAsync(string name);

        Task AddGenreAsync(Genre genre);

        Task UpdateGenreAsync(Genre genre);

        Task<IEnumerable<Genre>> GetAllGenreAsync();

    }
}

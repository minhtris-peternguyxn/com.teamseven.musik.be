using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface IGenreRepository
    {
        Task<Genre?> GetGenreAsync(int id);
        Task<IEnumerable<Genre>> GetGenresByNameAsync(string name);

        Task AddGenreAsync(Genre genre);

        Task UpdateGenreAsync(Genre genre);

        Task DeleteGenreAsync(int id);

        Task<IEnumerable<Genre>> GetAllGenreAsync();

        Task<Boolean> CheckGenresExitstAsync(string name);

    }
}

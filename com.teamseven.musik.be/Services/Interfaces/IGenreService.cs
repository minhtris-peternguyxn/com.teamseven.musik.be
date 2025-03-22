using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface IGenreService
    {
        GenreRequest GetGenreEntityToDTO(Genre genre);
        Task AddGenreAsync(GenreRequest genre);
        Task<IEnumerable<Genre>> ListAllGenre();
        Task<Genre?> GetOneGenre(int id);
        Task RemoveGenre(int id);
    }
}
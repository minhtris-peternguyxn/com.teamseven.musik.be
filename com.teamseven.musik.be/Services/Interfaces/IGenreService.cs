using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface IGenreService
    {
        Genre ConvertGenreDTO(GenreDataTransfer data);
        GenreDataTransfer GetGenreEntityToDTO(Genre genre);
        Task AddGenreAsync(GenreDataTransfer genre);
        Task<IEnumerable<Genre>> ListAllGenre();
        Task<Genre> GetOneGenre(int id);
        Task RemoveGenre(int id);
    }
}
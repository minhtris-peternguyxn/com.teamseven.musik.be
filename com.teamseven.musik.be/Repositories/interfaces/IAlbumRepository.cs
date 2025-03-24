using com.teamseven.musik.be.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface IAlbumRepository
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();

        Task<IEnumerable<Album>?> GetAlbumByNameAsync(string name);

        Task<Album?> GetAlbumByIdAsync(int albumId);
        Task AddAlbumAsync(Album album);
        Task UpdateAlbumAsync(Album album);
        Task DeleteAlbumAsync(int albumId);
    }
}

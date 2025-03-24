namespace com.teamseven.musik.be.Repositories.interfaces
{
    using com.teamseven.musik.be.Models.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAlbumArtistRepository
    {
        Task AddAlbumArtistAsync(AlbumArtist albumArtist);
        Task<IEnumerable<AlbumArtist>> GetAllAlbumArtistsAsync();
        Task<AlbumArtist?> GetAlbumArtistAsync(int albumId, int artistId);
        Task<IEnumerable<Artist>?> GetArtistsInAlbumAsync(int albumId);
        Task<IEnumerable<Album>?> GetAlbumWithArtistIdAsync(int artistId);
        Task RemoveAlbumArtistAsync(int albumId, int artistId);
    }
}

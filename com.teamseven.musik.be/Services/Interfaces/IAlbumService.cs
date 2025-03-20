using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAllAlbumsAsync();
        Task<Album> GetAlbumByIdAsync(int id);

        Task<IEnumerable<Album>> GetAlbumByNameAsync(string name);

        Task<IEnumerable<AlbumArtist>> GetAlbumArtistsByAlbumIdAsync(int albumId);

        Task<IEnumerable<TrackAlbum>> GetTrackInAlbumByAlbumIdAsync(int albumId);

        Task CreateNewAlbum(AlbumRequest album);

        Task UpdateAlbum(Album album);

        Task AddArtistToAlbum(List<AlbumArtist> artists);

        Task AddTracksToAlbum(List<TrackAlbum> tracks);

        Task DeleteAlbum(int id);

        Task DeleteAlbumArtistAsync(AlbumArtist albumArtist);

        Task DeleteTrackAlbumAsync(TrackAlbum trackAlbum);
    }
}

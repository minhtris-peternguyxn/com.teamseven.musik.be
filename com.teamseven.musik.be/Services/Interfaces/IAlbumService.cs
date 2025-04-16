using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<IEnumerable<AlbumResponse>> GetAllAlbumsAsync();
        Task<AlbumResponse> GetAlbumByIdAsync(int id);

        Task<IEnumerable<AlbumResponse>?> GetAlbumByNameAsync(string name);

        Task<IEnumerable<Artist>?> GetArtistsInAlbumAsync(int albumId);

        //Task<IEnumerable<Album>?> GetAlbumsInTrack(int trackId); //method kia có trong service Track

        Task CreateNewAlbum(AlbumRequest album);

        Task UpdateAlbumAsync(Album album);

        Task AddArtistToAlbum(List<int> artistIds, int albumId);



        Task AddTracksToAlbum(List<int> trackIds, int albumId);

        Task DeleteAlbum(int id);

        Task DeleteAlbumArtistAsync(AlbumArtist albumArtist);

        //Task DeleteTrackAlbumAsync(TrackAlbum trackAlbum);
    }
}

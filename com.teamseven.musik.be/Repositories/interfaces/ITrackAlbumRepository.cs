using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface ITrackAlbumRepository
    {
        Task AddTrackAlbumAsync(TrackAlbum trackAlbum);
        Task RemoveTrackAlbumAsync(int trackId, int albumId);
        Task<IEnumerable<TrackAlbum>> GetTrackAlbumsAsync(int albumId);
        Task <TrackAlbum> GetTrackAlbumExistAsync(TrackAlbum trackAlbum);
        Task<IEnumerable<TrackAlbum>> GetTrackAlbumsByTrackIdAsync(int trackId);
        Task<IEnumerable<TrackAlbum>> GetAllTrackAlbumsAsync();
        Task Update (TrackAlbum trackAlbum);
    }
}

using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface ITrackAlbumRepository
    {
        Task AddTrackAlbumAsync(TrackAlbum trackAlbum);
        Task RemoveTrackAlbumAsync(int trackId, int albumId);
        //Task <TrackAlbum?> GetTrackAlbumExistAsync(TrackAlbum trackAlbum);      
        Task<IEnumerable<Track>?> GetTrackFromAlbumAsync(int albumId);

        Task<IEnumerable<int>?> GetTrackIdsFromAlbumAsync(int albumId);
    }
}

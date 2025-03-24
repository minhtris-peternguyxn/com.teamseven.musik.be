namespace com.teamseven.musik.be.Repositories.interfaces
{
    using com.teamseven.musik.be.Models.Entities;
    using System.Threading.Tasks;

    public interface IPlaylistTrackRepository
    {
        Task AddPlaylistTrackAsync(PlaylistTrack playlistTrack);
        Task RemovePlaylistTrackAsync(int playlistId, int trackId);
        Task<IEnumerable<Track>?> GetAllTrackInPlaylist(int playlistId);
    }

}

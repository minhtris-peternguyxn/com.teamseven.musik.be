namespace com.teamseven.musik.be.Repositories.interfaces
{
    using com.teamseven.musik.be.Models.Entities;
    using System.Threading.Tasks;

    public interface ITrackArtistRepository
    {
        Task AddTrackArtistAsync(TrackArtist trackArtist);
        Task RemoveTrackArtistAsync(int trackId, int artistId);
        Task<IEnumerable<TrackArtist>> GetAllTrackArtistsAsync();

        Task<TrackArtist?> FindTrackArtistAsync(int trackId, int artistId);

        Task<IEnumerable<Artist>?> FindArtistInTrack(int trackId);

        Task<IEnumerable<string>?> FindArtistNameInTrack(int trackId);

        Task<IEnumerable<Track>?> FindTrackInArtist(int artistId);
    }

}

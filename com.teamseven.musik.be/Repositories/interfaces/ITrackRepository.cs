using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface ITrackRepository
    {
        Task<Track> GetByIdAsync(int id);
        Task AddTrackAsync(Track track);

        Task DeleteTrackAsync(int id);

        Task UpdateTrackAsync(Track track);

        Task<IEnumerable<Track>> GetAllTracksAsync();

        Task<IEnumerable<Track>> ListTracksByIdsAsync(List<int> ids);

        Task<IEnumerable<Track>> ListTrackByArtist(int artist);

        Task<IEnumerable<Track>> ListTrackByAlbum(int album);

        Task<IEnumerable<Track>> ListTrackByGenere(int genere);

        Task<IEnumerable<Track>> ListTrackLast7Days();
    }
}

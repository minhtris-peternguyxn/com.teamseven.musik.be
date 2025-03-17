namespace com.teamseven.musik.be.Repositories.interfaces
{
    using com.teamseven.musik.be.Models.Entities;
    using System.Threading.Tasks;

    public interface ITrackGenreRepository
    {
        Task AddTrackGenreAsync(TrackGenre trackGenre);
        Task RemoveTrackGenreAsync(int trackId, int genreId);
        Task<IEnumerable<TrackGenre>> GetAllTrackGenresAsync();
        Task Update (TrackGenre trackGenre);
    }

}

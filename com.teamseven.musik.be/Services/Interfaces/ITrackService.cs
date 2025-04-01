using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface ITrackService
    {
        Task<int> AddTrackAsync(TrackCreateRequest track);

        Task<IEnumerable<BasicTrackRespone>> GetTracksByIdsAsync(List<int> ids);

        Task DeleteTrackAsync(int id);

        Task<IEnumerable<Models.DataTranfers.BasicTrackRespone>> GetAllTracksAsync();

        Task<Track> GetTrackByIdAsync(int id);

        Task<IEnumerable<Track>?> ListTracksByAlbumAsync(int albumId);

        Task<IEnumerable<Track>?> ListTracksByArtistAsync(int artistId);

        Task<IEnumerable<Track>?> ListTracksByGenreAsync(int genreId);

        Task<IEnumerable<Track>> ListTracksLast7DaysAsync();

        Task UpdateTrackAsync(TrackUpdateRequest trackUpdate);


        Task AddTrackToAlbumAsync(TrackAlbum trackAlbum);

        Task AddTrackToGenreAsync(TrackGenre trackGenre);

        Task RemoveTrackFromAlbumAsync(int trackId, int albumId);

        Task RemoveTrackFromGenreAsync(int trackId, int genreId);
    }
}
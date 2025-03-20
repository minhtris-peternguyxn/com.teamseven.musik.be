using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface ITrackService
    {
        Task AddTrackAsync(TrackCreateRequest track);

        Task<IEnumerable<Track>> GetTracksByIdsAsync(List<int> ids);

        Task DeleteTrackAsync(int id, string token);

        Task<IEnumerable<Track>> GetAllTracksAsync();

        Task<Track> GetTrackByIdAsync(int id);

        Task<IEnumerable<TrackAlbum>> ListTracksByAlbumAsync(int albumId);

        Task<IEnumerable<TrackArtist>> ListTracksByArtistAsync(int artistId);

        Task<IEnumerable<TrackGenre>> ListTracksByGenreAsync(int genreId);

        Task<IEnumerable<Track>> ListTracksLast7DaysAsync();

        Task UpdateTrackAsync(TrackDataTransfer trackUpdate);


        Task AddTrackToAlbumAsync(TrackAlbum trackAlbum);

        Task AddTrackToGenreAsync(TrackGenre trackGenre);

        Task RemoveTrackFromAlbumAsync(int trackId, int albumId);

        Task RemoveTrackFromGenreAsync(int trackId, int genreId);
    }
}
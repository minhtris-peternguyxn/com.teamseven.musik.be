using AutoMapper;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Authentication;

namespace com.teamseven.musik.be.Services.QueryDB
{
    public class TrackService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public TrackService(ITrackRepository trackRepository, TokenService tokenService, IMapper mapper)
        {
            _trackRepository = trackRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public Track ConvertToTrackEntity(TrackMusicRequest request)
        {
            return _mapper.Map<Track>(request);
        }

        public TrackMusicRequest ConvertToTrackRespone(Track track) => _mapper.Map<TrackMusicRequest>(track);



        public async Task AddTrackAsync(TrackMusicRequest track)
        {
            if (track == null)
            {
                throw new ArgumentNullException(nameof(track), "Track information is required.");
            }

            // Gán thông tin mặc định
            track.AlbumId = 1; // 1 is DEFAULT

            await _trackRepository.AddTrackAsync(ConvertToTrackEntity(track));
        }

        public async Task<IEnumerable<Track>> GetTracksByIdsAsync(List<int> ids)
        {
            await _trackRepository.ListTracksByIdsAsync(ids);
            return null;
        }


        public async Task UpdateTrackAsync(Track track, string token)
        {
            if (!_tokenService.IsUserInRole(token, "Admin"))
            {
                throw new UnauthorizedAccessException("Users are not allowed to perform this action.");
            }

            await _trackRepository.UpdateTrackAsync(track);
        }

        public async Task DeleteTrackAsync(int id, string token)
        {
            if (!_tokenService.IsUserInRole(token, "Admin"))
            {
                throw new UnauthorizedAccessException("Users are not allowed to perform this action.");
            }

            await _trackRepository.DeleteTrackAsync(id);
        }

        public async Task<IEnumerable<Track>> GetAllTracksAsync() =>
            await _trackRepository.GetAllTracksAsync();

        public async Task<Track> GetTrackByIdAsync(int id) =>
            await _trackRepository.GetByIdAsync(id);

        public async Task<IEnumerable<Track>> ListTracksByAlbumAsync(int albumId) =>
            await _trackRepository.ListTrackByAlbum(albumId);

        public async Task<IEnumerable<Track>> ListTracksByArtistAsync(int artistId) =>
            await _trackRepository.ListTrackByArtist(artistId);

        public async Task<IEnumerable<Track>> ListTracksByGenreAsync(int genreId) =>
            await _trackRepository.ListTrackByGenere(genreId);

        public async Task<IEnumerable<Track>> ListTracksLast7DaysAsync() =>
            await _trackRepository.ListTrackLast7Days();
    }
}

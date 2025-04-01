using AutoMapper;
using AutoMapper.Internal;
using com.teamseven.musik.be.Models.Contexts;
using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Repositories.impl;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace com.teamseven.musik.be.Services.QueryDB
{
    public class TrackService: ITrackService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IAuthService _tokenService;
        private readonly IMapper _mapper;
        private readonly ITrackAlbumRepository _talbumRepository;
        private readonly ITrackArtistRepository _tartistRepository;
        private readonly ITrackGenreRepository _tgenreRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly MusikDbContext _context;


        public TrackService(ITrackRepository trackRepository, IAuthService tokenService, IMapper mapper, ITrackAlbumRepository albumRepository, ITrackArtistRepository artistRepository, ITrackGenreRepository genreRepository, IAlbumRepository album, IGenreRepository genre, IArtistRepository artist, MusikDbContext context)
        {
            _trackRepository = trackRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _talbumRepository = albumRepository;
            _tartistRepository = artistRepository;
            _tgenreRepository = genreRepository;
            _albumRepository = album;
            _genreRepository = genre;
            _artistRepository = artist;
            _context = context;
        }

        public Track ConvertToTrackEntity(TrackDataTransfer request)
        {
            return _mapper.Map<Track>(request);
        }

        public TrackDataTransfer ConvertToTrackRespone(Track track) => _mapper.Map<TrackDataTransfer>(track);



        public async Task<int> AddTrackAsync(TrackCreateRequest trackRequest)
        {
            if (trackRequest == null)
                throw new ArgumentNullException(nameof(trackRequest), "Track request cannot be null.");

            await ValidateTrackInputAsync(trackRequest);

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Tạo track mới
                var trackEntity = new Track
                {
                    TrackName = trackRequest.TrackName,
                    Duration = (int)trackRequest.Duration,
                    Img = trackRequest.Img,
                    TrackBlobsLink = trackRequest.TrackBlobsLink,
                    CreatedDate = DateTime.UtcNow,
                    TotalLikes = 0,
                    TotalViews = 0
                };

                await _trackRepository.AddTrackAsync(trackEntity);
                int trackId = trackEntity.TrackId;

                // Thêm Genres
                if (trackRequest.GenresIds?.Any() == true)
                {
                    foreach (var genreId in trackRequest.GenresIds)
                    {
                        await _tgenreRepository.AddTrackGenreAsync(new TrackGenre(trackId, genreId));
                    }
                }

                // Thêm Artists
                if (trackRequest.ArtistIds?.Any() == true)
                {
                    foreach (var artistId in trackRequest.ArtistIds)
                    {
                        await _tartistRepository.AddTrackArtistAsync(new TrackArtist(trackId,artistId));
                    }
                }

                await transaction.CommitAsync(); //giống save change async nhưng hỗ trợ rollback
                return trackId;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Failed to add track and its relationships.", ex);
            }
        }

        public async Task<IEnumerable<BasicTrackRespone>> GetTracksByIdsAsync(List<int> ids)
        {
            if (ids == null || !ids.Any())
                throw new ArgumentException("List of track IDs cannot be null or empty.", nameof(ids));

            var tracks = await _trackRepository.ListTracksByIdsAsync(ids);

            var respone = new List<BasicTrackRespone>();
            foreach (var t in tracks)
            {
                respone.Add( await MapTrackToBasicTrackDataTransfer(t));
            }
            return respone;
        }


        //public async Task UpdateTrackAsync(Track track, string token)
        //{
        //    if (!_tokenService.IsUserInRole(token, "Admin"))
        //    {
        //        throw new UnauthorizedAccessException("Users are not allowed to perform this action.");
        //    }

        //    await _trackRepository.UpdateTrackAsync(track);
        //}

        public async Task DeleteTrackAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Track ID must be a positive number.", nameof(id));

            await _trackRepository.DeleteTrackAsync(id);
        }

        public async Task<IEnumerable<BasicTrackRespone>> GetAllTracksAsync()
        {
          var tracks =  await _trackRepository.GetAllTracksAsync();
            var respone = new List<BasicTrackRespone>();
            foreach (var t in tracks)
            {
                respone.Add(await MapTrackToBasicTrackDataTransfer(t));
            }
            return respone;
        }
          

        public async Task<Track> GetTrackByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Track ID must be a positive number.", nameof(id));

            var track = await _trackRepository.GetByIdAsync(id);
            if (track == null)
                throw new KeyNotFoundException($"Track with ID {id} not found.");

            return track;
        }

        public async Task<IEnumerable<Track>?> ListTracksByAlbumAsync(int albumId) =>
          await _talbumRepository.GetTrackFromAlbumAsync(albumId);


        public async Task<IEnumerable<Track>?> ListTracksByArtistAsync(int artistId) =>
            await _tartistRepository.FindTrackInArtist(artistId);

        public async Task<IEnumerable<Track>?> ListTracksByGenreAsync(int genreId) =>
            await _tgenreRepository.GetTracksInGenreAsync(genreId);

        public async Task<IEnumerable<Track>> ListTracksLast7DaysAsync() =>
            await _trackRepository.ListTrackLast7Days();

        public async Task UpdateTrackAsync(TrackUpdateRequest trackUpdate)
        {
            if (trackUpdate == null)
                throw new ArgumentNullException(nameof(trackUpdate), "Track update request cannot be null.");

            if (trackUpdate.TrackId <= 0)
                throw new ArgumentException("Track ID must be a positive number.", nameof(trackUpdate.TrackId));

            var existingTrack = await _trackRepository.GetByIdAsync(trackUpdate.TrackId);
            if (existingTrack == null)
                throw new KeyNotFoundException($"Track with ID {trackUpdate.TrackId} not found.");

            // update
            if (!string.IsNullOrWhiteSpace(trackUpdate.TrackName))
                existingTrack.TrackName = trackUpdate.TrackName;

            if (!string.IsNullOrWhiteSpace(trackUpdate.Img))
                existingTrack.Img = trackUpdate.Img;

            if (!string.IsNullOrWhiteSpace(trackUpdate.TrackBlobsLink))
                existingTrack.TrackBlobsLink = trackUpdate.TrackBlobsLink;

            if (trackUpdate.Duration.HasValue)
                existingTrack.Duration = trackUpdate.Duration.Value;

            await _trackRepository.UpdateTrackAsync(existingTrack);
        }
        public async Task AddTrackToAlbumAsync(TrackAlbum trackAlbum)
        {
            if (trackAlbum == null)
                throw new ArgumentNullException(nameof(trackAlbum), "TrackAlbum cannot be null.");

            var track = await _trackRepository.GetByIdAsync(trackAlbum.TrackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with ID {trackAlbum.TrackId} not found.");

            var album = await _albumRepository.GetAlbumByIdAsync(trackAlbum.AlbumId);
            if (album == null)
                throw new KeyNotFoundException($"Album with ID {trackAlbum.AlbumId} not found.");

            await _talbumRepository.AddTrackAlbumAsync(trackAlbum);
        }

        public async Task AddTrackToGenreAsync(TrackGenre trackGenre)
        {
            if (trackGenre == null)
                throw new ArgumentNullException(nameof(trackGenre), "TrackGenre cannot be null.");

            var track = await _trackRepository.GetByIdAsync(trackGenre.TrackId);
            if (track == null)
                throw new KeyNotFoundException($"Track with ID {trackGenre.TrackId} not found.");

            var genre = await _genreRepository.GetGenreAsync(trackGenre.GenreId);
            if (genre == null)
                throw new KeyNotFoundException($"Genre with ID {trackGenre.GenreId} not found.");

            await _tgenreRepository.AddTrackGenreAsync(trackGenre);
        }

        public async Task RemoveTrackFromAlbumAsync(int trackId, int albumId)
        {
            // Validate inputs
            if (trackId <= 0 || albumId <= 0)
            {
                throw new ArgumentException("Track Id and Album Id must be valid positive numbers.");
            }

            // Remove track from album
            await _talbumRepository.RemoveTrackAlbumAsync(trackId, albumId);
        }

        public async Task RemoveTrackFromGenreAsync(int trackId, int genreId)
        {
            // Validate inputs
            if (trackId <= 0 || genreId <= 0)
            {
                throw new ArgumentException("Track Id and Genre Id must be valid positive numbers.");
            }
            // Remove track from genre
            await _tgenreRepository.RemoveTrackGenreAsync(trackId, genreId);
        }

        //public async Task AddTrackAsync(TrackCreateRequest track)
        //{
        //    if (track == null)
        //    {
        //        throw new ArgumentNullException(nameof(track), "Track information is required.");
        //    }

        //    // Validate 
        //    ValidateTrackInput(track);

        //    // Chuyển đổi DTO sang entity và thêm vào repository
        //    var trackEntity = _mapper.Map<Track>(track);
        //    await _trackRepository.AddTrackAsync(trackEntity);
        //}


        private async Task ValidateTrackInputAsync(TrackCreateRequest trackRequest)
        {
            if (string.IsNullOrWhiteSpace(trackRequest.TrackName))
                throw new ArgumentException("Track name is required.", nameof(trackRequest.TrackName));
            if (string.IsNullOrWhiteSpace(trackRequest.TrackBlobsLink))
                throw new ArgumentException("Track blobs link is required.", nameof(trackRequest.TrackBlobsLink));
            if (string.IsNullOrWhiteSpace(trackRequest.Img))
                throw new ArgumentException("Track image link is required.", nameof(trackRequest.Img));
            if (trackRequest.Duration <= 0)
                throw new ArgumentException("Duration must be a positive value.", nameof(trackRequest.Duration));

            if (trackRequest.GenresIds?.Any() == true)
            {
                var invalidGenreIds = trackRequest.GenresIds
                    .Where(id => _genreRepository.GetGenreAsync(id).Result == null)
                    .ToList();
                if (invalidGenreIds.Any())
                    throw new ArgumentException($"Invalid Genre IDs: {string.Join(", ", invalidGenreIds)}.");
            }

            if (trackRequest.ArtistIds?.Any() == true)
            {
                var invalidArtistIds = trackRequest.ArtistIds
                    .Where(id => _artistRepository.GetArtistAsync(id).Result == null)
                    .ToList();
                if (invalidArtistIds.Any())
                    throw new ArgumentException($"Invalid Artist IDs: {string.Join(", ", invalidArtistIds)}.");
            }
        }

        // Method to map Track to BasicTrackDataTransfer
        private async Task<BasicTrackRespone> MapTrackToBasicTrackDataTransfer(Track track)
        {
            // Get list artist
            List<string> artistNames = (List<string>)await _tartistRepository.FindArtistNameInTrack(track.TrackId);

            return new BasicTrackRespone
            {
                TrackId = track.TrackId,
                TrackName = track.TrackName,
                TrackBlobsLink = track.TrackBlobsLink,
                Duration = track.Duration,
                Img = track.Img,
                Artists = artistNames
            };
        }

    }
}
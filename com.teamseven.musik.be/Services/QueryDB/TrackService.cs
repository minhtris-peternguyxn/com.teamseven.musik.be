﻿using AutoMapper;
using AutoMapper.Internal;
using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.Request;
using com.teamseven.musik.be.Repositories.impl;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Authentication;
using com.teamseven.musik.be.Services.Interfaces;

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


        public TrackService(ITrackRepository trackRepository, IAuthService tokenService, IMapper mapper, ITrackAlbumRepository albumRepository, ITrackArtistRepository artistRepository, ITrackGenreRepository genreRepository, IAlbumRepository album, IGenreRepository genre)
        {
            _trackRepository = trackRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _talbumRepository = albumRepository;
            _tartistRepository = artistRepository;
            _tgenreRepository = genreRepository;
            _albumRepository = album;
            _genreRepository = genre;
        }

        public Track ConvertToTrackEntity(TrackDataTransfer request)
        {
            return _mapper.Map<Track>(request);
        }

        public TrackDataTransfer ConvertToTrackRespone(Track track) => _mapper.Map<TrackDataTransfer>(track);



        public async Task AddTrackAsync(TrackCreateRequest trackRequest)
        {
            if (trackRequest == null)
            {
                throw new ArgumentNullException(nameof(trackRequest), "Track information is required.");
            }

            ValidateTrackInput(trackRequest);

            // check exist
            if (trackRequest.GenresIds?.Any() == true)
            {
                foreach (var genreId in trackRequest.GenresIds)
                {
                    if (await _genreRepository.GetGenreAsync(genreId) == null)
                    {
                        throw new ArgumentException($"Genre ID {genreId} not found.");
                    }
                }
            }

            if (trackRequest.ArtistIds?.Any() == true)
            {
                foreach (var artistId in trackRequest.ArtistIds)
                {
                    if (await _tartistRepository.FindTrackArtistAsync(artistId) == null)
                    {
                        throw new ArgumentException($"Artist ID {artistId} not found.");
                    }
                }
            }

            // 
            var trackEntity = new Track
            {
                TrackName = trackRequest.TrackName,
                Duration = trackRequest.Duration,
                Img = trackRequest.Img,
                TrackBlobsLink = trackRequest.TrackBlobsLink,
                CreatedDate = DateTime.UtcNow,
                TotalLikes = 0,
                TotalViews = 0
            };

            await _trackRepository.AddTrackAsync(trackEntity);

            // track genre
            if (trackRequest.GenresIds?.Any() == true)
            {
                foreach (var genreId in trackRequest.GenresIds)
                {
                    await _tgenreRepository.AddTrackGenreAsync(new TrackGenre(trackEntity.TrackId, genreId));
                }
            }

            // track artist
            if (trackRequest.ArtistIds?.Any() == true)
            {
                foreach (var artistId in trackRequest.ArtistIds)
                {
                    await _tartistRepository.AddTrackArtistAsync(new TrackArtist(trackEntity.TrackId, artistId));
                }
            }
        }


        public async Task<IEnumerable<Track>> GetTracksByIdsAsync(List<int> ids)
        {
          return  await _trackRepository.ListTracksByIdsAsync(ids);
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
            await _trackRepository.DeleteTrackAsync(id);
        }

        public async Task<IEnumerable<Track>> GetAllTracksAsync() =>
            await _trackRepository.GetAllTracksAsync();

        public async Task<Track> GetTrackByIdAsync(int id) =>
            await _trackRepository.GetByIdAsync(id);

        public async Task<IEnumerable<TrackAlbum>> ListTracksByAlbumAsync(int albumId) =>
            await _trackRepository.ListTrackByAlbum(albumId);

        public async Task<IEnumerable<TrackArtist>> ListTracksByArtistAsync(int artistId) =>
            await _trackRepository.ListTrackByArtist(artistId);

        public async Task<IEnumerable<TrackGenre>> ListTracksByGenreAsync(int genreId) =>
            await _trackRepository.ListTrackByGenere(genreId);

        public async Task<IEnumerable<Track>> ListTracksLast7DaysAsync() =>
            await _trackRepository.ListTrackLast7Days();

        public async Task UpdateTrackAsync(TrackUpdateRequest trackUpdate)
        {
            if (trackUpdate == null)
            {
                throw new ArgumentNullException(nameof(trackUpdate), "Track information is required.");
            }

            // check exist
            var existingTrack = await _trackRepository.GetByIdAsync(trackUpdate.TrackId);
            if (existingTrack == null)
            {
                throw new KeyNotFoundException($"Track with ID {trackUpdate.TrackId} not found.");
            }

            // update 
            if (!string.IsNullOrWhiteSpace(trackUpdate.TrackName))
            {
                existingTrack.TrackName = trackUpdate.TrackName;
            }

            if (!string.IsNullOrWhiteSpace(trackUpdate.Img))
            {
                existingTrack.Img = trackUpdate.Img;
            }

            if (!string.IsNullOrWhiteSpace(trackUpdate.TrackBlobsLink))
            {
                existingTrack.TrackBlobsLink = trackUpdate.TrackBlobsLink;
            }

            if (trackUpdate.Duration.HasValue)
            {
                existingTrack.Duration = trackUpdate.Duration.Value;
            }

            // save async
            await _trackRepository.UpdateTrackAsync(existingTrack);
        }

        public async Task AddTrackToAlbumAsync(TrackAlbum trackAlbum)
        {
            if (trackAlbum == null)
            {
                throw new ArgumentNullException(nameof(trackAlbum), "Track album information is required.");
            }

            //check track and album is exist

            if (_trackRepository.GetByIdAsync(trackAlbum.TrackId) == null) { throw new ArgumentException("Track Id not found"); }

            if (_albumRepository.GetAlbumByIdAsync(trackAlbum.AlbumId) == null)
            {
                throw new ArgumentException("Album Id not found");
            }

            //add track to album
            await _talbumRepository.AddTrackAlbumAsync(trackAlbum);
        }


        public async Task AddTrackToGenreAsync(TrackGenre trackGenre)
        {
            if (trackGenre == null)
            {
                throw new ArgumentNullException(nameof(trackGenre), "Track genre information is required.");
            }

            // Check if track and genre exist
            if (_trackRepository.GetByIdAsync(trackGenre.TrackId) == null)
            {
                throw new ArgumentException("Track Id not found");
            }

            if (_genreRepository.GetGenreAsync(trackGenre.GenreId) == null)
            {
                throw new ArgumentException("Genre Id not found");
            }

            // Add track to genre
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

        // Phương thức validation riêng
        private void ValidateTrackInput(TrackCreateRequest track)
        {
            if (string.IsNullOrWhiteSpace(track.TrackName))
                throw new ArgumentException("Track name is required.", nameof(track.TrackName));
            if (string.IsNullOrWhiteSpace(track.TrackBlobsLink))
                throw new ArgumentException("Track blobs link is required.", nameof(track.TrackBlobsLink));
            if (string.IsNullOrWhiteSpace(track.Img))
                throw new ArgumentException("Track img link is required.", nameof(track.Img));
            if (track.Duration <= 0)
                throw new ArgumentException("Duration must be a positive value.", nameof(track.Duration));
            // Img có thể null, nên không bắt buộc
        }
    }
}
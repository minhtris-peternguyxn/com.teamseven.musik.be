using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services
{
    public class AlbumService: IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IAlbumArtistRepository _albumArtistRepository;
        private readonly ITrackAlbumRepository _trackAlbumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly ITrackRepository _trackRepository;
        public AlbumService(IAlbumRepository albumRepository, IAlbumArtistRepository albumArtistRepository, ITrackAlbumRepository trackAlbumRepository, IArtistRepository artistRepository, ITrackRepository trackRepository)
        {
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
            _albumArtistRepository = albumArtistRepository ?? throw new ArgumentNullException(nameof(albumArtistRepository));
            _trackAlbumRepository = trackAlbumRepository ?? throw new ArgumentNullException(nameof(trackAlbumRepository));
            _artistRepository = artistRepository ?? throw new ArgumentNullException(nameof(trackAlbumRepository));
            _trackRepository = trackRepository ?? throw new ArgumentNullException(nameof(trackRepository));
        }

        public async Task<IEnumerable<AlbumResponse>> GetAllAlbumsAsync()
        {
            var albums = await _albumRepository.GetAllAlbumsAsync();
            List<AlbumResponse> albumResponses = new List<AlbumResponse>();

            foreach (var album in albums)
            {
                var response = await ConvertToResponse(album);
                albumResponses.Add(response);
            }

            return albumResponses;
        }


        public async Task<AlbumResponse> GetAlbumByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Album ID must be greater than zero.");

            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null)
                throw new KeyNotFoundException($"Album with ID {id} not found.");

            return await ConvertToResponse(album); // <-- sửa ở đây
        }

        public async Task<IEnumerable<AlbumResponse>?> GetAlbumByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Album name cannot be null or empty.", nameof(name));

            var albums = await _albumRepository.GetAlbumByNameAsync(name);

            var result = new List<AlbumResponse>();
            foreach (var album in albums)
            {
                result.Add(await ConvertToResponse(album));
            }

            return result;
        }


     

        public async Task UpdateAlbumAsync(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album), "Album data cannot be null.");

            if (album.AlbumId <= 0)
                throw new ArgumentOutOfRangeException(nameof(album), "Album ID must be greater than zero.");


            var existingAlbum = await _albumRepository.GetAlbumByIdAsync(album.AlbumId);
            if (existingAlbum == null)
                throw new KeyNotFoundException($"Album with ID {album.AlbumId} not found.");

            existingAlbum.AlbumName = album.AlbumName;
            existingAlbum.ReleaseDate = album.ReleaseDate;
            existingAlbum.Img = album.Img;

            await _albumRepository.UpdateAlbumAsync(existingAlbum);
        }

        // Xóa album
        public async Task DeleteAlbumAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Album ID must be greater than zero.");

            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null)
                throw new KeyNotFoundException($"Album with ID {id} not found.");

            await _albumRepository.DeleteAlbumAsync(id);
        }

        //public async Task<IEnumerable<AlbumArtist>> GetAlbumArtistsByAlbumIdAsync(int albumId)
        //{
        //    if (albumId < 0) throw new ArgumentException("Id is invalid");
        //    return await _albumArtistRepository.GetArtistsInAlbumAsync(albumId);

        //}

        //public async Task<IEnumerable<TrackAlbum>> GetTrackInAlbumByAlbumIdAsync(int albumId)
        //{
        //    if (albumId < 0) throw new ArgumentException("Id is invalid");
        //    return await _trackAlbumRepository.GetTrackAlbumsAsync(albumId);
        //}

        public async Task CreateNewAlbum(AlbumRequest album)
        {
            // Kiem tra dau vao
            if (album == null) throw new ArgumentNullException(nameof(album), "Album data cannot be null.");
            if (string.IsNullOrWhiteSpace(album.AlbumName)) throw new ArgumentException("Album name is required.", nameof(album.AlbumName));
            if (album.ArtistIds == null)
            {
                throw new InvalidOperationException("ArtistIds is null for the album.");
            }

            //// Kiem tra repository truoc khi su dung
            //if (_artistRepository == null) throw new InvalidOperationException("Artist repository is not initialized.");
            //if (_albumRepository == null) throw new InvalidOperationException("Album repository is not initialized.");
            //if (_albumArtistRepository == null) throw new InvalidOperationException("Album artist repository is not initialized.");

            // Kiem tra tung artist truoc khi luu
            foreach (var artist in album.ArtistIds)
            {
                var exists = await _artistRepository.ArtistExistAsync(artist);
                if (!exists) throw new KeyNotFoundException($"Artist with ID {artist} not found.");
            }

            foreach (var track in album.TrackIds)
            {
                var exists = await _trackRepository.GetByIdAsync(track);
                if(exists == null) throw new KeyNotFoundException($"Track with ID {track} not found.");
            }

            // Tao va luu album
            var newAlbum = new Album
            {
                AlbumName = album.AlbumName,
                CreatedDate = DateTime.Now,
                ReleaseDate = album.ReleaseDate,
                Img = album.Img
            };
            await _albumRepository.AddAlbumAsync(newAlbum);

            // Kiem tra albumId sau khi luu
            int albumId = newAlbum.AlbumId;
            if (albumId <= 0) throw new InvalidOperationException("Failed to get valid AlbumId after saving.");

            // Luu quan he many-to-many
            foreach (var artistId in album.ArtistIds)
            {
                await _albumArtistRepository.AddAlbumArtistAsync(new AlbumArtist
                {
                    AlbumId = albumId,
                    ArtistId = artistId
                });
            }

            foreach (var trackId in album.TrackIds)
            {
                await _trackAlbumRepository.AddTrackAlbumAsync(new TrackAlbum
                {
                    AlbumId = albumId,
                    TrackId = trackId
                });
            }
        }
        public async Task AddArtistToAlbum(List<int> artistIds, int albumId)
        {
            if (artistIds == null || !artistIds.Any())
                throw new ArgumentException("Artist list cannot be null or empty.", nameof(artistIds));

            //if (_albumRepository.GetAlbumByIdAsync(albumId) == null)
            //    throw new InvalidOperationException($"Not Found Album ID ({albumId})");

            //// Validate toàn bộ dữ liệu trước khi lưu
            //foreach (var artist in artistIds)
            //{
            //    if (albumId <= 0 || artist <= 0)
            //        throw new ArgumentException($"Invalid Album ID ({albumId}) or Artist ID ({artist}).");

            //    if(_artistRepository.)

                
            //    //var exists = await _albumArtistRepository.GetAlbumArtistAsync(artist.AlbumId, artist.ArtistId);
            //    //if (exists != null)
            //    //    throw new ArgumentException($"Artist ID {artist.ArtistId} already linked to Album ID {artist.AlbumId}.");
            //}


            //KO CAN VALIDATE VI REPOSITORY VALIDATE ROI

            // Thêm dữ liệu và lưu
            foreach (var artist in artistIds)
            {
                await _albumArtistRepository.AddAlbumArtistAsync(new AlbumArtist(albumId,artist));
            }    
            
        }
        //public async Task AddTracksToAlbum(List<TrackAlbum> tracks)
        //{
        //    if (tracks == null || !tracks.Any())
        //        throw new ArgumentException("Track list cannot be null or empty.", nameof(tracks));

        //    // Validate toàn bộ dữ liệu trước khi lưu
        //    foreach (var track in tracks)
        //    {
        //        if (track.AlbumId <= 0 || track.TrackId <= 0)
        //            throw new ArgumentException($"Invalid Album ID ({track.AlbumId}) or Track ID ({track.TrackId}).");

        //        var exists = await _trackAlbumRepository.GetTrackAlbumExistAsync(track);
        //        if (exists != null)
        //            throw new ArgumentException($"Track ID {track.TrackId} already linked to Album ID {track.AlbumId}.");
        //    }

        //    // Thêm dữ liệu và lưu
        //    await _trackAlbumRepository.AddRangeAsync(tracks);
        //    await _trackAlbumRepository.SaveChangesAsync();
        //}


        public async Task DeleteAlbumArtistAsync(AlbumArtist albumArtist)
        {
            if (albumArtist == null)
                throw new ArgumentNullException(nameof(albumArtist), "AlbumArtist data cannot be null.");

            if (albumArtist.AlbumId <= 0 || albumArtist.ArtistId <= 0)
                throw new ArgumentException("Album ID and Artist ID must be positive numbers.");

            await _albumArtistRepository.RemoveAlbumArtistAsync(albumArtist.AlbumId, albumArtist.ArtistId);
        }

        //public async Task DeleteTrackAlbumAsync(TrackAlbum trackAlbum)
        //{
        //   if(trackAlbum == null) throw new ArgumentNullException(nameof(trackAlbum));
        //    await _trackAlbumRepository.RemoveTrackAlbumAsync(trackAlbum.TrackId, trackAlbum.AlbumId);
        //}

        public async Task DeleteAlbum(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Album ID must be greater than zero.");
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null) throw new KeyNotFoundException($"Album with ID {id} not found.");

            await _albumRepository.DeleteAlbumAsync(id);
        }

        public async Task UpdateAlbum(Album album)
        {
            if (album == null)
                throw new ArgumentNullException(nameof(album), "Album data cannot be null.");

            if (album.AlbumId <= 0)
                throw new ArgumentOutOfRangeException(nameof(album.AlbumId), "Album ID must be greater than zero.");

            var existingAlbum = await _albumRepository.GetAlbumByIdAsync(album.AlbumId);
            if (existingAlbum == null)
                throw new KeyNotFoundException($"Album with ID {album.AlbumId} not found.");

            await _albumRepository.UpdateAlbumAsync(album);
        }

        public async Task<IEnumerable<Artist>?> GetArtistsInAlbumAsync(int albumId)
        {
            return await _albumArtistRepository.GetArtistsInAlbumAsync(albumId);
        }

        public async Task AddTracksToAlbum(List<int> trackIds, int albumId)
        {
            if (albumId < 1 ||  trackIds.Count == 0 || !trackIds.Any()){
                throw new ArgumentException(" One information is missing");
            }
            // Validate toàn bộ dữ liệu trước khi lưu
            foreach (var track in trackIds)
            {
                if (track < 1)
                    throw new ArgumentException($"Invalid Track ID ({track}).");

                //var exists =  _trackRepository.GetByIdAsync(track);
                //if (exists == null)
                //    throw new ArgumentException($"Album ID not found.");
                //VALIDATE O REPOSITORY ROI
            }
            //convert to TrackAlbum element
            foreach(var track in trackIds)
            {
              TrackAlbum trackAlbum = new TrackAlbum(track,albumId);              
              await  _trackAlbumRepository.AddTrackAlbumAsync(trackAlbum); //da xu li duplicate nen ko sao

            }    

        }

        //public async Task<IEnumerable<Album>?> GetAlbumsInTrack(int trackId)
        //{
        //    return await _trackAlbumRepository.Get
        //} NONEED

        private async Task<AlbumResponse> ConvertToResponse(Album album)
        {
            var trackIds = await _trackAlbumRepository.GetTrackIdsFromAlbumAsync(album.AlbumId);
            var artistIds = await _albumArtistRepository.GetArtistIdsInAlbumAsync(album.AlbumId);

            AlbumResponse albumResponse = new AlbumResponse
            {
                Id = album.AlbumId,
                AlbumName = album.AlbumName,
                Img = album.Img,
                ReleaseDate = album.ReleaseDate,
                TrackIds = trackIds?.ToList() ?? new List<int>(),     // Handle null 
                ArtistIds = artistIds?.ToList() ?? new List<int>()    
            };

            return albumResponse;
        }

    }
}
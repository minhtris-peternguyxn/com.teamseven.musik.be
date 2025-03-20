using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services
{
    public class AlbumService: IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IAlbumArtistRepository _albumArtistRepository;
        private readonly ITrackAlbumRepository _trackAlbumRepository;
        public AlbumService(IAlbumRepository albumRepository, IAlbumArtistRepository albumArtistRepository, ITrackAlbumRepository trackAlbumRepository)
        {
            _albumRepository = albumRepository ?? throw new ArgumentNullException(nameof(albumRepository));
            _albumArtistRepository = albumArtistRepository ?? throw new ArgumentNullException(nameof(albumArtistRepository));
            _trackAlbumRepository = trackAlbumRepository ?? throw new ArgumentNullException(nameof(trackAlbumRepository));
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _albumRepository.GetAllAlbumsAsync();
        }

        public async Task<Album> GetAlbumByIdAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Album ID must be greater than zero.");
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            return album ?? throw new KeyNotFoundException($"Album with ID {id} not found.");
        }

        public async Task<IEnumerable<Album>> GetAlbumByNameAsync(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Album name cannot be null or empty.", nameof(name));
            return await _albumRepository.GetAlbumByNameAsync(name);
        }

        public async Task UpdateAlbumAsync(int id, Album album)
        {
            if (id != album.AlbumId)
            {
                throw new ArgumentException("Album ID mismatch.");
            }

            var existingAlbum = await _albumRepository.GetAlbumByIdAsync(id);
            if (existingAlbum == null)
            {
                throw new KeyNotFoundException("Album not found.");
            }

            existingAlbum.AlbumName = album.AlbumName;
            existingAlbum.ReleaseDate = album.ReleaseDate;
            existingAlbum.Img = album.Img;


            await _albumRepository.UpdateAlbumAsync(existingAlbum);
        }

        // Xóa album
        public async Task DeleteAlbumAsync(int id)
        {
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null)
            {
                throw new KeyNotFoundException("Album not found.");
            }

            await _albumRepository.DeleteAlbumAsync(id);
        }

        public async Task<IEnumerable<AlbumArtist>> GetAlbumArtistsByAlbumIdAsync(int albumId)
        {
            if (albumId < 0) throw new ArgumentException("Id is invalid");
            return await _albumArtistRepository.GetArtistsInAlbumAsync(albumId);

        }

        public async Task<IEnumerable<TrackAlbum>> GetTrackInAlbumByAlbumIdAsync(int albumId)
        {
            if (albumId < 0) throw new ArgumentException("Id is invalid");
            return await _trackAlbumRepository.GetTrackAlbumsAsync(albumId);
        }

        public async Task CreateNewAlbum(AlbumRequest album)
        {
            if (album == null) throw new ArgumentNullException(nameof(album), "Album data cannot be null.");
            if (string.IsNullOrWhiteSpace(album.AlbumName)) throw new ArgumentException("Album name is required.", nameof(album.AlbumName));

            var newAlbum = new Album
            {
                AlbumName = album.AlbumName,
                CreatedDate = DateTime.Now,
                ReleaseDate = album.ReleaseDate,
                Img = album.Img
            };

            await _albumRepository.AddAlbumAsync(newAlbum);
        }
        public async Task AddArtistToAlbum(List<AlbumArtist> artists)
        {
            if (artists == null || !artists.Any()) throw new ArgumentNullException(nameof(artists), "Artist list cannot be null or empty.");

            foreach (var artist in artists)
            {
                if (await _albumArtistRepository.GetAlbumArtistAsync(artist.AlbumId, artist.ArtistId) == null)
                {
                    await _albumArtistRepository.AddAlbumArtistAsync(artist);
                }
            }
        }

        public async Task AddTracksToAlbum(List<TrackAlbum> tracks)
        {
            if (tracks == null) throw new ArgumentNullException(nameof(tracks));
            for(int i = 0; i < tracks.Count; i++)
            {
                if(await _trackAlbumRepository.GetTrackAlbumExistAsync(tracks[i]) == null)
                {
                    await _trackAlbumRepository.AddTrackAlbumAsync(tracks[i]);
                }
            }
        }

        public async Task DeleteAlbumArtistAsync(AlbumArtist albumArtist)
        {
            if(_albumArtistRepository == null) throw new ArgumentNullException(nameof(albumArtist));
            await _albumArtistRepository.RemoveAlbumArtistAsync(albumArtist.AlbumId, albumArtist.ArtistId);
        }

        public async Task DeleteTrackAlbumAsync(TrackAlbum trackAlbum)
        {
           if(trackAlbum == null) throw new ArgumentNullException(nameof(trackAlbum));
            await _trackAlbumRepository.RemoveTrackAlbumAsync(trackAlbum.TrackId, trackAlbum.AlbumId);
        }

        public async Task DeleteAlbum(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Album ID must be greater than zero.");
            var album = await _albumRepository.GetAlbumByIdAsync(id);
            if (album == null) throw new KeyNotFoundException($"Album with ID {id} not found.");

            await _albumRepository.DeleteAlbumAsync(id);
        }

        public async Task UpdateAlbum(Album album)
        {
            if (album == null) throw new ArgumentNullException(nameof(album), "Album data cannot be null.");
            if (album.AlbumId <= 0) throw new ArgumentOutOfRangeException(nameof(album.AlbumId), "Album ID must be greater than zero.");

            var existingAlbum = await _albumRepository.GetAlbumByIdAsync(album.AlbumId);
            if (existingAlbum == null) throw new KeyNotFoundException($"Album with ID {album.AlbumId} not found.");

            await _albumRepository.UpdateAlbumAsync(album);
        }
    }
}
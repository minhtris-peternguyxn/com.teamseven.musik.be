using com.teamseven.musik.be.Models.DataTranfers.Album;
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
            _albumRepository = albumRepository;
            _albumArtistRepository = albumArtistRepository;
            _trackAlbumRepository = trackAlbumRepository;      
        }

        public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
        {
            return await _albumRepository.GetAllAlbumsAsync();
        }

        // Lấy album theo ID
        public async Task<Album> GetAlbumByIdAsync(int id)
        {
            if (id < 0) throw new ArgumentException("ID is invalid");
            return await _albumRepository.GetAlbumByIdAsync(id);
        }

        public async Task<IEnumerable<Album>> GetAlbumByNameAsync(string name)
        {
            if(name  == null) throw new ArgumentNullException("name");
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
            if (album == null)
            {
                throw new ArgumentNullException(nameof(album), "Invalid album data.");
            }
            Album input = new Album();
            input.AlbumName = album.AlbumName;
            input.CreatedDate = DateTime.Now;
            input.ReleaseDate = album.ReleaseDate;
            input.Img = album.Img;

            await _albumRepository.AddAlbumAsync(input);
        }

        public async Task AddArtistToAlbum(List<AlbumArtist> artists)
        {
            if(artists == null) throw new ArgumentNullException(nameof(artists));   
            for (int i = 0; i < artists.Count; i++)
            {
                //check exist before add
               if( await _albumArtistRepository.GetAlbumArtistAsync(artists[i].AlbumId, artists[i].ArtistId) == null)
                {
                   await _albumArtistRepository.AddAlbumArtistAsync(artists[i]);
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
            if(id < 0) throw new ArgumentOutOfRangeException("Invalid ID");
            //check album exists
            if(await _albumRepository.GetAlbumByIdAsync(id) == null)
            {
                throw new KeyNotFoundException($"Album with ID {id} was not found.");
            }
            else
            {
                await _albumRepository.DeleteAlbumAsync(id);
            }
        }

        public async Task UpdateAlbum(Album album)
        {
            if (album == null) throw new ArgumentNullException("Album is Empty");
            if (await GetAlbumByIdAsync(album.AlbumId) == null) { throw new ArgumentNullException(nameof(album)); }
            await _albumRepository.UpdateAlbumAsync(album);
        }
    }
}
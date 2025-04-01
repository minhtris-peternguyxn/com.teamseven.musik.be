using AutoMapper;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Services.Interfaces;
using com.teamseven.musik.be.Repositories.impl;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Models.DataTranfers;
using Microsoft.EntityFrameworkCore;

namespace com.teamseven.musik.be.Services.QueryDB
{
    public class SingerService: ISingerService
    {
        private readonly IArtistRepository _repo;
        private readonly IMapper _mapper;

        public SingerService(IArtistRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Artist ConvertArtist(ArtistRequest info)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info), "ArtistRequest cannot be null.");
            }

            var artist = new Artist
            {
                ArtistName = info.ArtistName,
                VerifiedArtist = true,
                Img = info.Img,
                SubscribeNumber = 0, 
                CreatedDate = DateTime.UtcNow,
            };

            return artist;
        }
        public SingerInfoDataTransfer ConvertArtistToDTO(Artist artist)
        {
            return _mapper.Map<SingerInfoDataTransfer>(artist);
        }

        public async Task<Artist> GetArtistInfoAsync(int id)
        {
            return await _repo.GetArtistAsync(id)
                   ?? throw new KeyNotFoundException($"Artist with ID {id} not found.");
        }


        public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
        {
            return await _repo.ListAllArtists();
        }

        public async Task CreateNewArtistAsync(Artist artistEntity)
        {
            if (artistEntity == null)
            {
                throw new ArgumentNullException(nameof(artistEntity), "Artist entity cannot be null.");
            }

            var existingArtist = await _repo.GetArtistByNameAsync(artistEntity.ArtistName);
            if (existingArtist != null)
            {
                throw new ArgumentException($"Artist '{artistEntity.ArtistName}' already exists.");
            }

            artistEntity.CreatedDate = DateTime.UtcNow;
            artistEntity.VerifiedArtist = true;
            artistEntity.SubscribeNumber = 0;

            Console.WriteLine("Connecting repository component...");
            await _repo.AddArtistAsync(artistEntity);
        }

        public async Task UpdateArtistAsync(Artist artistEntity)
        {
            if (artistEntity == null)
            {
                throw new ArgumentNullException(nameof(artistEntity), "Artist entity cannot be null.");
            }

            if (artistEntity.ArtistId <= 0)
            {
                throw new ArgumentException("Artist ID must be a positive integer.", nameof(artistEntity.ArtistId));
            }

            if (!string.IsNullOrWhiteSpace(artistEntity.ArtistName) && artistEntity.ArtistName.Length > 255)
            {
                throw new ArgumentException("Artist name cannot exceed 255 characters.", nameof(artistEntity.ArtistName));
            }

            if (artistEntity.SubscribeNumber < 0)
            {
                throw new ArgumentException("Subscribe number cannot be negative.", nameof(artistEntity.SubscribeNumber));
            }

            if (!string.IsNullOrWhiteSpace(artistEntity.Img) && artistEntity.Img.Length > 255)
            {
                throw new ArgumentException("Image URL cannot exceed 255 characters.", nameof(artistEntity.Img));
            }

            var existingArtist = await _repo.GetArtistAsync(artistEntity.ArtistId);
            if (existingArtist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {artistEntity.ArtistId} not found.");
            }

            // update
            existingArtist.ArtistName = artistEntity.ArtistName ?? existingArtist.ArtistName; 
            existingArtist.Img = artistEntity.Img ?? existingArtist.Img;
            existingArtist.VerifiedArtist = artistEntity.VerifiedArtist; 
            existingArtist.SubscribeNumber = artistEntity.SubscribeNumber;

            Console.WriteLine("Updating artist in repository...");
            await _repo.UpdateArtistAsync(existingArtist);
        }
        public async Task DeleteArtistAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Artist ID must be a positive integer.", nameof(id));
            }

            var existingArtist = await _repo.GetArtistAsync(id);
            if (existingArtist == null)
            {
                throw new KeyNotFoundException($"Artist with ID {id} not found.");
            }

            // Ghi log và gọi repository để xóa
            Console.WriteLine($"Deleting artist with ID {id} from repository...");
            
        }

        public async Task<IEnumerable<Artist?>> GetArtistByNameAsync(string name)
        {
            return await _repo.GetArtistByNameAsync(name);
        }
    }
}

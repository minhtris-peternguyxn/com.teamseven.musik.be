using AutoMapper;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using com.teamseven.musik.be.Repositories.impl;
using com.teamseven.musik.be.Repositories.interfaces;

namespace com.teamseven.musik.be.Services.QueryDB
{
    public class SingerService
    {
        private readonly IArtistRepository _repo;
        private readonly IMapper _mapper;

        public SingerService(IArtistRepository repo, IMapper mapper)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Artist ConvertArtist(SingerInfoDataTransfer info)
        {
            return _mapper.Map<Artist>(info);
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
            artistEntity.VerifiedArtist = 1;
            artistEntity.SubscribeNumber = 0;

            Console.WriteLine("Connecting repository component...");
            await _repo.AddArtistAsync(artistEntity);
        }

    }
}

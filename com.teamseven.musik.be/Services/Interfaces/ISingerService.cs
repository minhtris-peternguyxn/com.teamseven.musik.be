using com.teamseven.musik.be.Models.DataTranfers;
using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface ISingerService
    {
        Artist ConvertArtist(ArtistRequest info);


        SingerInfoDataTransfer ConvertArtistToDTO(Artist artist);

        Task<Artist> GetArtistInfoAsync(int id);

        Task<IEnumerable<Artist>?> GetArtistByNameAsync(string name);

        Task<IEnumerable<Artist>> GetAllArtistsAsync();

        Task CreateNewArtistAsync(Artist artistEntity);

        Task UpdateArtistAsync(Artist artistEntity);

        Task DeleteArtistAsync(int id);

        //Task<string> GetArtistNameAsync(int id);

        //Task<bool> GetArtistExist(int id);
    }
}
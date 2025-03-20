using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Models.RequestDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Interfaces
{
    public interface ISingerService
    {
        Artist ConvertArtist(SingerInfoDataTransfer info);


        SingerInfoDataTransfer ConvertArtistToDTO(Artist artist);

        Task<Artist> GetArtistInfoAsync(int id);

        Task<IEnumerable<Artist>> GetAllArtistsAsync();

        Task CreateNewArtistAsync(Artist artistEntity);
    }
}
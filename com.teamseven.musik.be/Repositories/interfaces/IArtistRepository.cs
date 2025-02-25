using com.teamseven.musik.be.Models.Entities;

namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface IArtistRepository
    {
        Task<Artist> GetArtistAsync(int id);
        Task<Artist> GetArtistByNameAsync(string name);

        Task AddArtistAsync(Artist artist);

        Task UpdateArtistAsync(Artist artist);

        Task<IEnumerable<Artist>> ListAllArtists();

    }
}

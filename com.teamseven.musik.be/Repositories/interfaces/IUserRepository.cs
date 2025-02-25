using com.teamseven.musik.be.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;



namespace com.teamseven.musik.be.Repositories.interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);

        Task AddUserAsync(User user);

        Task DeleteUserAsync(int id);

        Task UpdateUserAsync(User user);
    }
}

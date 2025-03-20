using com.teamseven.musik.be.Models.Entities;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Authentication
{
    public interface ILoginService
    {
        Task<string> ValidateUserAsync(string email, string password);
    }
}
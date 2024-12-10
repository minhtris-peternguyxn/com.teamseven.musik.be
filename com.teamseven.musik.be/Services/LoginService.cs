using com.teamseven.musik.be.Entities;
using com.teamseven.musik.be.Repositories;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services
{
    public class LoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordEncryptionService _passwordEncryptionService;

        public LoginService(IUserRepository userRepository, PasswordEncryptionService passwordEncryptionService)
        {
            _userRepository = userRepository;
            _passwordEncryptionService = passwordEncryptionService;
        }

        public async Task<(User user, int statusCode)> ValidateUserAsync(string email, string password)
        {
            User user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return (null, 404); // Not Found
            }

            if (_passwordEncryptionService.VerifyPassword(password, user.Password))
            {
                return (user, 200); // OK
            }
            else
            {
                return (null, 401); // Unauthorized
            }
        }
    }
}

using com.teamseven.musik.be.Entities;
using com.teamseven.musik.be.Repositories;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services
{
    public class RegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordEncryptionService _passwordEncryptionService;
        private readonly EmailService _emailService;

        public RegisterService(IUserRepository userRepository, PasswordEncryptionService passwordEncryptionService, EmailService emailService) { _userRepository = userRepository; _passwordEncryptionService = passwordEncryptionService; _emailService = emailService; }
        public async Task<(bool isSuccess, string message)> RegisterUserAsync(string email, string password, string name, string address)
        {
            // Kiểm tra tính duy nhất của email
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
            {
                return (false, "Email already in use.");
            }

            // Kiểm tra các trường bắt buộc không được để trống
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(address))
            {
                return (false, "All fields are required.");
            }

            // Mã hóa mật khẩu
            var user = new User
            {
                Email = email,
                Password = _passwordEncryptionService.EncryptPassword(password),
                Name = name,
                Address = address,
                CreatedDate = DateTime.Now,
                Role = "User",
                AccountType = "Free"
            };

            // Lưu thông tin người dùng vào cơ sở dữ liệu
            await _userRepository.AddUserAsync(user);

            SendWelcomeMail(user);

            return (true, "Registration successful.");
        }

        private void SendWelcomeMail(User user)
        {
            string subject = "Wellcome to Musik";
            string body = $"Hello {user.Email},\n\nWellcome to Musik - A free music web player";
            _emailService.SendEmail(user.Email, subject, body);

        }
    }
}

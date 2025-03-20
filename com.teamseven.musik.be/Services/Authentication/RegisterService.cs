using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using System;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Authentication
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly IEmailService _emailService;

        public RegisterService(
            IUserRepository userRepository,
            IPasswordEncryptionService passwordEncryptionService,
            IEmailService emailService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordEncryptionService = passwordEncryptionService ?? throw new ArgumentNullException(nameof(passwordEncryptionService));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task RegisterUserAsync(string email, string password, string name, string img)
        {
            // Validation
            ValidateInput(email, password, name);
            await ValidateEmailUniqueness(email);

            // Tạo user mới
            var user = new User
            {
                Email = email,
                Password = _passwordEncryptionService.EncryptPassword(password),
                Name = name,
                Address = "NO DATA",
                CreatedAt = DateTime.Now,
                Role = "User",
                AccountType = "Free",
                ImgLink = img,
                NumberOfSubscriber = 0
            };

            // Lưu vào database và gửi email
            await _userRepository.AddUserAsync(user);
            SendWelcomeMail(user);
        }

        private void ValidateInput(string email, string password, string name)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.", nameof(password));
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required.", nameof(name));
        }

        private async Task ValidateEmailUniqueness(string email)
        {
            var existingUser = await _userRepository.GetByEmailAsync(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email already in use.");
        }

        private void SendWelcomeMail(User user)
        {
            string subject = "Welcome to Musik";
            string body = $"Hello {user.Email},\n\nWelcome to Musik - A free music web player";
            _emailService.SendEmail(user.Email, subject, body);
        }
    }
}
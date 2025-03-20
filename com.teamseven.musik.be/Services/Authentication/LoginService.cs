using com.teamseven.musik.be.Models.Entities;
using com.teamseven.musik.be.Repositories.interfaces;
using com.teamseven.musik.be.Services.Interfaces;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.teamseven.musik.be.Services.Authentication
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordEncryptionService _passwordEncryptionService;
        private readonly IAuthService _authService;

        public LoginService(
            IUserRepository userRepository,
            IPasswordEncryptionService passwordEncryptionService,
            IAuthService authService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordEncryptionService = passwordEncryptionService ?? throw new ArgumentNullException(nameof(passwordEncryptionService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async Task<string> ValidateUserAsync(string email, string password)
        {
            // Validation đầu vào
            ValidateInput(email, password);

            // Kiểm tra user tồn tại
            User user = await ValidateUserExistence(email);

            // Kiểm tra mật khẩu
            ValidatePassword(password, user.Password);

            // Tạo và trả về token
            return _authService.GenerateJwtToken(user);
        }

        private void ValidateInput(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email is required.", nameof(email));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password is required.", nameof(password));
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.", nameof(email));
        }

        private async Task<User> ValidateUserExistence(string email)
        {
            User user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return user;
        }

        private void ValidatePassword(string password, string hashedPassword)
        {
            if (!_passwordEncryptionService.VerifyPassword(password, hashedPassword))
                throw new UnauthorizedAccessException("Invalid password.");
        }

        private bool IsValidEmail(string email)
        {
            // Regex đơn giản để kiểm tra định dạng email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
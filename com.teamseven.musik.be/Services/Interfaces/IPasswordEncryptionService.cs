namespace com.teamseven.musik.be.Services.Authentication
{
    public interface IPasswordEncryptionService
    {
        /// <summary>
        /// Mã hóa mật khẩu người dùng.
        /// </summary>
        /// <param name="plainPassword">Mật khẩu gốc.</param>
        /// <returns>Mật khẩu đã mã hóa.</returns>
        string EncryptPassword(string plainPassword);

        /// <summary>
        /// Xác minh mật khẩu người dùng so với mật khẩu đã mã hóa.
        /// </summary>
        /// <param name="plainPassword">Mật khẩu gốc do người dùng nhập.</param>
        /// <param name="hashedPassword">Mật khẩu đã mã hóa trong cơ sở dữ liệu.</param>
        /// <returns>True nếu mật khẩu khớp, ngược lại False.</returns>
        bool VerifyPassword(string plainPassword, string hashedPassword);
    }
}
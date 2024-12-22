namespace SecureStore.Api.InfrastructureLayer.Utils
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public static bool ValidatePassword(string password, string hashedPassword)
        {
            string hashedInputPassword = HashPassword(password);
            return hashedInputPassword.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }

}

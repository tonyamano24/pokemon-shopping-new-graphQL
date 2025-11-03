// i want to do hash class for encryption and compare with bcrypt

using System;

namespace itsc_dotnet_practice.Utilities
{
    public class EncryptionUtility
    {
        public static string HashPassword(string password)
        {
            // Use a secure hashing algorithm like BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify the provided password against the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        
        public static string GenerateSalt()
        {
            // Generate a salt for hashing (not needed for BCrypt as it handles this internally)
            return BCrypt.Net.BCrypt.GenerateSalt();
        }
    }
}

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace backend.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public partial class User
    {
        public User() { }

        public User(string username, string password)
        {
            Username = username;
            Salt = GenerateSalt();
            PasswordHash = HashPassword(password, Salt);
        }

        public bool CheckCredentials(string password, byte[] salt)
        {
            return PasswordHash!.SequenceEqual(HashPassword(password, salt));
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[]? PasswordHash { get; set; }

        [Required]
        [MaxLength(16)]
        public byte[]? Salt { get; set; }

        public LikedMusic LikedMusic { get; set; }

        public DislikedMusic DislikedMusic { get; set; }

        private static readonly Regex regex = UsernamePasswordValidationRegex();

        public static bool IsAlphanumeric(string input)
        {
            return regex.IsMatch(input);
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        private static byte[] HashPassword(string password, byte[] salt)
        {
            byte[] hashedPassword = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32);

            return hashedPassword;
        }

        [GeneratedRegex("^[a-zA-Z0-9]*$")]
        private static partial Regex UsernamePasswordValidationRegex();
    }
}

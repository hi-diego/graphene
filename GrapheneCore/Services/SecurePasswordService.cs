using GrapheneCore.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Services
{
    public class HashingOptions
    {
        public int Iterations { get; set; } = 10000;
    }

    public class SecurePasswordService : IPasswordHasher<IAuthenticable>
    {
        private const int SaltSize = 16; // 128 bit 
        private const int KeySize = 32; // 256 bit

        public SecurePasswordService(HashingOptions options = null)
        {
            if (options == null)
                options = new HashingOptions();
            Options = options;
        }

        private HashingOptions Options { get; }

        public string Hash(string password = "secret")
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Options.Iterations,
                HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return $"{Options.Iterations}.{salt}.{key}";
            }
        }

        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. " +
                "Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != Options.Iterations;

            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);

                var verified = keyToCheck.SequenceEqual(key);

                return (verified, needsUpgrade);
            }
        }

        public string HashPassword(IAuthenticable user, string password)
        {
            return Hash(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(IAuthenticable user, string hashedPassword, string providedPassword)
        {
            var result = Check(hashedPassword, providedPassword);
            return result.NeedsUpgrade
                ? PasswordVerificationResult.SuccessRehashNeeded
                : result.Verified
                    ? PasswordVerificationResult.Success
                    : PasswordVerificationResult.Failed;
        }
    }
}

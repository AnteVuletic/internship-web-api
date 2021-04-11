using System;
using System.Security.Cryptography;

namespace StudentMentor.Domain.Helpers
{
    public static class EncryptionHelper
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int NumberOfIterations = 100;

        public static string Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, NumberOfIterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            return Convert.ToBase64String(hashBytes);
        }

        public static bool ValidatePassword(string password, string hashedPassword)
        {
            //get hashbytes
            var hashBytes = Convert.FromBase64String(hashedPassword);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, NumberOfIterations);
            var hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

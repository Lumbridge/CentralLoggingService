using System;
using System.Security.Cryptography;
using System.Text;

namespace CLS.Infrastructure.Helpers
{
    public class EncryptionHelper
    {
        private const int SALT_SIZE = 24;
        private const int HASH_SIZE = 24;
        private const int ITERATIONS = 100000;

        /// <summary>
        /// Get the hash of the input
        /// </summary>
        /// <param name="input">string input</param>
        /// <param name="salt">a salt can be passed in to hash the input against</param>
        /// <returns>Hash secured input</returns>
        public static string ComputeHash(string input)
        {
            // 1.-Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALT_SIZE]);

            // 2.-Create the RFC2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, ITERATIONS);
            var hash = pbkdf2.GetBytes(HASH_SIZE);

            // 3.-Combine the salt and input bytes for later use
            var hashBytes = new byte[SALT_SIZE + HASH_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, HASH_SIZE);

            // 4.-Turn the combined salt+hash into a string for storage
            var hashedInput = Convert.ToBase64String(hashBytes);

            return hashedInput;
        }

        /// <summary>
        /// Check if the input is valid
        /// </summary>
        /// <param name="password">entered by user</param>
        /// <param name="hashPass">stored input</param>
        /// <returns>true if valid, false if invalid</returns>
        public static bool IsValidPassword(string password, string hashPass)
        {
            //1.-Extract the bytes
            var hashBytes = Convert.FromBase64String(hashPass);
            
            //2.-Get the salt
            var salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);
            
            //3.-Compute the hash on the input the user entered
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            var hash = pbkdf2.GetBytes(HASH_SIZE);

            //4.-compare the results
            for (var i = 0; i < SALT_SIZE; i++)
            {
                if (hashBytes[i + SALT_SIZE] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}

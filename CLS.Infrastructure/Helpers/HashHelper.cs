using System;
using System.Security.Cryptography;
using System.Text;

namespace CLS.Infrastructure.Helpers
{
    public static class HashHelper
    {
        public static string Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder(hash.Length * 2);

                foreach (var b in hash)
                {
                    // can be "x2" if you want lowercase
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}

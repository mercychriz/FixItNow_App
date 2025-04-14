using System;
using System.Security.Cryptography;
using System.Text;

namespace FixITNowWebApp.Helpers
{
    public static class HashHelper
    {
        public static string ComputeSha256(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}

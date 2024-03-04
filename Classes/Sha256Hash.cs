using System;
using System.Text;
using System.Security.Cryptography;

namespace PalletSyncApi.Classes
{
    //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
    //https://gamedevacademy.org/c-sha256-tutorial-complete-guide/#Comparing_SHA256_Hashes

    public class Sha256Hash
    {
        //public static string ComputeSha256Hash(string rawData)
        //{
        //    using (SHA256 sha256Hash = SHA256.Create())
        //    {
        //        // ComputeHash - returns byte array
        //        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

        //        // Convert byte array to string
        //        StringBuilder builder = new StringBuilder();

        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();  
        //    }
        //}

        public static string HashPasswordWithSalt(string rawData, out string salt)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[16];
            rng.GetBytes(saltBytes);

            salt = Convert.ToBase64String(saltBytes);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(rawData, saltBytes, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            return Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string salt, string hashedPassword)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            string newHashedPassword = Convert.ToBase64String(hash);
            return newHashedPassword.Equals(hashedPassword);
        }
    }
}

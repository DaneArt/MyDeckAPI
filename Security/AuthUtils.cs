using Microsoft.IdentityModel.Tokens;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyDeckAPI.Security
{
    public class AuthUtils
    {
        public const string ISSUER = "MyDeckAPI";
        public const string AUDIENCE = "MyDeck";
        public const string GOOGLEAUDIENCE = "292694254456-dau67egnnm7s513fofc4gvu5ftjut9le.apps.googleusercontent.com";
        public const string GOOGLEISSUER = "https://accounts.google.com";
        private const string KEY = "2tvPsIUvPTegtAH5TZ7e9ktUUGctOnOkwfiE98luLdcsoUeFECRk55wclKZWlXau";
        private const string EMAILCONFIRMATIONKEY = "2tvPsIUvPTegtAH5TZ7e9ktUUGsoUeFECRk55wclKZWlXauFECRkKZWlXauoUeFs";
        private const string GOOGLEKEY = "tLECRAH5TZ7e9ktUUGct2tvPdcsE98luk55wIUvPTegOnOkwficlKZWlXauoUeFs";
        public const int LIFETIME = 30;
        public const int REFRESH_LIFETIME = 43800;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(KEY));
        }
        public static SymmetricSecurityKey GetEmailConfirmationSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(EMAILCONFIRMATIONKEY));
        }
        public static SymmetricSecurityKey GetGoogleSecurityKey()
        {
            return new SymmetricSecurityKey(Convert.FromBase64String(GOOGLEKEY));
        }
        public byte[] GenerateRandomPasswordForUser(Guid userId)
        {

            var password = GetMd5HashString("mydeckapi/user/" + userId);
            return Encoding.UTF8.GetBytes(password);
        }

        public byte[] GeneratePaswordWithSaltForUser(Guid userId)
        {
            return GetPasswordWithSalt(GenerateRandomPasswordForUser(userId));
        }
        public String GetMd5HashString(String value)
        {
            byte[] hash;
            var sb = new StringBuilder();
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
            }
            return sb.ToString();
        }

        public string GetFilePathFromMD5(string md5Path)
        {
            
            StringBuilder pathBuilder = new StringBuilder();


            for (int i = 0; i < md5Path.Length; i++)
            {
                pathBuilder.Append(md5Path[i]);
                if (i % 2 == 0)
                {
                    pathBuilder.Append("/");
                }
            }

            return pathBuilder.ToString();

        }

        public bool validatePassword(byte[] userPassword, byte[] dbPassword)
        {
            var passHash = dbPassword.Take(32).ToArray();
            var salt = dbPassword.Skip(32).ToArray();

            var userPassHash = new byte[dbPassword.Length];
            for (int i = 0; i < userPassword.Length; i++)
                userPassHash[i] = userPassword[i];

            for (int i = 0; i < salt.Length; i++)
                userPassHash[userPassword.Length + i] = salt[i];

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hashBytes = sha256Hash.ComputeHash(userPassHash);
                return Enumerable.SequenceEqual(passHash,hashBytes);
            }
        }
        public byte[] GetPasswordWithSalt(byte[] password)
        {
            var rng = new RNGCryptoServiceProvider();
            Random random = new Random();
            var pass = password;
            int saltSize = random.Next(4, 8);
            var salt = new byte[saltSize];
            var passwordHash = new byte[saltSize + pass.Length];

            rng.GetNonZeroBytes(salt);
            for (int i = 0; i < pass.Length; i++)
                passwordHash[i] = pass[i];


            for (int i = 0; i < salt.Length; i++)
                passwordHash[pass.Length + i] = salt[i];

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hashBytes = sha256Hash.ComputeHash(passwordHash);
                byte[] passwordHashWithSalt = new byte[hashBytes.Length +
                                          salt.Length];
                for (int i = 0; i < hashBytes.Length; i++)
                    passwordHashWithSalt[i] = hashBytes[i];

                for (int i = 0; i < salt.Length; i++)
                    passwordHashWithSalt[hashBytes.Length + i] = salt[i];
                return passwordHashWithSalt;
            }
        }
    }
}

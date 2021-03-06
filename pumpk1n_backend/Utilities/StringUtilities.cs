using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace pumpk1n_backend.Utilities
{
    public static class StringUtilities
    {
        public static string Sha512Hash(string input)
        {
            SHA512 sha512 = new SHA512Managed();
            var hashedResult = sha512.ComputeHash(Encoding.UTF8.GetBytes(input));
            var result = BitConverter.ToString(hashedResult).Replace("-", "").ToLower();
            return result;
        }
        
        public static string RemoveMark(string input)
        {
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            var temp = input.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static int GenerateRandomNumber(int start = 100000000, int end = 999999999)
        {
            var random = new Random();
            var randomNumber = random.Next(start, end);
            return randomNumber;
        }
        
        public static string GenerateRandomString(int cryptoByteLength=64)
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[cryptoByteLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var randomGuid = new Guid();
            var currentDateTime = new DateTime().ToString(CultureInfo.InvariantCulture);
            var randomCryptographicString = Convert.ToBase64String(randomBytes);
            var combinedString = $"{randomCryptographicString}+{randomGuid.ToString()}+{currentDateTime}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(combinedString));
        }

        public static string HashPassword(string password, string nonce)
        {
            var data = $"{password}+{nonce}";
            return Sha512Hash(data);
        }
    }
}
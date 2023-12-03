using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Cryptography;
using System.Text;

namespace Waifustasia.Data.Authentication
{
    public class CryptoString
    {
        private readonly string defaultKey = "waifustasiakey";
        private readonly byte[] key;

        public CryptoString(byte[] key)
        {
            this.key = key;
        }

        public CryptoString(string key)
        {
            this.key = GenerateKey(key);
        }

        public CryptoString()
        {
            key = GenerateKey(defaultKey);
        }

        public byte[] EncryptString(string plainText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = new byte[16]; // IV (Initialization Vector) should be unique and random

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            return msEncrypt.ToArray();
        }

        public byte[] DecryptString(byte[] cipherText)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = key;
            aesAlg.IV = new byte[16]; // IV (Initialization Vector) should be the same as used for encryption

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using MemoryStream msDecrypt = new MemoryStream(cipherText);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);

            return Encoding.UTF8.GetBytes(srDecrypt.ReadToEnd());
        }

        public byte[] GenerateKey(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public string DecryptSettingString(string v)
        {
            var bytes = v.StringToBytes();
            var decryptedString = DecryptString(bytes);

            string result = Encoding.UTF8.GetString(decryptedString);
            return result;
        }
    }

    public static class CryptoExtensions
    {
        public static string KeyAsString(this byte[] encrypted)
        {
            return Convert.ToBase64String(encrypted);
        }

        public static byte[] StringToBytes(this string text)
        {
            return Convert.FromBase64String(text);
        }
    }
}

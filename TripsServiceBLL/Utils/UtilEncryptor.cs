using System.Security.Cryptography;
using System.Text;

namespace TripsServiceBLL.Utils;

public static class UtilEncryptor
{
    private static readonly byte[] _aesKey = Encoding.ASCII.GetBytes(UtilConstants.AesKey);
    private static readonly byte[] _aesSecret = Encoding.ASCII.GetBytes(UtilConstants.AesSecret);

    public static string Encrypt(string text)
    {
        byte[] encrypted;
        using (Aes aes = Aes.Create())
        {
            aes.Key = _aesKey;
            aes.IV = _aesSecret;
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream =
                new(memoryStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new(cryptoStream))
            {
                streamWriter.Write(text);
            }

            encrypted = memoryStream.ToArray();
        }

        return Convert.ToBase64String(encrypted);
    }
}
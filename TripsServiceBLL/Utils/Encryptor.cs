using System.Security.Cryptography;
using System.Text;

namespace TripsServiceBLL.Utils
{
	public static class Encryptor
	{

		static readonly byte[] _aesKey = Encoding.ASCII.GetBytes(Constants.AesKey);

		static readonly byte[] _aesIV = Encoding.ASCII.GetBytes(Constants.AesIV);

		public static string Encrypt(string text)
		{
			byte[] encrypted;
			using (Aes aes = Aes.Create())
			{
				aes.Key = _aesKey;
				aes.IV = _aesIV;
				using MemoryStream memoryStream = new();
				using CryptoStream cryptoStream = new(memoryStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write);
				using (StreamWriter streamWriter = new(cryptoStream))
				{
					streamWriter.Write(text);
				}

				encrypted = memoryStream.ToArray();
			}

			return Convert.ToBase64String(encrypted);
		}
	}
}

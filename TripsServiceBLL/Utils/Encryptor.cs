using System.Security.Cryptography;
using System.Text;

namespace TripsServiceBLL.Utils
{
	public static class Encryptor
	{

		static readonly byte[] _tripleDESKey = Encoding.ASCII.GetBytes("JrchIVQqLTTFkS1SiL1JPRJm");

		static readonly byte[] _tripleDESIV = Encoding.ASCII.GetBytes("tsXLqW0Y");

		public static string Encrypt(string text)
		{
			byte[] encrypted;
			using (TripleDES tripleDES = TripleDES.Create())
			{
				tripleDES.Key = _tripleDESKey;
				tripleDES.IV = _tripleDESIV;
				using MemoryStream memoryStream = new();
				using CryptoStream cryptoStream = new(memoryStream, tripleDES.CreateEncryptor(), CryptoStreamMode.Write);
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

using System.Security.Cryptography;
using System.Text;

namespace DatingApp.API.Extensions
{
	public static class EncryptExtensions
	{
		private static readonly string _key = "7786933305309489";
		private static readonly string _iv = "3087914151076047";

		public static string ToAESDecrypt(this string needDecrypt)
		{
			string result = "";
			try
			{
				byte[] needDecryptBytes = Convert.FromBase64String(needDecrypt);
				using Aes aesAlg = GetEncryptionAlgSetting();
				using ICryptoTransform decryptor = aesAlg.CreateDecryptor();
				byte[] decryptedBytes = decryptor.TransformFinalBlock(needDecryptBytes, 0, needDecryptBytes.Length);
				result = Encoding.UTF8.GetString(decryptedBytes);
			}
			catch (Exception ex)
			{
				throw;
			}

			return result;
		}

		public static string ToAESEncrypt(this string needEncrypt)
		{
			string result = "";
			try
			{
				byte[] needEncryptBytes = Encoding.UTF8.GetBytes(needEncrypt);
				using Aes aesAlg = GetEncryptionAlgSetting();
				using ICryptoTransform encryptor = aesAlg.CreateEncryptor();
				byte[] encryptedBytes = encryptor.TransformFinalBlock(needEncryptBytes, 0, needEncryptBytes.Length);
				result = Convert.ToBase64String(encryptedBytes);
			}
			catch (Exception ex)
			{
				throw;
			}

			return result;
		}

		private static Aes GetEncryptionAlgSetting()
		{
			Aes aes = Aes.Create();
			aes.Key = Encoding.UTF8.GetBytes(_key);
			aes.IV = Encoding.UTF8.GetBytes(_iv);
			aes.Padding = PaddingMode.PKCS7;
			aes.Mode = CipherMode.CBC;
			return aes;
		}
	}
}
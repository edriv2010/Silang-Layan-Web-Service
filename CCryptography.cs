// CCryptography
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class CCryptography
{
	public struct Keys
	{
		public string Key;

		public string IV;
	}

	public class Rijndael
	{
		private static byte[] savedKey;

		private static byte[] savedIV;

		public static byte[] Key
		{
			get
			{
				return savedKey;
			}
			set
			{
				savedKey = value;
			}
		}

		public static byte[] IV
		{
			get
			{
				return savedIV;
			}
			set
			{
				savedIV = value;
			}
		}

		private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
		{
			if (savedKey == null)
			{
				rdProvider.KeySize = 256;
				rdProvider.GenerateKey();
				savedKey = rdProvider.Key;
			}
		}

		private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
		{
			if (savedIV == null)
			{
				rdProvider.GenerateIV();
				savedIV = rdProvider.IV;
			}
		}

		public static string Encrypt(string originalStr, string SavedKeyString, string SavedIVString)
		{
			byte[] originalStrAsBytes = Encoding.ASCII.GetBytes(originalStr);
			MemoryStream memStream = new MemoryStream(originalStrAsBytes.Length);
			RijndaelManaged Rijndael = new RijndaelManaged();
			if (SavedKeyString == "")
			{
				RdGenerateSecretKey(Rijndael);
			}
			else
			{
				savedKey = Convert.FromBase64String(SavedKeyString);
			}
			if (SavedIVString == "")
			{
				RdGenerateSecretInitVector(Rijndael);
			}
			else
			{
				savedIV = Convert.FromBase64String(SavedIVString);
			}
			if (savedKey == null || savedIV == null)
			{
				throw new NullReferenceException("savedKey and savedIV must be non-null.");
			}
			ICryptoTransform rdTransform = Rijndael.CreateEncryptor((byte[])savedKey.Clone(), (byte[])savedIV.Clone());
			CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, CryptoStreamMode.Write);
			cryptoStream.Write(originalStrAsBytes, 0, originalStrAsBytes.Length);
			cryptoStream.FlushFinalBlock();
			byte[] originalBytes = memStream.ToArray();
			memStream.Close();
			cryptoStream.Close();
			rdTransform.Dispose();
			Rijndael.Clear();
			return Convert.ToBase64String(originalBytes);
		}

		public static string Decrypt(string encryptedStr, string SavedKeyString, string SavedIVString)
		{
			if (SavedKeyString.ToString() != "")
			{
				savedKey = Convert.FromBase64String(SavedKeyString);
				savedIV = Convert.FromBase64String(SavedIVString);
			}
			byte[] encryptedStrAsBytes = Convert.FromBase64String(encryptedStr);
			byte[] initialText = new byte[encryptedStrAsBytes.Length];
			RijndaelManaged Rijndael = new RijndaelManaged();
			MemoryStream memStream = new MemoryStream(encryptedStrAsBytes);
			if (savedKey == null || savedIV == null)
			{
				throw new NullReferenceException("savedKey and savedIV must be non-null.");
			}
			ICryptoTransform rdTransform = Rijndael.CreateDecryptor((byte[])savedKey.Clone(), (byte[])savedIV.Clone());
			CryptoStream cryptoStream = new CryptoStream(memStream, rdTransform, CryptoStreamMode.Read);
			cryptoStream.Read(initialText, 0, initialText.Length);
			memStream.Close();
			cryptoStream.Close();
			rdTransform.Dispose();
			Rijndael.Clear();
			string decryptedStr = Encoding.ASCII.GetString(initialText);
			return decryptedStr.Replace("\0", "");
		}
	}

	public static Keys GetKeys(string password, int keyBitLength)
	{
		Keys keys = default(Keys);
		RijndaelManaged Rijndael = new RijndaelManaged();
		byte[] salt = new byte[10] { 1, 2, 23, 234, 37, 48, 134, 63, 248, 4 };
		using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 234))
		{
			if (!Rijndael.ValidKeySize(keyBitLength))
			{
				throw new InvalidOperationException("Invalid size key");
			}
			keys.Key = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(keyBitLength / 8));
			keys.IV = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(Rijndael.BlockSize / 8));
		}
		return keys;
	}
}

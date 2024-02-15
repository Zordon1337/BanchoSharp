using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
        public static string key = "h89f2-890h2h89b34g-h80g134n90133";
        public static string EncryptString(string message, byte[] key, ref string iv)
        {   
			string result = null;
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			try
			{
				rijndaelManaged.Key = key;
				rijndaelManaged.BlockSize = 256;
				rijndaelManaged.Mode = CipherMode.CBC;
				if (iv != null)
				{
					rijndaelManaged.IV = Convert.FromBase64String(iv);
				}
				else
				{
					rijndaelManaged.GenerateIV();
					iv = Convert.ToBase64String(rijndaelManaged.IV);
				}
				try
				{
					MemoryStream memoryStream = new MemoryStream();
					CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(key, rijndaelManaged.IV), CryptoStreamMode.Write);
					try
					{
						StreamWriter streamWriter = new StreamWriter(cryptoStream);
						try
						{
							streamWriter.Write(message);
							streamWriter.Close();
						}
						finally
						{
							((IDisposable)streamWriter).Dispose();
						}
						cryptoStream.Close();
					}
					finally
					{
						((IDisposable)cryptoStream).Dispose();
					}
					result = Convert.ToBase64String(memoryStream.ToArray());
					memoryStream.Close();
				}
				finally
				{
					rijndaelManaged.Clear();
				}
			}
			finally
			{
				((IDisposable)rijndaelManaged).Dispose();
			}
			return result;
		}
        public static string DecryptString(string encryptedMessage, byte[] key, string iv)
        {
            return "I HATE DECRYPTING";
        }



		

		public static string GetMd5String(string instr)
		{
			return CryptoHelper.GetMd5String(Encoding.UTF8.GetString(CryptoHelper.utf8Encoding.GetBytes(instr)));
		}

		

		public static byte[] GetMd5ByteArray(byte[] data)
		{
			MD5 obj = CryptoHelper.md5Hasher;
			lock (obj)
			{
				try
				{
					data = CryptoHelper.md5Hasher.ComputeHash(data);
				}
				catch (Exception)
				{
					return new byte[0];
				}
			}
			return data;
		}

		public static byte[] GetMd5ByteArrayString(string instr)
		{
			MD5 obj = CryptoHelper.md5Hasher;
			byte[] result;
			lock (obj)
			{
				try
				{
					result = CryptoHelper.md5Hasher.ComputeHash(CryptoHelper.utf8Encoding.GetBytes(instr));
				}
				catch (Exception)
				{
					return null;
				}
			}
			return result;
		}

		private static MD5 md5Hasher = MD5.Create();

		private static UTF8Encoding utf8Encoding = new UTF8Encoding();

		
}
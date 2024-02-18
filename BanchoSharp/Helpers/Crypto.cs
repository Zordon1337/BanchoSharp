using System.Security.Cryptography;
using System.Text;

public static class CryptoHelper
{
        public static string key = "h89f2-890h2h89b34g-h80g134n90133";
        public static string CreateMD5(string text)
		{
			string result = BitConverter.ToString(md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-","").ToLower();
			Console.WriteLine(result);
			return result;
		}

		private static MD5 md5Hasher = MD5.Create();

		private static UTF8Encoding utf8Encoding = new UTF8Encoding();

		
}
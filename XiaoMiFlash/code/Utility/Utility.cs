using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000024 RID: 36
	public class Utility
	{
		// Token: 0x060000FE RID: 254 RVA: 0x0000D7A8 File Offset: 0x0000B9A8
		public static string GetMD5HashFromFile(string fileName)
		{
			string result;
			try
			{
				FileStream file = new FileStream(fileName, 3);
				MD5 md5 = new MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				file.Close();
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < retVal.Length; i++)
				{
					sb.Append(retVal[i].ToString("x2"));
				}
				result = sb.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
			}
			return result;
		}
	}
}

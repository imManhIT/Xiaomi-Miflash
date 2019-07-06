using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000037 RID: 55
	internal class IniFile
	{
		// Token: 0x0600016B RID: 363
		[DllImport("kernel32", CharSet = CharSet.Auto)]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		// Token: 0x0600016C RID: 364
		[DllImport("kernel32", CharSet = CharSet.Auto)]
		private static extern long GetPrivateProfileString(string section, string key, string strDefault, StringBuilder retVal, int size, string filePath);

		// Token: 0x0600016D RID: 365
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern int GetPrivateProfileInt(string section, string key, int nDefault, string filePath);

		// Token: 0x0600016E RID: 366 RVA: 0x0000FE6C File Offset: 0x0000E06C
		public IniFile()
		{
			string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			string logicPath = path.Substring(0, path.IndexOf('\\') + 1);
			this.strIniFilePath = logicPath + "MiFlashvcom.ini";
			if (!File.Exists(this.strIniFilePath))
			{
				using (FileStream fs = File.Create(this.strIniFilePath))
				{
					fs.Close();
				}
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public bool GetIniString(string section, string key, string strDefault, StringBuilder retVal, int size)
		{
			long liRet = IniFile.GetPrivateProfileString(section, key, strDefault, retVal, size, this.strIniFilePath);
			return liRet >= 1L;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000FF18 File Offset: 0x0000E118
		public int GetIniInt(string section, string key, int nDefault)
		{
			return IniFile.GetPrivateProfileInt(section, key, nDefault, this.strIniFilePath);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000FF28 File Offset: 0x0000E128
		public bool WriteIniString(string section, string key, string val)
		{
			long liRet = IniFile.WritePrivateProfileString(section, key, val, this.strIniFilePath);
			return liRet != 0L;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000FF4C File Offset: 0x0000E14C
		public bool WriteIniInt(string section, string key, int val)
		{
			return this.WriteIniString(section, key, val.ToString());
		}

		// Token: 0x0400010F RID: 271
		private string strIniFilePath;
	}
}

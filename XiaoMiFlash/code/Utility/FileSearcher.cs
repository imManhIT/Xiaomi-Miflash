using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x0200002C RID: 44
	public class FileSearcher
	{
		// Token: 0x06000130 RID: 304 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public static string[] SearchFiles(string destinationDic, string pattern)
		{
			List<string> patchList = new List<string>();
			DirectoryInfo folder = new DirectoryInfo(destinationDic);
			foreach (FileInfo file in folder.GetFiles())
			{
				Regex reg = new Regex(pattern);
				Match match = reg.Match(file.Name);
				if (match.Groups.Count > 0 && match.Groups[0].Value == file.Name)
				{
					patchList.Add(file.FullName);
				}
			}
			return patchList.ToArray();
		}
	}
}

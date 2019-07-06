using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200003C RID: 60
	public class MiProperty
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00010C9C File Offset: 0x0000EE9C
		public static Hashtable DlTable
		{
			get
			{
				MiProperty._dltable = new Hashtable();
				MiProperty._dltable.Add("download_only", "dl_only");
				MiProperty._dltable.Add("format_and_download", "fm_and_dl");
				MiProperty._dltable.Add("firmware_upgrade", "firmware_upgrade");
				MiProperty._dltable.Add("format_all", "fm");
				return MiProperty._dltable;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00010D08 File Offset: 0x0000EF08
		public static Dictionary<string, int> ChkSumTable
		{
			get
			{
				MiProperty._chksumtable = new Dictionary<string, int>();
				MiProperty._chksumtable.Add("None", 0);
				MiProperty._chksumtable.Add("Usb+dram checksum", 1);
				MiProperty._chksumtable.Add("Flash checksum", 2);
				MiProperty._chksumtable.Add("All checksum", 3);
				return MiProperty._chksumtable;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00010D64 File Offset: 0x0000EF64
		public static string[] DaList
		{
			get
			{
				string basePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
				string daPath = basePath + "\\da";
				if (Directory.Exists(daPath))
				{
					DirectoryInfo dicInfo = new DirectoryInfo(daPath);
					FileInfo[] fileList = dicInfo.GetFiles();
					MiProperty._dalist = new string[Enumerable.Count<FileInfo>(fileList)];
					for (int i = 0; i < Enumerable.Count<FileInfo>(fileList); i++)
					{
						MiProperty._dalist[i] = fileList[i].Name;
					}
				}
				return MiProperty._dalist;
			}
		}

		// Token: 0x04000138 RID: 312
		private static Hashtable _dltable;

		// Token: 0x04000139 RID: 313
		private static Dictionary<string, int> _chksumtable;

		// Token: 0x0400013A RID: 314
		private static string[] _dalist;
	}
}

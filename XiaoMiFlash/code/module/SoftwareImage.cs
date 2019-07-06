using System;
using System.Collections;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000016 RID: 22
	public class SoftwareImage
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000BA1F File Offset: 0x00009C1F
		public static string ProgrammerPattern
		{
			get
			{
				return "MPRG.*.hex|MPRG.*.mbn|prog_.*_firehose_.*.*";
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000BA26 File Offset: 0x00009C26
		public static string BootImage
		{
			get
			{
				return "*_msimage.mbn";
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x0000BA2D File Offset: 0x00009C2D
		public static string ProvisionPattern
		{
			get
			{
				return "provision.*\\.xml";
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000BA34 File Offset: 0x00009C34
		public static string RawProgramPattern
		{
			get
			{
				return "rawprogram[0-9]{1,20}\\.xml";
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x0000BA3B File Offset: 0x00009C3B
		public static string PatchPattern
		{
			get
			{
				return "patch[0-9]{1,20}\\.xml";
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x0000BA44 File Offset: 0x00009C44
		public static Hashtable DummyProgress
		{
			get
			{
				Hashtable table = new Hashtable();
				table.Add("xbl", 1);
				table.Add("tz", 2);
				table.Add("hyp", 3);
				table.Add("rpm", 4);
				table.Add("emmc_appsboot", 5);
				table.Add("pmic", 6);
				table.Add("devcfg", 7);
				table.Add("BTFM", 8);
				table.Add("cmnlib", 9);
				table.Add("cmnlib64", 10);
				table.Add("NON-HLOS", 11);
				table.Add("adspso", 12);
				table.Add("mdtp", 13);
				table.Add("keymaster", 14);
				table.Add("misc", 15);
				table.Add("system", 16);
				table.Add("cache", 30);
				table.Add("userdata", 34);
				table.Add("recovery", 35);
				table.Add("splash", 36);
				table.Add("logo", 37);
				table.Add("boot", 38);
				table.Add("cust", 45);
				return table;
			}
		}
	}
}

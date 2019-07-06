using System;

namespace XiaoMiFlash.code.data
{
	// Token: 0x02000023 RID: 35
	public class MiFlashGlobal
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000D780 File Offset: 0x0000B980
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000D787 File Offset: 0x0000B987
		public static string Version
		{
			get
			{
				return MiFlashGlobal._version;
			}
			set
			{
				MiFlashGlobal._version = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000D78F File Offset: 0x0000B98F
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000D796 File Offset: 0x0000B996
		public static bool IsFactory
		{
			get
			{
				return MiFlashGlobal._isfactory;
			}
			set
			{
				MiFlashGlobal._isfactory = value;
			}
		}

		// Token: 0x040000B7 RID: 183
		private static string _version;

		// Token: 0x040000B8 RID: 184
		private static bool _isfactory;
	}
}

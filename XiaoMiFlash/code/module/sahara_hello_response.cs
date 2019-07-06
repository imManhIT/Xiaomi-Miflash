using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000044 RID: 68
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_hello_response
	{
		// Token: 0x0400015C RID: 348
		public uint Command;

		// Token: 0x0400015D RID: 349
		public uint Length;

		// Token: 0x0400015E RID: 350
		public uint Version;

		// Token: 0x0400015F RID: 351
		public uint Version_min;

		// Token: 0x04000160 RID: 352
		public uint Status;

		// Token: 0x04000161 RID: 353
		public uint Mode;

		// Token: 0x04000162 RID: 354
		[MarshalAs(30, SizeConst = 6)]
		public uint[] Reserved;
	}
}

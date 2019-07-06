using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000046 RID: 70
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_64b_readdata_packet
	{
		// Token: 0x04000168 RID: 360
		public uint Command;

		// Token: 0x04000169 RID: 361
		public uint Length;

		// Token: 0x0400016A RID: 362
		public ulong Image_id;

		// Token: 0x0400016B RID: 363
		public ulong Offset;

		// Token: 0x0400016C RID: 364
		public ulong SLength;
	}
}

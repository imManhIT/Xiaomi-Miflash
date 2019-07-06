using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000047 RID: 71
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_end_transfer_packet
	{
		// Token: 0x0400016D RID: 365
		public uint Command;

		// Token: 0x0400016E RID: 366
		public uint Length;

		// Token: 0x0400016F RID: 367
		public uint Image_id;

		// Token: 0x04000170 RID: 368
		public uint Status;
	}
}

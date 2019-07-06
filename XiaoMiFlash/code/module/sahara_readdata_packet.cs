using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000045 RID: 69
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_readdata_packet
	{
		// Token: 0x04000163 RID: 355
		public uint Command;

		// Token: 0x04000164 RID: 356
		public uint Length;

		// Token: 0x04000165 RID: 357
		public uint Image_id;

		// Token: 0x04000166 RID: 358
		public uint Offset;

		// Token: 0x04000167 RID: 359
		public uint SLength;
	}
}

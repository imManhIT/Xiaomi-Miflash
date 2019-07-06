using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000048 RID: 72
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_done_response
	{
		// Token: 0x04000171 RID: 369
		public uint Command;

		// Token: 0x04000172 RID: 370
		public uint Length;

		// Token: 0x04000173 RID: 371
		public uint Status;
	}
}

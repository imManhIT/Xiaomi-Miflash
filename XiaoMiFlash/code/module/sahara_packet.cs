using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000042 RID: 66
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_packet
	{
		// Token: 0x04000153 RID: 339
		public uint Command;

		// Token: 0x04000154 RID: 340
		public uint Length;
	}
}

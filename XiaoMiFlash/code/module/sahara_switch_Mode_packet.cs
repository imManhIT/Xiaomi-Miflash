using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000049 RID: 73
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_switch_Mode_packet
	{
		// Token: 0x04000174 RID: 372
		public uint Command;

		// Token: 0x04000175 RID: 373
		public uint Length;

		// Token: 0x04000176 RID: 374
		public uint Mode;
	}
}

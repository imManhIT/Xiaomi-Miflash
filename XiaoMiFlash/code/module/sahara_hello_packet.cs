using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000043 RID: 67
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct sahara_hello_packet
	{
		// Token: 0x04000155 RID: 341
		public uint Command;

		// Token: 0x04000156 RID: 342
		public uint Length;

		// Token: 0x04000157 RID: 343
		public uint Version;

		// Token: 0x04000158 RID: 344
		public uint Version_min;

		// Token: 0x04000159 RID: 345
		public uint Max_Command_Length;

		// Token: 0x0400015A RID: 346
		public uint Mode;

		// Token: 0x0400015B RID: 347
		[MarshalAs(30, SizeConst = 6)]
		public uint[] Reserved;
	}
}

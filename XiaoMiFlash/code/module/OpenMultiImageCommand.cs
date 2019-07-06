using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000052 RID: 82
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct OpenMultiImageCommand
	{
		// Token: 0x04000198 RID: 408
		public byte uCommand;

		// Token: 0x04000199 RID: 409
		public byte uType;

		// Token: 0x0400019A RID: 410
		[MarshalAs(30, SizeConst = 1)]
		public byte[] uData;
	}
}

using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000054 RID: 84
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct StreamWriteResponse
	{
		// Token: 0x0400019D RID: 413
		public byte uResponse;

		// Token: 0x0400019E RID: 414
		public int uAddress;
	}
}

using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200004F RID: 79
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ReadPacketResponse
	{
		// Token: 0x04000192 RID: 402
		public byte uResponse;

		// Token: 0x04000193 RID: 403
		public int uAddress;

		// Token: 0x04000194 RID: 404
		[MarshalAs(30, SizeConst = 1)]
		public byte[] uData;
	}
}

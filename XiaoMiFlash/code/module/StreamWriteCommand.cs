using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000055 RID: 85
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct StreamWriteCommand
	{
		// Token: 0x0400019F RID: 415
		public byte uCommand;

		// Token: 0x040001A0 RID: 416
		public int uAddress;

		// Token: 0x040001A1 RID: 417
		[MarshalAs(30, SizeConst = 1)]
		public byte[] uData;
	}
}

using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200004D RID: 77
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct HelloCommand
	{
		// Token: 0x04000185 RID: 389
		public byte uCommand;

		// Token: 0x04000186 RID: 390
		[MarshalAs(30, SizeConst = 32)]
		public byte[] uMagicNumber;

		// Token: 0x04000187 RID: 391
		public byte uVersionNumber;

		// Token: 0x04000188 RID: 392
		public byte uCompatibleVersion;

		// Token: 0x04000189 RID: 393
		public byte uFeatureBits;
	}
}

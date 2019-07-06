using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200004E RID: 78
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct HelloResponse
	{
		// Token: 0x0400018A RID: 394
		public byte uResponse;

		// Token: 0x0400018B RID: 395
		[MarshalAs(30, SizeConst = 32)]
		public byte[] uMagicNumber;

		// Token: 0x0400018C RID: 396
		public byte uVersionNumber;

		// Token: 0x0400018D RID: 397
		public byte uCompatibleVersion;

		// Token: 0x0400018E RID: 398
		public uint uMaxBlockSize;

		// Token: 0x0400018F RID: 399
		public uint uFlashBaseAddress;

		// Token: 0x04000190 RID: 400
		public byte uFlashIdLength;

		// Token: 0x04000191 RID: 401
		[MarshalAs(30, SizeConst = 1024)]
		public byte[] uVariantBuffer;
	}
}

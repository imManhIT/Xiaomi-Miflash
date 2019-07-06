using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200004B RID: 75
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SparseChunkHeader
	{
		// Token: 0x04000180 RID: 384
		public ushort uChunkType;

		// Token: 0x04000181 RID: 385
		public ushort uReserved1;

		// Token: 0x04000182 RID: 386
		public uint uChunkSize;

		// Token: 0x04000183 RID: 387
		public uint uTotalSize;
	}
}

using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200004A RID: 74
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SparseImageHeader
	{
		// Token: 0x04000177 RID: 375
		public uint uMagic;

		// Token: 0x04000178 RID: 376
		public ushort uMajorVersion;

		// Token: 0x04000179 RID: 377
		public ushort uMinorVersion;

		// Token: 0x0400017A RID: 378
		public ushort uFileHeaderSize;

		// Token: 0x0400017B RID: 379
		public ushort uChunkHeaderSize;

		// Token: 0x0400017C RID: 380
		public uint uBlockSize;

		// Token: 0x0400017D RID: 381
		public uint uTotalBlocks;

		// Token: 0x0400017E RID: 382
		public uint uTotalChunks;

		// Token: 0x0400017F RID: 383
		public uint uImageChecksum;
	}
}

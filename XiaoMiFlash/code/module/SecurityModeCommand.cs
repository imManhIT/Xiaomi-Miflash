using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000050 RID: 80
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct SecurityModeCommand
	{
		// Token: 0x04000195 RID: 405
		public byte uCommand;

		// Token: 0x04000196 RID: 406
		public byte uMode;
	}
}

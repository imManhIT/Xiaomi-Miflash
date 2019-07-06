using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000056 RID: 86
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct UserInfo
	{
		// Token: 0x040001A2 RID: 418
		public IntPtr user_id;

		// Token: 0x040001A3 RID: 419
		public IntPtr user_name;

		// Token: 0x040001A4 RID: 420
		public IntPtr user_icon;

		// Token: 0x040001A5 RID: 421
		public int user_icon_len;
	}
}

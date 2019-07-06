using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200000F RID: 15
	public struct FlashType
	{
		// Token: 0x0400002A RID: 42
		public static string CleanAll = "flash_all.bat";

		// Token: 0x0400002B RID: 43
		public static string SaveUserData = "flash_all_except.*\\.bat";

		// Token: 0x0400002C RID: 44
		public static string CleanAllAndLock = "flash_all_lock.bat";
	}
}

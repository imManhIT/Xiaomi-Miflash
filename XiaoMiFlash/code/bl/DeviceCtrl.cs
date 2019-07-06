using System;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x0200000C RID: 12
	public abstract class DeviceCtrl
	{
		// Token: 0x0600003E RID: 62
		public abstract void flash();

		// Token: 0x0600003F RID: 63
		public abstract string[] getDevice();

		// Token: 0x06000040 RID: 64
		public abstract void CheckSha256();

		// Token: 0x04000017 RID: 23
		public string swPath = "D:\\SW\\A1\\FDL153I\\images\\";

		// Token: 0x04000018 RID: 24
		public string scatter = "";

		// Token: 0x04000019 RID: 25
		public ushort idproduct;

		// Token: 0x0400001A RID: 26
		public ushort idvendor;

		// Token: 0x0400001B RID: 27
		public string da = "";

		// Token: 0x0400001C RID: 28
		public string dl_type = "";

		// Token: 0x0400001D RID: 29
		public string sha256Path = "";

		// Token: 0x0400001E RID: 30
		public string deviceName = "";

		// Token: 0x0400001F RID: 31
		public string flashScript;

		// Token: 0x04000020 RID: 32
		public bool readBackVerify;

		// Token: 0x04000021 RID: 33
		public bool openWriteDump;

		// Token: 0x04000022 RID: 34
		public bool openReadDump;

		// Token: 0x04000023 RID: 35
		public bool verbose;
	}
}

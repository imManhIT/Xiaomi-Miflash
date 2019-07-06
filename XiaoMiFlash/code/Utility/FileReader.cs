using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x0200002A RID: 42
	internal class FileReader
	{
		// Token: 0x06000128 RID: 296
		[DllImport("kernel32", SetLastError = true)]
		public static extern IntPtr CreateFile(string FileName, uint DesiredAccess, uint ShareMode, uint SecurityAttributes, uint CreationDisposition, uint FlagsAndAttributes, int hTemplateFile);

		// Token: 0x06000129 RID: 297
		[DllImport("kernel32", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x0600012A RID: 298 RVA: 0x0000E3CB File Offset: 0x0000C5CB
		public IntPtr Open(string FileName)
		{
			this.handle = FileReader.CreateFile(FileName, 2147483648u, 0u, 0u, 3u, 0u, 0);
			if (this.handle != IntPtr.Zero)
			{
				return this.handle;
			}
			throw new Exception("打开文件失败");
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000E406 File Offset: 0x0000C606
		public bool Close()
		{
			return FileReader.CloseHandle(this.handle);
		}

		// Token: 0x040000E5 RID: 229
		private const uint GENERIC_READ = 2147483648u;

		// Token: 0x040000E6 RID: 230
		private const uint OPEN_EXISTING = 3u;

		// Token: 0x040000E7 RID: 231
		private IntPtr handle;
	}
}

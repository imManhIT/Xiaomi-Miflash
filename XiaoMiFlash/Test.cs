using System;
using System.Runtime.InteropServices;
using MiUSB;

namespace XiaoMiFlash
{
	// Token: 0x02000035 RID: 53
	public class Test
	{
		// Token: 0x0600015A RID: 346
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern bool DeviceIoControl(IntPtr hFile, int dwIoControlCode, ref byte[] lpInBuffer, int nInBufferSize, ref byte[] lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

		// Token: 0x0600015B RID: 347 RVA: 0x0000FD54 File Offset: 0x0000DF54
		public static UsbNodeInformation[] GetUsbNodeInformation(string DevicePath)
		{
			if (string.IsNullOrEmpty(DevicePath))
			{
				return null;
			}
			IntPtr hHubDevice = Kernel32.CreateFile("\\\\.\\" + DevicePath, NativeFileAccess.GENERIC_WRITE, NativeFileShare.FILE_SHARE_WRITE, IntPtr.Zero, NativeFileMode.OPEN_EXISTING, IntPtr.Zero, IntPtr.Zero);
			if (hHubDevice == Kernel32.INVALID_HANDLE_VALUE)
			{
				return null;
			}
			byte[] Buffer = new byte[76];
			int nBytesReturned;
			bool Status = Test.DeviceIoControl(hHubDevice, 2229256, ref Buffer, Marshal.SizeOf(Buffer), ref Buffer, Marshal.SizeOf(Buffer), out nBytesReturned, IntPtr.Zero);
			Marshal.GetLastWin32Error();
			Kernel32.CloseHandle(hHubDevice);
			if (!Status)
			{
				return null;
			}
			return null;
		}
	}
}

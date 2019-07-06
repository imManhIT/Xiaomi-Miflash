using System;
using System.Runtime.InteropServices;
using System.Text;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000005 RID: 5
	public class DeviceInfo
	{
		// Token: 0x0600001A RID: 26
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiClassGuidsFromNameA(string ClassN, ref Guid guids, uint ClassNameSize, ref uint ReqSize);

		// Token: 0x0600001B RID: 27
		[DllImport("setupapi.dll")]
		private static extern IntPtr SetupDiGetClassDevsA(ref Guid ClassGuid, uint Enumerator, IntPtr hwndParent, uint Flags);

		// Token: 0x0600001C RID: 28
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, DeviceInfo.SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x0600001D RID: 29
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x0600001E RID: 30
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiGetDeviceRegistryPropertyA(IntPtr DeviceInfoSet, DeviceInfo.SP_DEVINFO_DATA DeviceInfoData, uint Property, uint PropertyRegDataType, StringBuilder PropertyBuffer, uint PropertyBufferSize, IntPtr RequiredSize);

		// Token: 0x0600001F RID: 31 RVA: 0x00002930 File Offset: 0x00000B30
		public static int EnumerateDevices(uint DeviceIndex, string ClassName, StringBuilder DeviceName)
		{
			uint RequiredSize = 0u;
			Guid empty = Guid.Empty;
			Guid[] guids = new Guid[1];
			DeviceInfo.SP_DEVINFO_DATA DeviceInfoData = new DeviceInfo.SP_DEVINFO_DATA();
			bool res = DeviceInfo.SetupDiClassGuidsFromNameA(ClassName, ref guids[0], RequiredSize, ref RequiredSize);
			if (RequiredSize == 0u)
			{
				DeviceName = new StringBuilder("");
				return -2;
			}
			if (!res)
			{
				guids = new Guid[RequiredSize];
				res = DeviceInfo.SetupDiClassGuidsFromNameA(ClassName, ref guids[0], RequiredSize, ref RequiredSize);
				if (!res || RequiredSize == 0u)
				{
					DeviceName = new StringBuilder("");
					return -2;
				}
			}
			IntPtr NewDeviceInfoSet = DeviceInfo.SetupDiGetClassDevsA(ref guids[0], 0u, IntPtr.Zero, 2u);
			if (NewDeviceInfoSet.ToInt32() == -1)
			{
				DeviceName = new StringBuilder("");
				return -3;
			}
			DeviceInfoData.cbSize = 28;
			DeviceInfoData.DevInst = 0;
			DeviceInfoData.ClassGuid = Guid.Empty;
			DeviceInfoData.Reserved = 0UL;
			if (!DeviceInfo.SetupDiEnumDeviceInfo(NewDeviceInfoSet, DeviceIndex, DeviceInfoData))
			{
				DeviceInfo.SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
				DeviceName = new StringBuilder("");
				return -1;
			}
			DeviceName.Capacity = 1000;
			if (!DeviceInfo.SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet, DeviceInfoData, 12u, 0u, DeviceName, 1000u, IntPtr.Zero) && !DeviceInfo.SetupDiGetDeviceRegistryPropertyA(NewDeviceInfoSet, DeviceInfoData, 0u, 0u, DeviceName, 1000u, IntPtr.Zero))
			{
				DeviceInfo.SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
				DeviceName = new StringBuilder("");
				return -4;
			}
			return 0;
		}

		// Token: 0x04000007 RID: 7
		private const int DIGCF_PRESENT = 2;

		// Token: 0x04000008 RID: 8
		private const int MAX_DEV_LEN = 1000;

		// Token: 0x04000009 RID: 9
		private const int SPDRP_FRIENDLYNAME = 12;

		// Token: 0x0400000A RID: 10
		private const int SPDRP_DEVICEDESC = 0;

		// Token: 0x02000006 RID: 6
		[StructLayout(LayoutKind.Sequential)]
		private class SP_DEVINFO_DATA
		{
			// Token: 0x0400000B RID: 11
			public int cbSize;

			// Token: 0x0400000C RID: 12
			public Guid ClassGuid;

			// Token: 0x0400000D RID: 13
			public int DevInst;

			// Token: 0x0400000E RID: 14
			public ulong Reserved;
		}
	}
}

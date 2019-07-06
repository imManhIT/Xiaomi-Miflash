using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x0200002E RID: 46
	public class DeviceClasses
	{
		// Token: 0x06000134 RID: 308
		[DllImport("cfgmgr32.dll")]
		private static extern uint CM_Enumerate_Classes(uint ClassIndex, ref Guid ClassGuid, uint Params);

		// Token: 0x06000135 RID: 309
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiClassNameFromGuidA(ref Guid ClassGuid, StringBuilder ClassName, uint ClassNameSize, ref uint RequiredSize);

		// Token: 0x06000136 RID: 310
		[DllImport("setupapi.dll")]
		private static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr hwndParent, uint Flags);

		// Token: 0x06000137 RID: 311
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

		// Token: 0x06000138 RID: 312
		[DllImport("setupapi.dll")]
		private static extern IntPtr SetupDiOpenClassRegKeyExA(ref Guid ClassGuid, uint samDesired, int Flags, IntPtr MachineName, uint Reserved);

		// Token: 0x06000139 RID: 313
		[DllImport("setupapi.dll")]
		private static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex, DeviceClasses.SP_DEVINFO_DATA DeviceInfoData);

		// Token: 0x0600013A RID: 314
		[DllImport("advapi32.dll")]
		private static extern uint RegQueryValueA(IntPtr KeyClass, uint SubKey, StringBuilder ClassDescription, ref uint sizeB);

		// Token: 0x0600013B RID: 315
		[DllImport("user32.dll")]
		public static extern int LoadBitmapW(int hInstance, ulong Reserved);

		// Token: 0x0600013C RID: 316
		[DllImport("setupapi.dll")]
		public static extern bool SetupDiGetClassImageList(out DeviceClasses.SP_CLASSIMAGELIST_DATA ClassImageListData);

		// Token: 0x0600013D RID: 317
		[DllImport("setupapi.dll")]
		public static extern int SetupDiDrawMiniIcon(Graphics hdc, DeviceClasses.RECT rc, int MiniIconIndex, int Flags);

		// Token: 0x0600013E RID: 318
		[DllImport("setupapi.dll")]
		public static extern bool SetupDiGetClassBitmapIndex(Guid ClassGuid, out int MiniIconIndex);

		// Token: 0x0600013F RID: 319
		[DllImport("setupapi.dll")]
		public static extern int SetupDiLoadClassIcon(ref Guid classGuid, out IntPtr hIcon, out int index);

		// Token: 0x06000140 RID: 320 RVA: 0x0000EAB8 File Offset: 0x0000CCB8
		public static int EnumerateClasses(uint ClassIndex, StringBuilder ClassName, StringBuilder ClassDescription, ref bool DevicePresent)
		{
			Guid ClassGuid = Guid.Empty;
			DeviceClasses.SP_DEVINFO_DATA DeviceInfoData = new DeviceClasses.SP_DEVINFO_DATA();
			uint RequiredSize = 0u;
			uint result = DeviceClasses.CM_Enumerate_Classes(ClassIndex, ref ClassGuid, 0u);
			DevicePresent = false;
			new DeviceClasses.SP_CLASSIMAGELIST_DATA();
			if (result != 0u)
			{
				return (int)result;
			}
			DeviceClasses.SetupDiClassNameFromGuidA(ref ClassGuid, ClassName, RequiredSize, ref RequiredSize);
			if (RequiredSize > 0u)
			{
				ClassName.Capacity = (int)RequiredSize;
				DeviceClasses.SetupDiClassNameFromGuidA(ref ClassGuid, ClassName, RequiredSize, ref RequiredSize);
			}
			IntPtr NewDeviceInfoSet = DeviceClasses.SetupDiGetClassDevs(ref ClassGuid, 0u, IntPtr.Zero, 2u);
			if (NewDeviceInfoSet.ToInt32() == -1)
			{
				DevicePresent = false;
				return 0;
			}
			uint numD = 0u;
			DeviceInfoData.cbSize = 28;
			DeviceInfoData.DevInst = 0;
			DeviceInfoData.ClassGuid = Guid.Empty;
			DeviceInfoData.Reserved = 0UL;
			if (!DeviceClasses.SetupDiEnumDeviceInfo(NewDeviceInfoSet, numD, DeviceInfoData))
			{
				DevicePresent = false;
				return 0;
			}
			DeviceClasses.SetupDiDestroyDeviceInfoList(NewDeviceInfoSet);
			IntPtr KeyClass = DeviceClasses.SetupDiOpenClassRegKeyExA(ref ClassGuid, 33554432u, 1, IntPtr.Zero, 0u);
			if (KeyClass.ToInt32() == -1)
			{
				DevicePresent = false;
				return 0;
			}
			uint sizeB = 1000u;
			ClassDescription.Capacity = 1000;
			uint res = DeviceClasses.RegQueryValueA(KeyClass, 0u, ClassDescription, ref sizeB);
			if (res != 0u)
			{
				ClassDescription = new StringBuilder("");
			}
			DevicePresent = true;
			DeviceClasses.ClassesGuid = DeviceInfoData.ClassGuid;
			return 0;
		}

		// Token: 0x040000E8 RID: 232
		public const int MAX_SIZE_DEVICE_DESCRIPTION = 1000;

		// Token: 0x040000E9 RID: 233
		public const int CR_SUCCESS = 0;

		// Token: 0x040000EA RID: 234
		public const int CR_NO_SUCH_VALUE = 37;

		// Token: 0x040000EB RID: 235
		public const int CR_INVALID_DATA = 31;

		// Token: 0x040000EC RID: 236
		private const int DIGCF_PRESENT = 2;

		// Token: 0x040000ED RID: 237
		private const int DIOCR_INSTALLER = 1;

		// Token: 0x040000EE RID: 238
		private const int MAXIMUM_ALLOWED = 33554432;

		// Token: 0x040000EF RID: 239
		public const int DMI_MASK = 1;

		// Token: 0x040000F0 RID: 240
		public const int DMI_BKCOLOR = 2;

		// Token: 0x040000F1 RID: 241
		public const int DMI_USERECT = 4;

		// Token: 0x040000F2 RID: 242
		public const int DIGCF_DEFAULT = 1;

		// Token: 0x040000F3 RID: 243
		public const int DIGCF_ALLCLASSES = 4;

		// Token: 0x040000F4 RID: 244
		public const int DIGCF_PROFILE = 8;

		// Token: 0x040000F5 RID: 245
		public const int DIGCF_DEVICEINTERFACE = 16;

		// Token: 0x040000F6 RID: 246
		public static Guid ClassesGuid;

		// Token: 0x0200002F RID: 47
		[StructLayout(LayoutKind.Sequential)]
		private class SP_DEVINFO_DATA
		{
			// Token: 0x040000F7 RID: 247
			public int cbSize;

			// Token: 0x040000F8 RID: 248
			public Guid ClassGuid;

			// Token: 0x040000F9 RID: 249
			public int DevInst;

			// Token: 0x040000FA RID: 250
			public ulong Reserved;
		}

		// Token: 0x02000030 RID: 48
		[StructLayout(LayoutKind.Sequential)]
		public class SP_CLASSIMAGELIST_DATA
		{
			// Token: 0x040000FB RID: 251
			public int cbSize;

			// Token: 0x040000FC RID: 252
			public ImageList ImageList;

			// Token: 0x040000FD RID: 253
			public ulong Reserved;
		}

		// Token: 0x02000031 RID: 49
		public struct RECT
		{
			// Token: 0x040000FE RID: 254
			private long left;

			// Token: 0x040000FF RID: 255
			private long top;

			// Token: 0x04000100 RID: 256
			private long right;

			// Token: 0x04000101 RID: 257
			private long bottom;
		}
	}
}

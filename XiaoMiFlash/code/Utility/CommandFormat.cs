using System;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000015 RID: 21
	public class CommandFormat
	{
		// Token: 0x060000BE RID: 190 RVA: 0x0000B96C File Offset: 0x00009B6C
		public static byte[] StructToBytes(object structObj)
		{
			int size = 48;
			byte[] bytes = new byte[size];
			IntPtr structPtr = Marshal.AllocHGlobal(size);
			Marshal.StructureToPtr(structObj, structPtr, false);
			Marshal.Copy(structPtr, bytes, 0, size);
			Marshal.FreeHGlobal(structPtr);
			return bytes;
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000B9A4 File Offset: 0x00009BA4
		public static byte[] StructToBytes(object structObj, int length)
		{
			byte[] bytes = new byte[length];
			IntPtr structPtr = Marshal.AllocHGlobal(length);
			Marshal.StructureToPtr(structObj, structPtr, false);
			Marshal.Copy(structPtr, bytes, 0, length);
			Marshal.FreeHGlobal(structPtr);
			return bytes;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000B9DC File Offset: 0x00009BDC
		public static object BytesToStuct(byte[] bytes, Type type)
		{
			int size = Marshal.SizeOf(type);
			if (size > bytes.Length)
			{
				return null;
			}
			IntPtr structPtr = Marshal.AllocHGlobal(size);
			Marshal.Copy(bytes, 0, structPtr, size);
			object obj = Marshal.PtrToStructure(structPtr, type);
			Marshal.FreeHGlobal(structPtr);
			return obj;
		}
	}
}

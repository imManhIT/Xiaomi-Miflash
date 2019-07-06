using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000028 RID: 40
	public class ShareMemory
	{
		// Token: 0x06000112 RID: 274
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

		// Token: 0x06000113 RID: 275
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);

		// Token: 0x06000114 RID: 276
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr OpenFileMapping(int dwDesiredAccess, [MarshalAs(2)] bool bInheritHandle, string lpName);

		// Token: 0x06000115 RID: 277
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr MapViewOfFile(IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

		// Token: 0x06000116 RID: 278
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);

		// Token: 0x06000117 RID: 279
		[DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06000118 RID: 280
		[DllImport("kernel32")]
		public static extern int GetLastError();

		// Token: 0x06000119 RID: 281
		[DllImport("kernel32.dll")]
		private static extern void GetSystemInfo(out ShareMemory.SYSTEM_INFO lpSystemInfo);

		// Token: 0x0600011A RID: 282 RVA: 0x0000DF30 File Offset: 0x0000C130
		public static uint GetPartitionsize()
		{
			ShareMemory.SYSTEM_INFO sysInfo;
			ShareMemory.GetSystemInfo(out sysInfo);
			return sysInfo.allocationGranularity;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000DF4C File Offset: 0x0000C14C
		public ShareMemory(string filename, uint memSize)
		{
			this.m_MemSize = memSize;
			this.Init(filename);
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000DF9C File Offset: 0x0000C19C
		public ShareMemory(string filename)
		{
			this.m_MemSize = 20971520u;
			this.Init(filename);
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000DFF0 File Offset: 0x0000C1F0
		~ShareMemory()
		{
			this.Close();
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000E01C File Offset: 0x0000C21C
		protected void Init(string strName)
		{
			if (!System.IO.File.Exists(strName))
			{
				throw new Exception("未找到文件");
			}
			FileInfo f = new FileInfo(strName);
			this.m_FileSize = f.Length;
			this.m_fHandle = this.File.Open(strName);
			if (strName.Length > 0)
			{
				this.m_hSharedMemoryFile = ShareMemory.CreateFileMapping(this.m_fHandle, IntPtr.Zero, 2u, 0u, (uint)this.m_FileSize, "mdata");
				if (this.m_hSharedMemoryFile == IntPtr.Zero)
				{
					this.m_bAlreadyExist = false;
					this.m_bInit = false;
					throw new Exception("CreateFileMapping失败LastError=" + ShareMemory.GetLastError().ToString());
				}
				this.m_bInit = true;
				if ((ulong)this.m_MemSize > (ulong)this.m_FileSize)
				{
					this.m_MemSize = (uint)this.m_FileSize;
				}
				this.m_pwData = ShareMemory.MapViewOfFile(this.m_hSharedMemoryFile, 4u, 0u, 0u, this.m_MemSize);
				if (this.m_pwData == IntPtr.Zero)
				{
					this.m_bInit = false;
					throw new Exception("m_hSharedMemoryFile失败LastError=" + ShareMemory.GetLastError().ToString() + "  " + new Win32Exception(ShareMemory.GetLastError()).Message);
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000E155 File Offset: 0x0000C355
		private static uint GetHighWord(ulong intValue)
		{
			return Convert.ToUInt32(intValue >> 32);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000E160 File Offset: 0x0000C360
		private static uint GetLowWord(ulong intValue)
		{
			return Convert.ToUInt32(intValue & (ulong)-1);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000E16C File Offset: 0x0000C36C
		public uint GetNextblock()
		{
			if (!this.m_bInit)
			{
				throw new Exception("文件未初始化。");
			}
			uint m_Size = this.GetMemberSize();
			if (m_Size == 0u)
			{
				return m_Size;
			}
			this.m_MemSize = m_Size;
			this.m_pwData = ShareMemory.MapViewOfFile(this.m_hSharedMemoryFile, 4u, ShareMemory.GetHighWord((ulong)this.m_offsetBegin), ShareMemory.GetLowWord((ulong)this.m_offsetBegin), m_Size);
			if (this.m_pwData == IntPtr.Zero)
			{
				this.m_bInit = false;
				throw new Exception("映射文件块失败" + ShareMemory.GetLastError().ToString());
			}
			this.m_offsetBegin += (long)((ulong)m_Size);
			return m_Size;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000E210 File Offset: 0x0000C410
		private uint GetMemberSize()
		{
			if (this.m_offsetBegin >= this.m_FileSize)
			{
				return 0u;
			}
			if (this.m_offsetBegin + (long)((ulong)this.m_MemSize) >= this.m_FileSize)
			{
				long temp = this.m_FileSize - this.m_offsetBegin;
				return (uint)temp;
			}
			return this.m_MemSize;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000E25A File Offset: 0x0000C45A
		public void Close()
		{
			if (this.m_bInit)
			{
				ShareMemory.UnmapViewOfFile(this.m_pwData);
				ShareMemory.CloseHandle(this.m_hSharedMemoryFile);
				this.File.Close();
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000E288 File Offset: 0x0000C488
		public void Read(ref byte[] bytData, int lngAddr, int lngSize, bool Unmap)
		{
			if ((long)(lngAddr + lngSize) > (long)((ulong)this.m_MemSize))
			{
				throw new Exception("Read操作超出数据区");
			}
			if (this.m_bInit)
			{
				Marshal.Copy(this.m_pwData, bytData, lngAddr, lngSize);
				if (Unmap)
				{
					bool l_result = ShareMemory.UnmapViewOfFile(this.m_pwData);
					if (l_result)
					{
						this.m_pwData = IntPtr.Zero;
					}
				}
				return;
			}
			throw new Exception("文件未初始化");
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000E2EF File Offset: 0x0000C4EF
		public void Read(ref byte[] bytData, int lngAddr, int lngSize)
		{
			if ((long)(lngAddr + lngSize) > (long)((ulong)this.m_MemSize))
			{
				throw new Exception("Read操作超出数据区");
			}
			if (this.m_bInit)
			{
				Marshal.Copy(this.m_pwData, bytData, lngAddr, lngSize);
				return;
			}
			throw new Exception("文件未初始化");
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000E32C File Offset: 0x0000C52C
		public uint ReadBytes(int lngAddr, ref byte[] byteData, int StartIndex, uint intSize)
		{
			if ((long)lngAddr >= (long)((ulong)this.m_MemSize))
			{
				throw new Exception("起始数据超过缓冲区长度");
			}
			if ((long)lngAddr + (long)((ulong)intSize) > (long)((ulong)this.m_MemSize))
			{
				intSize = this.m_MemSize - (uint)lngAddr;
			}
			if (this.m_bInit)
			{
				IntPtr s;
				s..ctor((long)this.m_pwData + (long)lngAddr);
				Marshal.Copy(s, byteData, StartIndex, (int)intSize);
				return intSize;
			}
			throw new Exception("文件未初始化");
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000E39F File Offset: 0x0000C59F
		private int Write(byte[] bytData, int lngAddr, int lngSize)
		{
			if ((long)(lngAddr + lngSize) > (long)((ulong)this.m_MemSize))
			{
				return 2;
			}
			if (this.m_bInit)
			{
				Marshal.Copy(bytData, lngAddr, this.m_pwData, lngSize);
				return 0;
			}
			return 1;
		}

		// Token: 0x040000C2 RID: 194
		private const int ERROR_ALREADY_EXISTS = 183;

		// Token: 0x040000C3 RID: 195
		private const int FILE_MAP_COPY = 1;

		// Token: 0x040000C4 RID: 196
		private const int FILE_MAP_WRITE = 2;

		// Token: 0x040000C5 RID: 197
		private const int FILE_MAP_READ = 4;

		// Token: 0x040000C6 RID: 198
		private const int FILE_MAP_ALL_ACCESS = 6;

		// Token: 0x040000C7 RID: 199
		private const int PAGE_READONLY = 2;

		// Token: 0x040000C8 RID: 200
		private const int PAGE_READWRITE = 4;

		// Token: 0x040000C9 RID: 201
		private const int PAGE_WRITECOPY = 8;

		// Token: 0x040000CA RID: 202
		private const int PAGE_EXECUTE = 16;

		// Token: 0x040000CB RID: 203
		private const int PAGE_EXECUTE_READ = 32;

		// Token: 0x040000CC RID: 204
		private const int PAGE_EXECUTE_READWRITE = 64;

		// Token: 0x040000CD RID: 205
		private const int SEC_COMMIT = 134217728;

		// Token: 0x040000CE RID: 206
		private const int SEC_IMAGE = 16777216;

		// Token: 0x040000CF RID: 207
		private const int SEC_NOCACHE = 268435456;

		// Token: 0x040000D0 RID: 208
		private const int SEC_RESERVE = 67108864;

		// Token: 0x040000D1 RID: 209
		private IntPtr m_fHandle;

		// Token: 0x040000D2 RID: 210
		private IntPtr m_hSharedMemoryFile = IntPtr.Zero;

		// Token: 0x040000D3 RID: 211
		private IntPtr m_pwData = IntPtr.Zero;

		// Token: 0x040000D4 RID: 212
		private bool m_bAlreadyExist;

		// Token: 0x040000D5 RID: 213
		private bool m_bInit;

		// Token: 0x040000D6 RID: 214
		private uint m_MemSize = 20971520u;

		// Token: 0x040000D7 RID: 215
		private long m_offsetBegin;

		// Token: 0x040000D8 RID: 216
		public long m_FileSize;

		// Token: 0x040000D9 RID: 217
		private FileReader File = new FileReader();

		// Token: 0x02000029 RID: 41
		public struct SYSTEM_INFO
		{
			// Token: 0x040000DA RID: 218
			public ushort processorArchitecture;

			// Token: 0x040000DB RID: 219
			private ushort reserved;

			// Token: 0x040000DC RID: 220
			public uint pageSize;

			// Token: 0x040000DD RID: 221
			public IntPtr minimumApplicationAddress;

			// Token: 0x040000DE RID: 222
			public IntPtr maximumApplicationAddress;

			// Token: 0x040000DF RID: 223
			public IntPtr activeProcessorMask;

			// Token: 0x040000E0 RID: 224
			public uint numberOfProcessors;

			// Token: 0x040000E1 RID: 225
			public uint processorType;

			// Token: 0x040000E2 RID: 226
			public uint allocationGranularity;

			// Token: 0x040000E3 RID: 227
			public ushort processorLevel;

			// Token: 0x040000E4 RID: 228
			public ushort processorRevision;
		}
	}
}

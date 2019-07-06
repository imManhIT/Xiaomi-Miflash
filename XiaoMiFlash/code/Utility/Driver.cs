using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000058 RID: 88
	public class Driver
	{
		// Token: 0x060001D1 RID: 465
		[DllImport("setupapi.dll", SetLastError = true)]
		private static extern bool SetupCopyOEMInf(string SourceInfFileName, string OEMSourceMediaLocation, OemSourceMediaType OEMSourceMediaType, OemCopyStyle CopyStyle, StringBuilder DestinationInfFileName, int DestinationInfFileNameSize, int RequiredSize, out string DestinationInfFileNameComponent);

		// Token: 0x060001D2 RID: 466
		[DllImport("setupapi.dll", SetLastError = true)]
		private static extern bool SetupUninstallOEMInf(string InfFileName, SetupUOInfFlags Flags, IntPtr Reserved);

		// Token: 0x060001D3 RID: 467
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint GetWindowsDirectory(StringBuilder path, int pathLen);

		// Token: 0x060001D4 RID: 468 RVA: 0x00011C34 File Offset: 0x0000FE34
		public static string SetupOEMInf(string infPath, out string destinationInfFileName, out string destinationInfFileNameComponent, out bool success)
		{
			string msg = "";
			StringBuilder destinationInfFileName_builder = new StringBuilder(260);
			success = Driver.SetupCopyOEMInf(infPath, "", OemSourceMediaType.SPOST_PATH, OemCopyStyle.SP_COPY_NEWER, destinationInfFileName_builder, destinationInfFileName_builder.Capacity, 0, out destinationInfFileNameComponent);
			if (!success)
			{
				int errorCode = Marshal.GetLastWin32Error();
				string errorString = new Win32Exception(errorCode).Message;
				msg = errorString;
			}
			destinationInfFileName = destinationInfFileName_builder.ToString();
			return msg;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00011C8C File Offset: 0x0000FE8C
		public static string UninstallInf(string infFileName, out bool success)
		{
			string msg = "";
			success = Driver.SetupUninstallOEMInf(infFileName, SetupUOInfFlags.SUOI_FORCEDELETE, IntPtr.Zero);
			if (!success)
			{
				int errorCode = Marshal.GetLastWin32Error();
				string errorString = new Win32Exception(errorCode).Message;
				msg = errorString;
			}
			return msg;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00011CC8 File Offset: 0x0000FEC8
		public static string UninstallInfByText(string text, out bool success)
		{
			success = false;
			StringBuilder winDir = new StringBuilder(256);
			if (Driver.GetWindowsDirectory(winDir, winDir.Capacity) == 0u)
			{
				return "UninstallInfByText: GetWindowsDirectory failed with system error code " + Marshal.GetLastWin32Error().ToString();
			}
			string infDir = winDir.ToString() + "\\inf";
			string[] infFiles = Directory.GetFiles(infDir, "*.inf");
			string retval = "";
			foreach (string infFile in infFiles)
			{
				string inf = File.ReadAllText(infFile);
				if (inf.Contains(text))
				{
					string infFileName = infFile.Remove(0, infFile.LastIndexOf('\\') + 1);
					if (!Driver.SetupUninstallOEMInf(infFileName, SetupUOInfFlags.SUOI_FORCEDELETE, IntPtr.Zero))
					{
						string text2 = retval;
						retval = string.Concat(new string[]
						{
							text2,
							"UninstallInfByText: SetupUninstallOEMInf failed with code ",
							Marshal.GetLastWin32Error().ToString(),
							" for file ",
							infFileName
						});
					}
					else
					{
						success = true;
					}
				}
			}
			if (retval.Length > 0)
			{
				return retval;
			}
			return null;
		}
	}
}

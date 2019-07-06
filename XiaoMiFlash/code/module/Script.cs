using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000038 RID: 56
	public class Script
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000173 RID: 371 RVA: 0x0000FF5D File Offset: 0x0000E15D
		public static string AndroidPath
		{
			get
			{
				return "\"" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Source\\ThirdParty\\Google\\Android\"";
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000174 RID: 372 RVA: 0x0000FF7D File Offset: 0x0000E17D
		public static string fastboot
		{
			get
			{
				return "\"" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Source\\ThirdParty\\Google\\Android\\fastboot.exe\"";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000FF9D File Offset: 0x0000E19D
		public static string QcLsUsb
		{
			get
			{
				return "\"" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Source\\ThirdParty\\Qualcomm\\fh_loader\\lsusb.exe\"";
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000FFBD File Offset: 0x0000E1BD
		public static string SP_Download_Tool_PATH
		{
			get
			{
				return "\"" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SP_Download_tool\"";
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000FFDD File Offset: 0x0000E1DD
		public static string LKMSG_FASTBOOT
		{
			get
			{
				return "\"" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Source\\ThirdParty\\Mi\\fastboot.exe\"";
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000FFFD File Offset: 0x0000E1FD
		public static string qcCoInstaller
		{
			get
			{
				return "\"" + AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Source\\ThirdParty\\Mi\\qcCoInstaller.dll\"";
			}
		}
	}
}

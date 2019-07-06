using System;
using System.IO;
using Microsoft.Win32;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x0200005E RID: 94
	public class MiDriver
	{
		// Token: 0x060001F7 RID: 503 RVA: 0x000129B4 File Offset: 0x00010BB4
		public void CopyFiles(string installationPath)
		{
			installationPath = installationPath.Substring(0, installationPath.LastIndexOf('\\') + 1);
			try
			{
				string sysPath = Environment.SystemDirectory;
				string[] files = new string[]
				{
					"Qualcomm\\Driver\\serial\\i386\\qcCoInstaller.dll"
				};
				sysPath + "\\qcCoInstaller.dll";
				installationPath + "Source\\ThirdParty\\";
				for (int i = 0; i < files.Length; i++)
				{
				}
			}
			catch (Exception ex)
			{
				Log.Installw(installationPath, string.Format("copy file failed,{0}", ex.Message));
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00012A40 File Offset: 0x00010C40
		public void InstallAllDriver(string installationPath, bool uninstallOld)
		{
			installationPath = installationPath.Substring(0, installationPath.LastIndexOf('\\') + 1);
			string basePath = installationPath + "Source\\ThirdParty\\";
			DirectoryInfo dic = new DirectoryInfo(basePath);
			if (dic.Exists)
			{
				for (int i = 0; i < this.infFiles.Length; i++)
				{
					this.InstallDriver(basePath + this.infFiles[i], installationPath, uninstallOld);
				}
				return;
			}
			Log.Installw(installationPath, "dic " + basePath + " not exists.");
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00012ABC File Offset: 0x00010CBC
		public void InstallDriver(string infPath, string installationPath, bool uninstallOld)
		{
			try
			{
				string REG_MIFLASH = "Software\\XiaoMi\\MiFlash\\";
				FileInfo file = new FileInfo(infPath);
				RegistryKey key = Registry.LocalMachine;
				RegistryKey myreg = key.OpenSubKey(REG_MIFLASH, true);
				Log.Installw(installationPath, string.Format("open RegistryKey {0}", REG_MIFLASH));
				if (myreg == null)
				{
					myreg = key.CreateSubKey(REG_MIFLASH, 2);
					Log.Installw(installationPath, string.Format("create RegistryKey {0}", REG_MIFLASH));
				}
				myreg.GetValueNames();
				object info = myreg.GetValue(file.Name);
				bool success = true;
				string msg;
				if (info != null && uninstallOld)
				{
					msg = Driver.UninstallInf(info.ToString(), out success);
					Log.Installw(installationPath, string.Format("driver {0} exists,uninstall,reuslt {1},GetLastWin32Error{2}", info.ToString(), success.ToString(), msg));
				}
				string destinationInfFileNameComponent = "";
				string destinationInfFileName = "";
				msg = Driver.SetupOEMInf(file.FullName, out destinationInfFileName, out destinationInfFileNameComponent, out success);
				Log.Installw(installationPath, string.Format("install driver {0} to {1},result {2},GetLastWin32Error {3}", new object[]
				{
					file.FullName,
					destinationInfFileName,
					success.ToString(),
					msg
				}));
				if (success)
				{
					myreg.SetValue(file.Name, destinationInfFileNameComponent);
					Log.Installw(installationPath, string.Format("set RegistryKey value:{0}--{1}", file.Name, destinationInfFileNameComponent));
				}
				myreg.Close();
				if (infPath.IndexOf("android_winusb.inf") >= 0)
				{
					string uerProfilePath = Environment.GetEnvironmentVariable("USERPROFILE");
					Cmd cmd = new Cmd("", "");
					string cmdStr = string.Format("mkdir \"{0}\\.android\"", uerProfilePath);
					string output = cmd.Execute(null, cmdStr);
					Log.Installw(installationPath, cmdStr);
					Log.Installw(installationPath, "output:" + output);
					cmdStr = string.Format(" echo 0x2717 >>\"{0}\\.android\\adb_usb.ini\"", uerProfilePath);
					output = cmd.Execute(null, cmdStr);
					Log.Installw(installationPath, cmdStr);
					Log.Installw(installationPath, "output:" + output);
				}
			}
			catch (Exception ex)
			{
				Log.Installw(installationPath, string.Format("install driver {0}, exception:{1}", infPath, ex.Message));
			}
		}

		// Token: 0x040001D9 RID: 473
		public string[] infFiles = new string[]
		{
			"Google\\Driver\\android_winusb.inf",
			"Nvidia\\Driver\\NvidiaUsb.inf",
			"Microsoft\\Driver\\tetherxp.inf",
			"Microsoft\\Driver\\wpdmtphw.inf",
			"Qualcomm\\Driver\\qcser.inf"
		};
	}
}

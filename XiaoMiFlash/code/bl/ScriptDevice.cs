using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x02000018 RID: 24
	public class ScriptDevice : DeviceCtrl
	{
		// Token: 0x060000D2 RID: 210 RVA: 0x0000BC44 File Offset: 0x00009E44
		public override void flash()
		{
			try
			{
				string fastboot = Script.fastboot;
				string[] searchFiles = FileSearcher.SearchFiles(this.swPath, this.flashScript);
				if (searchFiles.Length == 0)
				{
					throw new Exception("can not found file " + this.flashScript);
				}
				string scriptFile = searchFiles[0];
				string cmdPattern = "pushd \"{0}\"&&prompt $$&&set PATH=\"{1}\";%PATH%&&\"{2}\" -s {3}&&popd";
				string cmdStr = string.Format(cmdPattern, new object[]
				{
					this.swPath,
					Script.AndroidPath,
					scriptFile,
					this.deviceName
				});
				Log.w(this.deviceName, "image path:" + this.swPath);
				Log.w(this.deviceName, "env android path:" + Script.AndroidPath);
				Log.w(this.deviceName, "script :" + scriptFile);
				Cmd cmd = new Cmd(this.deviceName, scriptFile);
				if (Convert.ToInt32(this.idproduct).ToString("x4") == "0a65" && Convert.ToInt32(this.idvendor).ToString("x4") == "8087")
				{
					string msg = "DNX fastboot mode";
					Log.w(this.deviceName, msg);
					FlashingDevice.UpdateDeviceStatus(this.deviceName, default(float?), msg, "boot into kernelflinger", false);
					string loaderPath = this.swPath + "\\images\\loader.efi";
					if (File.Exists(loaderPath))
					{
						msg = "Boot into kernelflinger " + loaderPath;
						Log.w(this.deviceName, msg);
						FlashingDevice.UpdateDeviceStatus(this.deviceName, default(float?), msg, msg, false);
						string loaderPattern = "pushd \"{0}\"&&prompt $$&&set PATH=\"{1}\";%PATH%&&fastboot boot \"{2}\" -s {3}&&popd";
						string loaderCmdStr = string.Format(loaderPattern, new object[]
						{
							this.swPath + "\\images",
							Script.AndroidPath,
							loaderPath,
							this.deviceName
						});
						string output = cmd.Execute(this.deviceName, loaderCmdStr);
						Log.w(this.deviceName, output);
						int count = 4;
						bool found = false;
						while (count-- >= 0 && !found)
						{
							List<string> fastbootDevices = Enumerable.ToList<string>(UsbDevice.GetScriptDevice());
							if (fastbootDevices.IndexOf(this.deviceName) >= 0)
							{
								found = true;
								break;
							}
							Thread.Sleep(1000);
						}
						if (!found)
						{
							Log.w(this.deviceName, "error:device couldn't boot up ");
						}
					}
					else
					{
						Log.w(this.deviceName, "error:couldn't find " + this.swPath + "\\loader.efi");
					}
				}
				cmd.Execute_returnLine(this.deviceName, cmdStr, 1);
			}
			catch (Exception ex)
			{
				FlashingDevice.UpdateDeviceStatus(this.deviceName, default(float?), ex.Message, "error", true);
				Log.w(this.deviceName, ex, false);
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000BF34 File Offset: 0x0000A134
		public void GrapLog()
		{
			string path = "C:\\fastbootlog";
			if (!Directory.Exists(path.Replace("\"", "")))
			{
				Directory.CreateDirectory(path);
			}
			Cmd cmd = new Cmd(this.deviceName, "");
			string logType = "lkmsg";
			FlashingDevice.UpdateDeviceStatus(this.deviceName, default(float?), string.Format("start grab {0} log", logType), "grabbing log", false);
			string logName = string.Format("{0}_{1}@{2}.txt", this.deviceName, logType, DateTime.Now.ToString("yyyyMdHms"));
			string logPath = path + "\\" + logName;
			string command = string.Format("\"{0}\" -s {1} oem lkmsg > {2}", Script.LKMSG_FASTBOOT, this.deviceName, logPath);
			cmd.Execute(this.deviceName, command);
			FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(1f), string.Format("grab done", logType), "grabbing log", false);
			logType = "lpmsg";
			FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(0f), string.Format("start grab {0} log", logType), "grabbing log", false);
			logName = string.Format("{0}_{1}@{2}.txt", this.deviceName, logType, DateTime.Now.ToString("yyyyMdHms"));
			logPath = path + "\\" + logName;
			command = string.Format("\"{0}\" -s {1} oem lpmsg > {2}", Script.LKMSG_FASTBOOT, this.deviceName, logPath);
			cmd.Execute(this.deviceName, command);
			FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(1f), "flash done", "grab done", true);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000C0CD File Offset: 0x0000A2CD
		public override string[] getDevice()
		{
			return UsbDevice.GetScriptDevice();
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public override void CheckSha256()
		{
		}
	}
}

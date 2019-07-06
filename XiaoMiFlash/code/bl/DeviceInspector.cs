using System;
using System.Text.RegularExpressions;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x02000007 RID: 7
	public class DeviceInspector
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002A80 File Offset: 0x00000C80
		public static bool checkLockStatus(string device, out string msg)
		{
			msg = "";
			Cmd cmd = new Cmd(device, "");
			string cmdStr = string.Format("fastboot -s {0} getvar token", device);
			string result = cmd.Execute(device, cmdStr);
			string[] arr = Regex.Split(result, "\\r\\n");
			foreach (string item in arr)
			{
				if (!string.IsNullOrEmpty(item))
				{
					string tag = "token:";
					if (item.IndexOf("token:") >= 0)
					{
						if (item.Trim() == tag)
						{
							msg = "token is null.";
							return false;
						}
						break;
					}
				}
			}
			cmdStr = string.Format("fastboot -s {0} oem device-info", device);
			result = cmd.Execute(device, cmdStr);
			string[] lockTag = new string[]
			{
				"Device unlocked: false",
				" Device critical unlocked: false"
			};
			bool isLock = true;
			foreach (string item2 in lockTag)
			{
				isLock = (isLock && result.IndexOf(item2) >= 0);
				if (!isLock)
				{
					break;
				}
			}
			if (!isLock)
			{
				msg = "device is unlocked!";
			}
			return isLock;
		}
	}
}

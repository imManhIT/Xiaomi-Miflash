using System;
using System.IO;
using System.Text;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.module;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000004 RID: 4
	public class Log
	{
		// Token: 0x0600000D RID: 13 RVA: 0x000023EC File Offset: 0x000005EC
		public static void w(string deviceName, Exception ex, bool stopFlash)
		{
			string msg = ex.Message;
			string stackTrace = ex.StackTrace;
			if (stopFlash)
			{
				msg = "error:" + msg;
				stackTrace = "error" + stackTrace;
			}
			Log.w(deviceName, msg, stopFlash);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000242A File Offset: 0x0000062A
		public static void w(string deviceName, string msg)
		{
			Log.w(deviceName, msg, true);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002434 File Offset: 0x00000634
		public static void w(string deviceName, string msg, bool throwEx)
		{
			if (string.IsNullOrEmpty(deviceName))
			{
				Log.w(msg);
				return;
			}
			string logName = "";
			foreach (Device d in FlashingDevice.flashDeviceList)
			{
				if (d.Name == deviceName)
				{
					logName = string.Format("{0}@{1}.txt", d.Name, d.StartTime.ToString("yyyyMdHms"));
					break;
				}
			}
			if (string.IsNullOrEmpty(logName))
			{
				Log.w(msg, true);
				return;
			}
			string logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\" + logName;
			try
			{
				if (!File.Exists(logPath))
				{
					File.Create(logPath).Close();
				}
			}
			catch (Exception)
			{
				Log.w(msg);
				return;
			}
			string timeStr = DateTime.Now.ToLongTimeString();
			msg = string.Format("[{0}  {1}]:{2}", timeStr, deviceName, msg);
			Log.WriteFile(msg, logPath);
			if (msg.ToLower().IndexOf("error") >= 0 || msg.ToLower().IndexOf("fail") >= 0 || msg.ToLower().IndexOf("找不到批处理文件") >= 0)
			{
				FlashingDevice.UpdateDeviceStatus(deviceName, default(float?), msg, "error", true);
				if (throwEx)
				{
					throw new Exception(msg);
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000025A0 File Offset: 0x000007A0
		public static void w(string msg)
		{
			Log.w(msg, false);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025AC File Offset: 0x000007AC
		public static void w(string msg, bool throwEx)
		{
			string logName = string.Format("{0}@{1}.txt", "miflash", DateTime.Now.ToString("yyyyMd"));
			string logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\" + logName;
			if (!File.Exists(logPath))
			{
				File.Create(logPath).Close();
			}
			msg = string.Format("[{0}]:{1}", DateTime.Now.ToLongTimeString(), msg);
			Log.WriteFile(msg, logPath);
			if ((msg.ToLower().IndexOf("error") >= 0 || msg.ToLower().IndexOf("fail") >= 0 || msg.ToLower().IndexOf("找不到批处理文件") >= 0) && throwEx)
			{
				throw new Exception(msg);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002674 File Offset: 0x00000874
		public static void wFlashStatus(string msg)
		{
			try
			{
				string logName = string.Format("{0}-Result@{1}.txt", "MiFlash", DateTime.Now.ToString("yyyyMd"));
				string logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\" + logName;
				if (!File.Exists(logPath))
				{
					File.Create(logPath).Close();
				}
				FileStream fs = new FileStream(logPath, 6, 2, 3);
				StreamWriter sw = new StreamWriter(fs, Encoding.Default);
				sw.WriteLine(string.Format("[{0}]:{1}", DateTime.Now.ToLongTimeString(), msg));
				sw.Close();
			}
			catch (Exception ex)
			{
				Log.w(string.Format("wFlashStatus {0}  {1} {2}", msg, ex.Message, ex.StackTrace));
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002748 File Offset: 0x00000948
		public static void Installw(string installationPath, string msg)
		{
			string logName = string.Format("{0}@{1}.txt", "miflash", DateTime.Now.ToString("yyyyMd"));
			string logPath = installationPath + "log\\" + logName;
			if (!File.Exists(logPath))
			{
				File.Create(logPath).Close();
			}
			FileStream fs = new FileStream(logPath, 6, 2, 3);
			StreamWriter sw = new StreamWriter(fs, Encoding.Default);
			sw.WriteLine(string.Format("[{0}]:{1}", DateTime.Now.ToLongTimeString(), msg));
			sw.Close();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027DC File Offset: 0x000009DC
		public static void LogMsg(string deviceName, string msg, string suffix)
		{
			string logName = string.Format("{0}_{1}@{2}.txt", deviceName, suffix, DateTime.Now.ToString("yyyyMdHms"));
			string logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\" + logName;
			try
			{
				if (!File.Exists(logPath))
				{
					File.Create(logPath).Close();
				}
			}
			catch (Exception)
			{
				Log.w(msg);
				return;
			}
			Log.WriteFile(msg, logPath);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002858 File Offset: 0x00000A58
		public static void WriteFile(string log, string path)
		{
			lock (Log._lock)
			{
				if (!File.Exists(path))
				{
					using (new FileStream(path, 2))
					{
					}
				}
				using (FileStream fs2 = new FileStream(path, 6, 2))
				{
					using (StreamWriter sw = new StreamWriter(fs2))
					{
						string value = log.ToString();
						sw.WriteLine(value);
						sw.Flush();
					}
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002908 File Offset: 0x00000B08
		public static void WriteLog(string log, string logPath)
		{
			Log.WriteFile(log, logPath);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002911 File Offset: 0x00000B11
		public static void WriteErrorLog(string log, string logPath)
		{
			Log.WriteFile(log, logPath);
		}

		// Token: 0x04000006 RID: 6
		public static readonly object _lock = new object();
	}
}

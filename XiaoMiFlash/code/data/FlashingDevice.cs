using System;
using System.Collections.Generic;
using System.Linq;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.data
{
	// Token: 0x0200000B RID: 11
	public static class FlashingDevice
	{
		// Token: 0x06000039 RID: 57 RVA: 0x0000322C File Offset: 0x0000142C
		public static void UpdateDeviceStatus(string deviceName, float? progress, string status, string result, bool isDone)
		{
			try
			{
				foreach (Device item in FlashingDevice.flashDeviceList)
				{
					if (item.Name == deviceName)
					{
						if (!string.IsNullOrEmpty(status))
						{
							item.StatusList.Add(status);
						}
						if (progress != null)
						{
							item.Progress = progress.Value;
						}
						if (!string.IsNullOrEmpty(status))
						{
							item.Status = status;
						}
						if (!string.IsNullOrEmpty(result))
						{
							item.Result = result;
						}
						item.IsDone = new bool?(isDone);
						break;
					}
				}
			}
			catch (Exception ex)
			{
				Log.w(ex.Message + "\r\n" + ex.StackTrace);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003354 File Offset: 0x00001554
		public static List<Device> GetFlashDoneDs()
		{
			IEnumerable<Device> dr = Enumerable.Where<Device>(FlashingDevice.flashDeviceList, (Device d) => d.IsDone == null || (d.IsDone != null && d.IsDone.Value && !d.IsUpdate));
			return Enumerable.ToList<Device>(dr);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003390 File Offset: 0x00001590
		public static bool IsAllDone()
		{
			bool result = false;
			if (FlashingDevice.GetFlashDoneDs().Count == FlashingDevice.flashDeviceList.Count)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x04000014 RID: 20
		public static bool consoleMode_UsbInserted = false;

		// Token: 0x04000015 RID: 21
		public static List<Device> flashDeviceList = new List<Device>();
	}
}

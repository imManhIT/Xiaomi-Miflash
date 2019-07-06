using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MiUSB;
using XiaoMiFlash.code.bl;
using XiaoMiFlash.code.module;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000021 RID: 33
	public class UsbDevice
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000EC RID: 236 RVA: 0x0000CE2D File Offset: 0x0000B02D
		// (set) Token: 0x060000ED RID: 237 RVA: 0x0000CE34 File Offset: 0x0000B034
		public static List<string> MtkDevice
		{
			get
			{
				return UsbDevice._mtkdevice;
			}
			set
			{
				UsbDevice._mtkdevice = value;
			}
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000CE3C File Offset: 0x0000B03C
		public static List<Device> GetDevice()
		{
			List<Device> ds = new List<Device>();
			string chip = MiAppConfig.Get("chip");
			if (chip == "MTK")
			{
				using (List<string>.Enumerator enumerator = UsbDevice.MtkDevice.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string item = enumerator.Current;
						int index = int.Parse(item.ToLower().Replace("com", ""));
						ds.Add(new Device
						{
							Index = index,
							Name = item,
							DeviceCtrl = new MTKDevice()
						});
					}
					return ds;
				}
			}
			string[] dArr = ComPortCtrl.getDevicesQc();
			foreach (string item2 in dArr)
			{
				int index = int.Parse(item2.ToLower().Replace("com", "")) / 10;
				ds.Add(new Device
				{
					Index = index,
					Name = item2,
					DeviceCtrl = new SerialPortDevice()
				});
			}
			Log.w("get AllUsbDevices");
			List<TreeViewUsbItem> list = TreeViewUsbItem.AllUsbDevices;
			Log.w("GetScriptDevices");
			List<UsbNodeConnectionInformation> devs = UsbDevice.GetScriptDevices(list);
			List<string> fastbootDevices = Enumerable.ToList<string>(UsbDevice.GetScriptDevice());
			foreach (string item3 in fastbootDevices)
			{
				foreach (UsbNodeConnectionInformation info in devs)
				{
					if (!string.IsNullOrEmpty(item3) && !string.IsNullOrEmpty(info.DeviceDescriptor.SerialNumber) && item3.ToLower() == info.DeviceDescriptor.SerialNumber.ToLower())
					{
						int deviceIndex = UsbDevice.GetDeviceIndex(item3);
						int count = 10;
						while (deviceIndex <= 0 && count-- >= 0)
						{
							if (count < 9)
							{
								Thread.Sleep(200);
							}
							deviceIndex = UsbDevice.GetDeviceIndex(item3);
						}
						ds.Add(new Device
						{
							Index = deviceIndex,
							Name = info.DeviceDescriptor.SerialNumber,
							IdProduct = info.DeviceDescriptor.idProduct,
							IdVendor = info.DeviceDescriptor.idVendor,
							DeviceCtrl = new ScriptDevice()
						});
					}
				}
			}
			return ds;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000D10C File Offset: 0x0000B30C
		public static string[] GetScriptDevice()
		{
			List<string> ds = new List<string>();
			string fastboot = Script.fastboot;
			if (File.Exists(fastboot.Replace("\"", "")))
			{
				Cmd cmd = new Cmd("", "");
				string devices = cmd.Execute(null, fastboot + " devices");
				string[] arr = Regex.Split(devices, "\r\n");
				for (int i = 0; i < arr.Length; i++)
				{
					if (!string.IsNullOrEmpty(arr[i]))
					{
						ds.Add(Regex.Split(arr[i], "\t")[0]);
					}
				}
				return ds.ToArray();
			}
			throw new Exception("no fastboot.");
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000D1B8 File Offset: 0x0000B3B8
		public static int GetDeviceIndex(string devicesSn)
		{
			int index = -1;
			string installer = Script.qcCoInstaller;
			if (File.Exists(installer.Replace("\"", "")))
			{
				string result = "";
				string cmdStr = "";
				try
				{
					Cmd cmd = new Cmd("", "");
					cmdStr = string.Format("rundll32.exe {0},qcGetDeviceIndex {1}", installer, devicesSn);
					result = cmd.Execute(null, cmdStr);
					if (!string.IsNullOrEmpty(result))
					{
						index = Convert.ToInt32(result);
					}
				}
				catch (Exception err)
				{
					Log.w(cmdStr + ":" + result);
					Log.w(err.Message + ":" + err.StackTrace);
				}
			}
			return index;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000D26C File Offset: 0x0000B46C
		private static List<UsbNodeConnectionInformation> GetScriptDevices(List<TreeViewUsbItem> UsbItems)
		{
			List<UsbNodeConnectionInformation> outItems = new List<UsbNodeConnectionInformation>();
			foreach (TreeViewUsbItem item in UsbItems)
			{
				UsbDevice.GetAndroidDevices(item, ref outItems);
			}
			return outItems;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		private static void GetAndroidDevices(TreeViewUsbItem item, ref List<UsbNodeConnectionInformation> outItems)
		{
			try
			{
				if (item.Children != null && item.Children.Count > 0)
				{
					List<TreeViewUsbItem> children = item.Children;
					using (List<TreeViewUsbItem>.Enumerator enumerator = children.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TreeViewUsbItem usbItem = enumerator.Current;
							UsbDevice.GetAndroidDevices(usbItem, ref outItems);
						}
						goto IL_1DF;
					}
				}
				UsbNodeConnectionInformation info = (UsbNodeConnectionInformation)item.Data;
				if (info.DeviceDescriptor.Manufacturer != null && (info.DeviceDescriptor.Product.ToLower() == "android" || info.DeviceDescriptor.Product.ToLower() == "fastboot" || info.DeviceDescriptor.Product.ToLower() == "intel android ad" || info.DeviceDescriptor.Manufacturer.ToLower().IndexOf("xiaomi inc") >= 0 || Convert.ToInt32(info.DeviceDescriptor.idVendor).ToString("x4") == "8087" || Convert.ToInt32(info.DeviceDescriptor.idVendor).ToString("x4") == "0955" || Convert.ToInt32(info.DeviceDescriptor.idVendor).ToString("x4").ToLower() == "05c6" || Convert.ToInt32(info.DeviceDescriptor.idVendor).ToString("x4").ToLower() == "18d1" || Convert.ToInt32(info.DeviceDescriptor.idVendor).ToString("x4") == "2717"))
				{
					outItems.Add(info);
				}
				IL_1DF:;
			}
			catch (Exception ex)
			{
				Log.w(ex.Message + " " + ex.StackTrace);
			}
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000D504 File Offset: 0x0000B704
		private static List<UsbNodeConnectionInformation> GetDeviceIndex(string[] deviceSn, List<TreeViewUsbItem> UsbItems)
		{
			List<UsbNodeConnectionInformation> outItems = new List<UsbNodeConnectionInformation>();
			foreach (TreeViewUsbItem item in UsbItems)
			{
				UsbDevice.GetLastChild(item, deviceSn, ref outItems);
				if (outItems.Count == deviceSn.Length)
				{
					break;
				}
			}
			return outItems;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000D568 File Offset: 0x0000B768
		private static TreeViewUsbItem GetLastChild(TreeViewUsbItem item, string[] deviceSn, ref List<UsbNodeConnectionInformation> outItems)
		{
			if (item.Children != null && item.Children.Count > 0)
			{
				List<TreeViewUsbItem> children = item.Children;
				using (List<TreeViewUsbItem>.Enumerator enumerator = children.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						TreeViewUsbItem usbItem = enumerator.Current;
						UsbDevice.GetLastChild(usbItem, deviceSn, ref outItems);
					}
					return item;
				}
			}
			try
			{
				UsbNodeConnectionInformation info = (UsbNodeConnectionInformation)item.Data;
				if (item.Data != null && Enumerable.ToList<string>(deviceSn).Contains(info.DeviceDescriptor.SerialNumber))
				{
					outItems.Add(info);
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				Log.w(e.Message + " " + e.StackTrace);
			}
			return item;
		}

		// Token: 0x040000B5 RID: 181
		private static List<string> _mtkdevice = new List<string>();
	}
}

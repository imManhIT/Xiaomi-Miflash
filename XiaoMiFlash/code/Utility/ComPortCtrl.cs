using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XiaoMiFlash.code.module;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x0200002B RID: 43
	public class ComPortCtrl
	{
		// Token: 0x0600012D RID: 301 RVA: 0x0000E41C File Offset: 0x0000C61C
		private static string[] getDevices()
		{
			List<string> validPorts = new List<string>();
			try
			{
				ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity WHERE ClassGuid=\"{4d36e978-e325-11ce-bfc1-08002be10318}\" and Name LIKE '%Qualcomm HS-USB QDLoader 9008%'");
				foreach (ManagementBaseObject managementBaseObject in searcher.Get())
				{
					ManagementObject obj = (ManagementObject)managementBaseObject;
					string name = obj.GetPropertyValue("Name").ToString();
					string desc = obj.GetPropertyValue("Description").ToString();
					string portName = name.Replace(desc, "").Replace('(', ' ').Replace(')', ' ');
					validPorts.Add(portName.Trim());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Log.w(ex.Message);
			}
			return validPorts.ToArray();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000E504 File Offset: 0x0000C704
		public static string[] getDevicesQc()
		{
			List<string> ds = new List<string>();
			string lsusb = Script.QcLsUsb;
			if (File.Exists(lsusb.Replace("\"", "")))
			{
				Cmd cmd = new Cmd("", "");
				string devices = cmd.Execute(null, lsusb);
				string[] arr = Regex.Split(devices, "\r\n");
				for (int i = 0; i < arr.Length; i++)
				{
					if (!string.IsNullOrEmpty(arr[i]) && arr[i].IndexOf("9008") > 0)
					{
						string name = arr[i].Split(new char[]
						{
							'('
						})[1].Replace(')', ' ');
						ds.Add(name.Trim());
					}
				}
				return ds.ToArray();
			}
			throw new Exception("no lsusb.");
		}
	}
}

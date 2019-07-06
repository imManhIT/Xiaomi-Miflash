using System;
using System.Collections;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.module;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000032 RID: 50
	public class Cmd
	{
		// Token: 0x06000144 RID: 324 RVA: 0x0000EBEC File Offset: 0x0000CDEC
		public Cmd(string _deivcename, string _scriptPath)
		{
			this.devicename = _deivcename;
			this.scriptPath = _scriptPath;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000EC39 File Offset: 0x0000CE39
		public string Execute(string deviceName, string dosCommand)
		{
			return this.Execute(deviceName, dosCommand, 0);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000EC44 File Offset: 0x0000CE44
		public string Execute_returnLine(string deviceName, string dosCommand)
		{
			return this.Execute_returnLine(deviceName, dosCommand, 1);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000EC50 File Offset: 0x0000CE50
		public string Execute(string deviceName, string command, int seconds)
		{
			string output = "";
			if (command != null && !command.Equals(""))
			{
				Process process = this.initCmdProcess(command);
				try
				{
					if (process.Start())
					{
						if (seconds == 0)
						{
							process.WaitForExit();
						}
						else
						{
							process.WaitForExit(seconds);
						}
						output = process.StandardOutput.ReadToEnd();
						output += process.StandardError.ReadToEnd();
					}
				}
				catch (Exception err)
				{
					Log.w(deviceName, err.Message);
				}
				finally
				{
					if (process != null)
					{
						process.Close();
						process.Dispose();
					}
				}
			}
			return output;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000ECF4 File Offset: 0x0000CEF4
		public string Execute_returnLine(string deviceName, string command, int seconds)
		{
			this.devicename = deviceName;
			StringBuilder output = new StringBuilder();
			if (command != null && !command.Equals(""))
			{
				try
				{
					Process process = this.initCmdProcess(command);
					this.process = process;
					foreach (Device item in FlashingDevice.flashDeviceList)
					{
						if (item.Name == this.devicename)
						{
							item.DCmd = this;
						}
					}
					process.OutputDataReceived += new DataReceivedEventHandler(this.process_OutputDataReceived);
					process.ErrorDataReceived += new DataReceivedEventHandler(this.process_ErrorDataReceived);
					process.EnableRaisingEvents = true;
					process.Exited += new EventHandler(this.process_Exited);
					if (process.Start())
					{
						Log.w(this.devicename, string.Format("Physical Memory Usage:{0} Byte", process.WorkingSet64.ToString()));
						process.BeginOutputReadLine();
						process.BeginErrorReadLine();
						process.WaitForExit();
					}
				}
				catch (Exception ex)
				{
					Log.w(deviceName, ex, false);
					FlashingDevice.UpdateDeviceStatus(this.devicename, default(float?), ex.Message, "error", true);
				}
				finally
				{
					this.FlashDone();
					if (this.process != null)
					{
						this.process.Close();
						this.process.Dispose();
						Log.w(this.devicename, "process exit.");
					}
				}
			}
			return output.ToString();
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000EEB0 File Offset: 0x0000D0B0
		private void process_Exited(object sender, EventArgs e)
		{
			try
			{
				Process process = (Process)sender;
				process.CancelErrorRead();
				process.CancelOutputRead();
				process.Refresh();
				this.FlashDone();
			}
			catch (Exception ex)
			{
				Log.w(this.devicename, ex.Message, false);
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000EF04 File Offset: 0x0000D104
		private void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				try
				{
					string line = e.Data;
					if (!string.IsNullOrEmpty(this.errMsg) || line.ToLower().IndexOf("error") >= 0 || line.ToLower().IndexOf("fail") >= 0 || line == "Missmatching image and device")
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = line;
						}
						Process process = (Process)sender;
						Cmd.KillProcessAndChildrens(process.Id);
						if (!process.HasExited)
						{
							process.Kill();
							process.Close();
							process.Dispose();
						}
						throw new Exception(line);
					}
					Log.w(this.devicename, e.Data, false);
					FlashingDevice.UpdateDeviceStatus(this.devicename, default(float?), e.Data, "flashing", false);
				}
				catch (Exception ex)
				{
					try
					{
						Process process2 = (Process)sender;
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = ex.Message + "\r\n" + ex.StackTrace;
						}
						if (!process2.HasExited)
						{
							process2.Kill();
							process2.Close();
							process2.Dispose();
						}
					}
					catch (Exception cex)
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = cex.Message + "\r\n" + cex.StackTrace;
						}
					}
				}
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000F080 File Offset: 0x0000D280
		private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				try
				{
					string line = e.Data;
					if (line.ToLower().IndexOf("pause") >= 0)
					{
						try
						{
							Process process = (Process)sender;
							if (!process.HasExited)
							{
								process.Kill();
								process.Close();
								process.Dispose();
								return;
							}
						}
						catch (Exception ex)
						{
							Log.w(ex.Message);
						}
					}
					if (!string.IsNullOrEmpty(this.errMsg) || line.ToLower().IndexOf("error") >= 0 || line.ToLower().IndexOf("fail") >= 0 || line.ToLower() == "missmatching image and device" || line.ToLower() == "missmatching board version")
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = line;
						}
						Process process2 = (Process)sender;
						Cmd.KillProcessAndChildrens(process2.Id);
						if (!process2.HasExited)
						{
							process2.Kill();
							process2.Close();
							process2.Dispose();
						}
					}
					float? percent = this.GetPercent(line);
					Log.w(this.devicename, e.Data, false);
					FlashingDevice.UpdateDeviceStatus(this.devicename, percent, line, "flashing", false);
				}
				catch (Exception ex2)
				{
					try
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = ex2.Message + "\r\n" + ex2.StackTrace;
						}
						Process process3 = (Process)sender;
						if (!process3.HasExited)
						{
							process3.Kill();
							process3.Close();
							process3.Dispose();
						}
					}
					catch (Exception cex)
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = cex.Message + "\r\n" + cex.StackTrace;
						}
					}
				}
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000F28C File Offset: 0x0000D48C
		public string consoleMode_Execute_returnLine(string deviceName, string command, int seconds)
		{
			this.devicename = deviceName;
			StringBuilder output = new StringBuilder();
			if (command != null && !command.Equals(""))
			{
				try
				{
					Process process = this.initCmdProcess(command);
					this.process = process;
					foreach (Device item in FlashingDevice.flashDeviceList)
					{
						if (item.Name == this.devicename)
						{
							item.DCmd = this;
						}
					}
					process.OutputDataReceived += new DataReceivedEventHandler(this.consoleMode_process_OutputDataReceived);
					process.ErrorDataReceived += new DataReceivedEventHandler(this.consoleMode_process_ErrorDataReceived);
					process.EnableRaisingEvents = true;
					process.Exited += new EventHandler(this.consoleMode_process_Exited);
					if (process.Start())
					{
						Log.w(this.devicename, string.Format("Physical Memory Usage:{0} Byte", process.WorkingSet64.ToString()));
						process.BeginOutputReadLine();
						process.BeginErrorReadLine();
						process.WaitForExit();
					}
				}
				catch (Exception ex)
				{
					Log.w(deviceName, ex, false);
					FlashingDevice.UpdateDeviceStatus(this.devicename, default(float?), ex.Message, "error", true);
				}
				finally
				{
					if (this.process != null)
					{
						this.process.Close();
						this.process.Dispose();
						Log.w(this.devicename, "process exit.");
					}
					this.FlashDone();
				}
			}
			return output.ToString();
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000F448 File Offset: 0x0000D648
		private void consoleMode_process_Exited(object sender, EventArgs e)
		{
			try
			{
				Process process = (Process)sender;
				process.CancelErrorRead();
				process.CancelOutputRead();
				process.Refresh();
			}
			catch (Exception ex)
			{
				Log.w(this.devicename, ex.Message, false);
			}
			finally
			{
				this.FlashDone();
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000F4AC File Offset: 0x0000D6AC
		private void consoleMode_process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				try
				{
					string line = e.Data;
					if (!string.IsNullOrEmpty(this.errMsg) || line.ToLower().IndexOf("error") >= 0 || line.ToLower().IndexOf("fail") >= 0 || line == "Missmatching image and device")
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = line;
						}
						Process process = (Process)sender;
						if (!process.HasExited)
						{
							process.Kill();
							process.Close();
							process.Dispose();
						}
					}
					Log.w(this.devicename, e.Data, false);
					FlashingDevice.UpdateDeviceStatus(this.devicename, default(float?), e.Data, "flashing", false);
				}
				catch (Exception ex)
				{
					try
					{
						Process process2 = (Process)sender;
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = ex.Message + "\r\n" + ex.StackTrace;
						}
						if (!process2.HasExited)
						{
							process2.Kill();
							process2.Close();
							process2.Dispose();
						}
					}
					catch (Exception cex)
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = cex.Message + "\r\n" + cex.StackTrace;
						}
					}
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000F614 File Offset: 0x0000D814
		private void consoleMode_process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Data))
			{
				try
				{
					string line = e.Data;
					if (line.ToLower().IndexOf("no insert comport") >= 0)
					{
						try
						{
							Process process = (Process)sender;
							this.errMsg = "no device insert";
							if (!process.HasExited)
							{
								process.Kill();
								process.Close();
								process.Dispose();
								return;
							}
						}
						catch (Exception ex)
						{
							Log.w(ex.Message, true);
						}
					}
					if (!string.IsNullOrEmpty(this.errMsg) || line.ToLower().IndexOf("error") >= 0 || line.ToLower().IndexOf("fail") >= 0)
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = line;
						}
						Process process2 = (Process)sender;
						if (!process2.HasExited)
						{
							process2.Kill();
							process2.Close();
							process2.Dispose();
						}
						Log.w(this.devicename, line, false);
					}
					else
					{
						if (line.ToLower().IndexOf("insert comport: com") >= 0)
						{
							string partName = line.ToLower().Replace("insert comport:", "").Trim();
							Device d = new Device();
							d.Index = int.Parse(partName.Replace("com", ""));
							d.Name = partName;
							d.StartTime = DateTime.Now;
							this.devicename = d.Name;
							if (MiAppConfig.Get("chip") == "MTK")
							{
								d.Status = "wait for device insert";
							}
							else
							{
								d.Status = "flashing";
							}
							d.Progress = 0f;
							d.IsDone = new bool?(false);
							d.IsUpdate = true;
							if (this.devicename.ToLower() == d.Name.ToLower() && UsbDevice.MtkDevice.IndexOf(d.Name) < 0)
							{
								UsbDevice.MtkDevice.Add(d.Name);
								FlashingDevice.consoleMode_UsbInserted = true;
							}
							Log.w(this.devicename, line);
						}
						else if (line.ToLower().IndexOf("da usb vcom") >= 0)
						{
							Regex reg = new Regex("\\(com.*\\)");
							Match match = reg.Match(line.ToLower());
							if (match.Groups.Count > 0)
							{
								string vPartName = match.Groups[0].Value.Replace("com", "").Replace("(", "").Replace(")", "").Trim();
								if (this.ini.GetIniInt("bootrom", this.devicename, 0) == 0 && !this.ini.WriteIniInt("bootrom", this.devicename, int.Parse(vPartName)))
								{
									MessageBox.Show("couldn't write vcom");
								}
							}
						}
						else if (line.ToLower().IndexOf("download success") >= 0)
						{
							try
							{
								Log.w(this.devicename, e.Data, false);
								this.FlashDone();
								if (!this.process.HasExited)
								{
									this.process.Kill();
									this.process.Close();
									this.process.Dispose();
								}
							}
							catch (Exception ex2)
							{
								Log.w(this.devicename, ex2.Message, false);
							}
							return;
						}
						string msg = line;
						float? percent = default(float?);
						if (line.ToLower().IndexOf("percent") >= 0)
						{
							string lowerData = line.ToLower();
							string tag = "percent";
							int tagSite = lowerData.IndexOf(tag);
							msg = lowerData.Substring(0, tagSite);
							string percentStr = lowerData.Substring(tagSite + tag.Length, lowerData.Length - tag.Length - tagSite).Trim();
							percent = new float?((float)Convert.ToInt32(percentStr) / 100f);
						}
						else
						{
							Log.w(this.devicename, e.Data, true);
						}
						FlashingDevice.UpdateDeviceStatus(this.devicename, percent, msg, "flashing", false);
					}
				}
				catch (Exception ex3)
				{
					try
					{
						if (string.IsNullOrEmpty(this.errMsg))
						{
							this.errMsg = ex3.Message + "\r\n" + ex3.StackTrace;
						}
						Process process3 = (Process)sender;
						if (!process3.HasExited)
						{
							process3.Kill();
							process3.Close();
							process3.Dispose();
						}
					}
					catch (Exception cex)
					{
						Log.w(cex.Message, true);
					}
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000FAF0 File Offset: 0x0000DCF0
		private void FlashDone()
		{
			if (string.IsNullOrEmpty(this.errMsg))
			{
				FlashingDevice.UpdateDeviceStatus(this.devicename, new float?(1f), "flash done", "success", true);
				Log.w(this.devicename, "flash done");
			}
			else
			{
				FlashingDevice.UpdateDeviceStatus(this.devicename, new float?(1f), this.errMsg, "error", true);
				Log.w(this.devicename, "error:" + this.errMsg, false);
			}
			UsbDevice.MtkDevice.Remove(this.devicename);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000FB8C File Offset: 0x0000DD8C
		private Process initCmdProcess(string command)
		{
			return new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = "cmd.exe",
					Arguments = "/C " + command,
					UseShellExecute = false,
					RedirectStandardInput = false,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					CreateNoWindow = true
				}
			};
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000FBEC File Offset: 0x0000DDEC
		private float? GetPercent(string line)
		{
			Hashtable table = SoftwareImage.DummyProgress;
			float? percent = default(float?);
			foreach (object obj in table.Keys)
			{
				string key = (string)obj;
				if (line.IndexOf(key) >= 0)
				{
					percent = new float?((float)Convert.ToInt32(table[key]) / 50f);
					break;
				}
			}
			return percent;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000FC78 File Offset: 0x0000DE78
		private static void KillProcessAndChildrens(int pid)
		{
			ManagementObjectSearcher processSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
			ManagementObjectCollection processCollection = processSearcher.Get();
			try
			{
				Process proc = Process.GetProcessById(pid);
				if (!proc.HasExited)
				{
					proc.Kill();
				}
			}
			catch (ArgumentException)
			{
			}
			if (processCollection != null)
			{
				foreach (ManagementBaseObject managementBaseObject in processCollection)
				{
					ManagementObject mo = (ManagementObject)managementBaseObject;
					Cmd.KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"]));
				}
			}
		}

		// Token: 0x04000102 RID: 258
		private string devicename;

		// Token: 0x04000103 RID: 259
		public string errMsg = "";

		// Token: 0x04000104 RID: 260
		private string scriptPath = "";

		// Token: 0x04000105 RID: 261
		public Process process;

		// Token: 0x04000106 RID: 262
		public string logType = "";

		// Token: 0x04000107 RID: 263
		private IniFile ini = new IniFile();
	}
}

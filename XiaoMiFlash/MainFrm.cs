using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MiUSB;
using XiaoMiFlash.code.authFlash;
using XiaoMiFlash.code.bl;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.lan;
using XiaoMiFlash.code.MiControl;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;
using XiaoMiFlash.form;
using XiaoMiFlash.Properties;

namespace XiaoMiFlash
{
	// Token: 0x02000013 RID: 19
	public partial class MainFrm : MiBaseFrm
	{
		// Token: 0x0600006D RID: 109 RVA: 0x00007FA0 File Offset: 0x000061A0
		public MainFrm()
		{
			this.InitializeComponent();
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00008033 File Offset: 0x00006233
		// (set) Token: 0x0600006F RID: 111 RVA: 0x0000803B File Offset: 0x0000623B
		public string ValidateSpecifyXml
		{
			get
			{
				return this.validateSpecifyXml;
			}
			set
			{
				this.validateSpecifyXml = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00008044 File Offset: 0x00006244
		public string SwPath
		{
			get
			{
				return this.txtPath.Text;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00008051 File Offset: 0x00006251
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00008059 File Offset: 0x00006259
		public bool ReadBackVerify
		{
			get
			{
				return this.readbackverify;
			}
			set
			{
				this.readbackverify = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00008062 File Offset: 0x00006262
		// (set) Token: 0x06000074 RID: 116 RVA: 0x0000806A File Offset: 0x0000626A
		public bool OpenWriteDump
		{
			get
			{
				return this.openwritedump;
			}
			set
			{
				this.openwritedump = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00008073 File Offset: 0x00006273
		// (set) Token: 0x06000076 RID: 118 RVA: 0x0000807B File Offset: 0x0000627B
		public bool OpenReadDump
		{
			get
			{
				return this.openreaddump;
			}
			set
			{
				this.openreaddump = value;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00008084 File Offset: 0x00006284
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000808C File Offset: 0x0000628C
		public bool Verbose
		{
			get
			{
				return this.verbose;
			}
			set
			{
				this.verbose = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00008095 File Offset: 0x00006295
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000809D File Offset: 0x0000629D
		public bool AutoDetectDevice
		{
			get
			{
				return this.autodetectdevice;
			}
			set
			{
				this.autodetectdevice = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000080A8 File Offset: 0x000062A8
		public List<string> ComportList
		{
			get
			{
				string comStr = string.Empty;
				if (!string.IsNullOrEmpty(MiAppConfig.Get("mtkComs")))
				{
					this.comportList = Enumerable.ToList<string>(MiAppConfig.Get("mtkComs").Split(new char[]
					{
						','
					}));
				}
				else
				{
					this.comportList.Clear();
					UsbDevice.MtkDevice.Clear();
				}
				UsbDevice.MtkDevice.Clear();
				foreach (string item in this.comportList)
				{
					if (!string.IsNullOrEmpty(item))
					{
						comStr = string.Format("com{0}", item);
						if (UsbDevice.MtkDevice.IndexOf(comStr) < 0)
						{
							UsbDevice.MtkDevice.Add(comStr);
						}
					}
				}
				return this.comportList;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00008188 File Offset: 0x00006388
		public void SetFactory(string factory)
		{
			if (!string.IsNullOrEmpty(factory))
			{
				this.statusTab.Text = factory;
				this.isFactory = true;
			}
			MiFlashGlobal.IsFactory = this.isFactory;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000081B0 File Offset: 0x000063B0
		public void SetChip(string chip)
		{
			if (!string.IsNullOrEmpty(chip))
			{
				if (chip != null)
				{
					if (chip == "Qualcomm")
					{
						this.pnlMTK.Visible = false;
						this.pnlQcom.Visible = true;
						this.devicelist.Location = new Point(21, 113);
						return;
					}
					if (chip == "MTK")
					{
						this.pnlMTK.Visible = true;
						this.pnlQcom.Visible = false;
						this.devicelist.Location = new Point(21, 154);
						if (!string.IsNullOrEmpty(MiAppConfig.Get("scatter")))
						{
							this.txtScatter.Text = MiAppConfig.Get("scatter");
						}
						if (!string.IsNullOrEmpty(MiAppConfig.Get("da")))
						{
							this.txtDa.Text = MiAppConfig.Get("da").ToString();
						}
						this.cmbDlType.Items.Clear();
						foreach (object obj in MiProperty.DlTable)
						{
							DictionaryEntry item = (DictionaryEntry)obj;
							this.cmbDlType.Items.Add(item.Key.ToString());
						}
						if (this.cmbDlType.Items.Count > 0)
						{
							if (!string.IsNullOrEmpty(MiAppConfig.Get("dlType")))
							{
								this.cmbDlType.Text = MiAppConfig.Get("dlType");
							}
							else
							{
								this.cmbDlType.Text = this.cmbDlType.Items[0].ToString();
							}
						}
						this.cmbChkSum.Items.Clear();
						foreach (KeyValuePair<string, int> item2 in MiProperty.ChkSumTable)
						{
							this.cmbChkSum.Items.Add(item2.Key.ToString());
						}
						if (this.cmbChkSum.Items.Count > 0)
						{
							if (!string.IsNullOrEmpty(MiAppConfig.Get("chkSum")))
							{
								this.cmbChkSum.Text = MiAppConfig.Get("chkSum");
							}
							else
							{
								this.cmbChkSum.Text = this.cmbChkSum.Items[0].ToString();
							}
						}
						if (this.ComportList.Count != 0)
						{
							return;
						}
						string mtkComs = MiAppConfig.Get("mtkComs");
						if (!string.IsNullOrEmpty(mtkComs))
						{
							mtkComs.Split(new char[]
							{
								','
							});
							return;
						}
						return;
					}
				}
				this.pnlMTK.Visible = false;
				this.pnlQcom.Visible = true;
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00008484 File Offset: 0x00006684
		private bool IsRunAsAdmin()
		{
			WindowsIdentity id = WindowsIdentity.GetCurrent();
			WindowsPrincipal principal = new WindowsPrincipal(id);
			return principal.IsInRole(544);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000084AC File Offset: 0x000066AC
		private void MainFrm_Load(object sender, EventArgs e)
		{
			this.SetLanguage();
			string path = MiAppConfig.Get("swPath");
			this.txtPath.Text = path;
			this.SetFactory(MiAppConfig.Get("factory"));
			if (Directory.Exists(this.txtPath.Text))
			{
				this.SetScriptItems(this.txtPath.Text);
				this.cmbScriptItem.SetText(MiAppConfig.Get("script"));
			}
			this.script = MiAppConfig.Get("script");
			this.SetChip(MiAppConfig.Get("chip"));
			this.txtScatter.Text = MiAppConfig.Get("scatter");
			MiFlashGlobal.Version = this.Text;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000855E File Offset: 0x0000675E
		public void AutoDetectUsb()
		{
			this.RefreshDevice();
			if (this.AutoDetectDevice)
			{
				this.miUsb.AddUSBEventWatcher(new EventArrivedEventHandler(this.USBInsertHandler), null, new TimeSpan(0, 0, 3));
			}
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000085B8 File Offset: 0x000067B8
		private void USBInsertHandler(object sender, EventArrivedEventArgs e)
		{
			if (e.NewEvent.ClassPath.ClassName == "__InstanceCreationEvent")
			{
				this.btnAutoFlash.BeginInvoke(delegate(string msg)
				{
					this.btnAutoFlash_Click(this.btnAutoFlash, new EventArgs());
				}, new object[]
				{
					""
				});
				return;
			}
			if (e.NewEvent.ClassPath.ClassName == "__InstanceDeletionEvent")
			{
				this.btnAutoFlash.BeginInvoke(delegate(string msg)
				{
					this.btnAutoFlash_Click(this.btnAutoFlash, new EventArgs());
				}, new object[]
				{
					""
				});
			}
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000865C File Offset: 0x0000685C
		private void USBRemoveHandler(object sender, EventArrivedEventArgs e)
		{
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00008674 File Offset: 0x00006874
		private void SetText(string text)
		{
			if (this.btnRefresh.InvokeRequired)
			{
				this.btnRefresh.BeginInvoke(delegate(string msg)
				{
					this.btnRefresh_Click(this.btnRefresh, new EventArgs());
				}, new object[]
				{
					text
				});
				return;
			}
			this.btnRefresh_Click(this.btnRefresh, new EventArgs());
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00008740 File Offset: 0x00006940
		private void SetLogText(string text)
		{
			try
			{
				if (this.txtLog.InvokeRequired)
				{
					this.txtLog.BeginInvoke(delegate(string msg)
					{
						if (this.txtLog.Text.Length >= 575488)
						{
							this.txtLog.Text = "";
						}
						this.txtLog.AppendText(msg);
						this.txtLog.AppendText("\r\n");
						this.txtLog.Select(this.txtLog.TextLength, 0);
						this.txtLog.ScrollToCaret();
					}, new object[]
					{
						text
					});
				}
				else
				{
					if (this.txtLog.Text.Length >= 575488)
					{
						this.txtLog.Text = "";
					}
					this.txtLog.AppendText(text);
					this.txtLog.AppendText("\r\n");
					this.txtLog.Select(this.txtLog.TextLength, 0);
					this.txtLog.ScrollToCaret();
				}
			}
			catch (Exception err)
			{
				Log.w(err.Message + ":" + err.StackTrace);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00008860 File Offset: 0x00006A60
		private void Valide()
		{
			try
			{
				string validateMsg = ImageValidation.Validate(this.txtPath.Text);
				if (validateMsg.IndexOf("md5 validate successfully") < 0)
				{
					this.lblMD5.ForeColor = Color.Red;
				}
				base.Invoke(delegate(object A_1, EventArgs A_2)
				{
					this.lblMD5.Text = validateMsg;
					this.frm.TopMost = false;
					this.frm.Hide();
				});
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00008960 File Offset: 0x00006B60
		private void ShowMsg(string msg, bool openOrClose)
		{
			try
			{
				base.Invoke(delegate(object A_1, EventArgs A_2)
				{
					this.lblMD5.Text = msg;
					if (openOrClose)
					{
						this.frm.TopMost = true;
						this.frm.Show();
						return;
					}
					this.frm.TopMost = false;
					this.frm.Hide();
				});
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000089C4 File Offset: 0x00006BC4
		private void btnBrwDic_Click(object sender, EventArgs e)
		{
			DialogResult result = this.fbdSelect.ShowDialog();
			if (result == 1)
			{
				this.txtPath.Text = this.fbdSelect.SelectedPath;
				this.SetScriptItems(this.txtPath.Text);
				if (MiAppConfig.GetAppConfig("checkMD5").ToLower() != "true")
				{
					return;
				}
				try
				{
					this.frm.TopMost = true;
					this.frm.Show();
					new Thread(new ThreadStart(this.Valide))
					{
						IsBackground = true
					}.Start();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					Log.w(ex.Message + " " + ex.StackTrace);
				}
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00008A9C File Offset: 0x00006C9C
		private void btnSelectScatter_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "scatter|*.txt";
			if (dialog.ShowDialog() == 1)
			{
				this.txtScatter.Text = dialog.FileName;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00008AD4 File Offset: 0x00006CD4
		private void btnDa_Click(object sender, EventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Filter = "da|*.bin";
			dialog.InitialDirectory = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "da";
			if (dialog.ShowDialog() == 1)
			{
				this.txtDa.Text = dialog.FileName;
				MiAppConfig.SetValue("da", this.txtDa.Text);
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00008B40 File Offset: 0x00006D40
		private void SetScriptItems(string path)
		{
			DirectoryInfo info = new DirectoryInfo(path);
			DirectoryInfo[] dics = info.GetDirectories();
			string scriptPath = info.Parent.FullName;
			foreach (DirectoryInfo item in dics)
			{
				if (item.Name.ToLower().IndexOf("images") >= 0)
				{
					scriptPath = path;
					break;
				}
			}
			info = new DirectoryInfo(scriptPath);
			List<string> scriptList = new List<string>();
			foreach (FileInfo item2 in info.GetFiles())
			{
				if (item2.Name.LastIndexOf(".bat") >= 0)
				{
					scriptList.Add(item2.Name);
				}
			}
			this.cmbScriptItem.SetItem(scriptList.ToArray());
			if (!Directory.Exists(this.txtPath.Text))
			{
				return;
			}
			string[] searchFiles = FileSearcher.SearchFiles(this.txtPath.Text, FlashType.SaveUserData);
			if (this.rdoCleanAll.IsChecked)
			{
				this.script = FlashType.CleanAll;
			}
			else if (this.rdoSaveUserData.IsChecked)
			{
				this.script = Path.GetFileName(searchFiles[0]);
			}
			else if (this.rdoCleanAllAndLock.IsChecked)
			{
				this.script = FlashType.CleanAllAndLock;
			}
			this.cmbScriptItem.SetText(this.script);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00008C8E File Offset: 0x00006E8E
		private void btnRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshDevice();
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00008D14 File Offset: 0x00006F14
		public void RefreshDevice()
		{
			try
			{
				this.btnRefresh.Enabled = false;
				this.btnRefresh.Cursor = Cursors.WaitCursor;
				this.deviceArr = UsbDevice.GetDevice();
				foreach (Device flashDce in this.deviceArr)
				{
					if (flashDce.DeviceCtrl.GetType() == typeof(SerialPortDevice))
					{
						if (!this.canFlash)
						{
							this.deviceArr = null;
							throw new Exception("Please authenticate edl permission.");
						}
						break;
					}
				}
				ListView.ListViewItemCollection currentItems = this.devicelist.Items;
				List<Thread> samples = new List<Thread>(this.threads);
				foreach (Thread titem in samples)
				{
					if (!titem.IsAlive)
					{
						Log.w(string.Format("thread {0} dead ,remove it", titem.Name));
						this.threads.Remove(titem);
					}
					else
					{
						bool found = false;
						foreach (Device ditem in this.deviceArr)
						{
							if (ditem.Name == titem.Name)
							{
								found = true;
								break;
							}
						}
						if (!found)
						{
							Log.w("couldn't find " + titem.Name);
							foreach (Device fitem in FlashingDevice.flashDeviceList)
							{
								if (fitem.IsDone != null && !fitem.IsDone.Value && fitem.IsUpdate)
								{
									Log.w(fitem.Name + " is flashing");
									Device tmp = null;
									foreach (Device flashingD in this.deviceArr)
									{
										if (flashingD.Name == fitem.Name)
										{
											tmp = flashingD;
											break;
										}
									}
									if (tmp != null)
									{
										Log.w(tmp.Name + " is flashing, don't add to new device");
										this.deviceArr.Remove(tmp);
									}
								}
								else if (fitem.Name == titem.Name)
								{
									fitem.IsDone = new bool?(true);
									fitem.IsUpdate = false;
									try
									{
										if (fitem.DCmd != null && fitem.DCmd.process != null && !fitem.DCmd.process.HasExited)
										{
											Log.w(string.Format("kill {0} process", fitem.Name));
											fitem.DCmd.process.CancelErrorRead();
											fitem.DCmd.process.CancelOutputRead();
											fitem.DCmd.errMsg = "process force stopped";
											fitem.DCmd.process.Kill();
											fitem.DCmd.process.Close();
											fitem.DCmd.process.Dispose();
										}
										if (fitem.DComm != null && fitem.DComm.serialPort != null && fitem.DComm.serialPort.IsOpen)
										{
											Log.w(string.Format("close serial port {0}", fitem.DComm.serialPort.PortName));
											fitem.DComm.serialPort.Close();
											fitem.DComm.serialPort.Dispose();
										}
										bool isAlive = titem.IsAlive;
										break;
									}
									catch (Exception ex)
									{
										Log.w(ex.Message);
										Log.w(ex.StackTrace);
										break;
									}
								}
							}
							if (MiAppConfig.Get("chip") != "MTK")
							{
								this.threads.Remove(titem);
							}
						}
					}
				}
				Device d;
				IEnumerable<string> dr = Enumerable.Select<Device, string>(Enumerable.Where<Device>(FlashingDevice.flashDeviceList, (Device d) => d.IsDone == null || (d.IsDone != null && d.IsDone.Value && !d.IsUpdate)), (Device d) => d.Name);
				foreach (string dn in Enumerable.ToList<string>(dr))
				{
					foreach (Device d3 in FlashingDevice.flashDeviceList)
					{
						if (d3.Name == dn.ToString())
						{
							Log.w(string.Format("FlashingDevice.flashDeviceList.Remove {0}", d3.Name));
							FlashingDevice.flashDeviceList.Remove(d3);
							break;
						}
					}
					foreach (object obj in currentItems)
					{
						ListViewItem ditem2 = (ListViewItem)obj;
						string deviceName = ditem2.SubItems[1].Text;
						if (deviceName == dn.ToString())
						{
							currentItems.Remove(ditem2);
							break;
						}
					}
					foreach (object obj2 in this.devicelist.Controls)
					{
						Control ctrl = (Control)obj2;
						if (ctrl.Name == dn.ToString() + "progressbar")
						{
							this.devicelist.Controls.Remove(ctrl);
							break;
						}
					}
				}
				foreach (Device d2 in this.deviceArr)
				{
					d = d2;
					dr = Enumerable.Select<Device, string>(Enumerable.Where<Device>(FlashingDevice.flashDeviceList, (Device fd) => fd.Name == d.Name), (Device fd) => fd.Name);
					if (Enumerable.Count<string>(dr) == 0)
					{
						d.Progress = 0f;
						d.IsDone = default(bool?);
						if (MiAppConfig.Get("chip") == "MTK")
						{
							d.Status = "wait for device insert";
							d.IsUpdate = true;
							d.StartTime = DateTime.Now;
						}
						else
						{
							d.IsUpdate = false;
						}
						Log.w("add device " + d.Name);
						FlashingDevice.flashDeviceList.Add(d);
						float progress = 0f;
						this.DrawCln(d.Index, d.Name, (double)progress);
					}
				}
				this.reDrawProgress();
			}
			catch (Exception ex2)
			{
				Log.w(ex2.Message);
				Log.w(ex2.StackTrace);
				MessageBox.Show(ex2.Message);
			}
			finally
			{
				this.btnRefresh.Enabled = true;
				this.btnRefresh.Cursor = Cursors.Default;
			}
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000095DC File Offset: 0x000077DC
		private void DrawCln(int deviceIndex, string deviceName, double progress)
		{
			ListViewItem item = new ListViewItem(new string[]
			{
				deviceIndex.ToString(),
				deviceName,
				"",
				"0s",
				"",
				""
			});
			this.devicelist.Items.Add(item);
			Rectangle SizeR = default(Rectangle);
			ProgressBar ProgBar = new ProgressBar();
			SizeR = item.SubItems[2].Bounds;
			SizeR.Width = this.devicelist.Columns[2].Width;
			ProgBar.Parent = this.devicelist;
			ProgBar.SetBounds(SizeR.X, SizeR.Y, SizeR.Width, SizeR.Height);
			ProgBar.Value = (int)progress;
			ProgBar.Visible = true;
			ProgBar.Name = deviceName + "progressbar";
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000096C4 File Offset: 0x000078C4
		private void btnFlash_Click(object sender, EventArgs e)
		{
			this.RefreshDevice();
			if (string.IsNullOrEmpty(this.txtPath.Text))
			{
				MessageBox.Show("Please select sw");
				return;
			}
			File.Exists(this.txtPath.Text + "\\" + this.script);
			if (FlashingDevice.flashDeviceList.Count == 0)
			{
				return;
			}
			try
			{
				if (!this.timer_updateStatus.Enabled)
				{
					this.timer_updateStatus.Enabled = true;
				}
				foreach (Device d in FlashingDevice.flashDeviceList)
				{
					if (d.StartTime > DateTime.MinValue && d.IsDone != null && !d.IsDone.Value && d.IsUpdate)
					{
						Log.w(d.Name + " already in flashing");
					}
					else
					{
						d.StartTime = DateTime.Now;
						d.Status = "flashing";
						d.Progress = 0f;
						d.IsDone = new bool?(false);
						d.IsUpdate = true;
						Log.w(d.Name, MiFlashGlobal.Version);
						FlashingDevice.UpdateDeviceStatus(d.Name, default(float?), "start flash", "flashing", false);
						if (this.isFactory)
						{
							CheckCPUIDResult resultArg = FactoryCtrl.GetSearchPathD(d.Name, this.txtPath.Text.Trim());
							if (!resultArg.Result)
							{
								FlashingDevice.UpdateDeviceStatus(d.Name, default(float?), string.Format("error:CheckCPUID result {0} status {1}", resultArg.Result.ToString(), resultArg.Msg), "factory ev error", true);
								Log.w(d.Name, string.Format("error:device {0} CheckCPUID result {1} status {2}", d.Name, resultArg.Result.ToString(), resultArg.Msg), false);
								continue;
							}
							this.txtPath.Text = resultArg.Path;
						}
						DeviceCtrl deviceCtrl = d.DeviceCtrl;
						deviceCtrl.deviceName = d.Name;
						deviceCtrl.swPath = this.txtPath.Text.Trim();
						deviceCtrl.flashScript = this.script;
						deviceCtrl.readBackVerify = this.ReadBackVerify;
						deviceCtrl.openReadDump = this.OpenReadDump;
						deviceCtrl.openWriteDump = this.OpenWriteDump;
						deviceCtrl.verbose = this.Verbose;
						deviceCtrl.idproduct = d.IdProduct;
						deviceCtrl.idvendor = d.IdVendor;
						Thread thread = new Thread(new ThreadStart(deviceCtrl.flash));
						thread.Name = d.Name;
						thread.IsBackground = true;
						thread.Start();
						d.DThread = thread;
						this.threads.Add(thread);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Log.w(ex.Message + "  " + ex.StackTrace);
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00009A14 File Offset: 0x00007C14
		private void btnAutoFlash_Click(object sender, EventArgs e)
		{
			this.RefreshDevice();
			this.btnFlash.BeginInvoke(delegate(string msg)
			{
				this.btnFlash_Click(this.btnFlash, new EventArgs());
			}, new object[]
			{
				""
			});
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00009A9C File Offset: 0x00007C9C
		private void btnDownload_Click(object sender, EventArgs e)
		{
			this.timer_updateStatus.Enabled = true;
			this.btnDownload.Enabled = false;
			if (this.ComportList.Count == 0)
			{
				int i;
				for (i = 1; i <= 16; i++)
				{
					IEnumerable<List<Thread>> currentThread = Enumerable.Select<Thread, List<Thread>>(Enumerable.Where<Thread>(this.threads, (Thread t) => t.Name == i.ToString()), (Thread t) => this.threads);
					if (Enumerable.Count<List<Thread>>(currentThread) == 0)
					{
						this.StartConsoleMode(i, 0);
					}
				}
			}
			else
			{
				string item;
				for (int j = 0; j < this.ComportList.Count; j++)
				{
					item = this.ComportList[j];
					IEnumerable<List<Thread>> currentThread2 = Enumerable.Select<Thread, List<Thread>>(Enumerable.Where<Thread>(this.threads, (Thread t) => t.Name == item), (Thread t) => this.threads);
					if (Enumerable.Count<List<Thread>>(currentThread2) == 0)
					{
						this.StartConsoleMode(int.Parse(item), j + 1);
					}
				}
			}
			this.RefreshDevice();
			MiAppConfig.SetValue("scatter", this.txtScatter.Text);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00009BF4 File Offset: 0x00007DF4
		private void btnStop_Click(object sender, EventArgs e)
		{
			MiProcess.KillProcess("SP_download_tool");
			foreach (Thread item in this.threads)
			{
				if (item.IsAlive)
				{
					item.Abort();
				}
			}
			this.threads.Clear();
			foreach (Device item2 in FlashingDevice.flashDeviceList)
			{
				FlashingDevice.UpdateDeviceStatus(item2.Name, new float?(1f), "stopped", "error", true);
			}
			FlashingDevice.flashDeviceList.Clear();
			this.devicelist.Items.Clear();
			this.devicelist.Controls.Clear();
			this.btnDownload.Enabled = true;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00009CF4 File Offset: 0x00007EF4
		private void StartConsoleMode(int comPort, int sort)
		{
			MTKDevice mtk = new MTKDevice();
			mtk.scatter = this.txtScatter.Text.Trim();
			mtk.da = this.txtDa.Text;
			mtk.dl_type = MiProperty.DlTable[this.cmbDlType.Text].ToString();
			mtk.deviceName = string.Format("com{0}", comPort);
			mtk.comPortIndex = comPort;
			mtk.chkSum = int.Parse(MiProperty.ChkSumTable[this.cmbChkSum.Text].ToString());
			mtk.sort = sort;
			Thread thread = new Thread(new ThreadStart(mtk.flash));
			thread.Name = mtk.comPortIndex.ToString();
			thread.IsBackground = true;
			thread.Start();
			this.threads.Add(thread);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00009DD8 File Offset: 0x00007FD8
		private void timer_updateStatus_Tick(object sender, EventArgs e)
		{
			try
			{
				if (FlashingDevice.consoleMode_UsbInserted)
				{
					FlashingDevice.consoleMode_UsbInserted = false;
					this.RefreshDevice();
				}
				bool? foundDevice = default(bool?);
				foreach (object obj in this.devicelist.Items)
				{
					ListViewItem ditem = (ListViewItem)obj;
					if (ditem.UseItemStyleForSubItems)
					{
						ditem.UseItemStyleForSubItems = false;
					}
					foreach (Device d in FlashingDevice.flashDeviceList)
					{
						if (d.IsUpdate)
						{
							List<string> cpyStatusList = new List<string>(d.StatusList);
							foreach (string item in cpyStatusList)
							{
								if (!string.IsNullOrEmpty(item))
								{
									Regex reg = new Regex("\\$fastboot -s .* reboot");
									if (reg.IsMatch(item.ToLower()))
									{
										if (item.ToLower().IndexOf("reboot bootloader") < 0)
										{
											FlashingDevice.UpdateDeviceStatus(d.Name, new float?(1f), "flash done", "success", true);
											break;
										}
										break;
									}
								}
							}
							if (d.Name.ToLower() == ditem.SubItems[1].Text.ToLower())
							{
								foundDevice = new bool?(true);
								ditem.SubItems[2].Text = d.Progress * 100f + "%";
								foreach (object obj2 in this.devicelist.Controls)
								{
									Control item2 = (Control)obj2;
									if (item2.Name == d.Name + "progressbar")
									{
										ProgressBar bar = (ProgressBar)item2;
										if (bar.Value == (int)(d.Progress * 100f) && (int)(d.Progress * 100f) < 100)
										{
											d.Progress += 0.001f;
										}
										bar.Value = (int)(d.Progress * 100f);
									}
								}
								if (d.StartTime > DateTime.MinValue)
								{
									int sec = (int)DateTime.Now.Subtract(d.StartTime).TotalSeconds;
									ditem.SubItems[3].Text = string.Format("{0}s", sec.ToString());
								}
								ditem.SubItems[4].Text = d.Status;
								ditem.SubItems[5].Text = d.Result;
								if (!(d.Status.ToLower() == "wait for device insert"))
								{
									bool? flashSuccess = default(bool?);
									if (d.IsDone != null && d.IsDone.Value && d.Status == "flash done")
									{
										d.IsDone = new bool?(true);
										ditem.SubItems[5].BackColor = Color.LightGreen;
										flashSuccess = new bool?(true);
									}
									if (d.IsDone != null && d.IsDone.Value && d.Result.ToLower() == "success")
									{
										d.IsDone = new bool?(true);
										ditem.SubItems[5].BackColor = Color.LightGreen;
										flashSuccess = new bool?(true);
									}
									else if (d.Result.ToLower().IndexOf("error") >= 0 || d.Result.ToLower().IndexOf("fail") >= 0)
									{
										d.IsDone = new bool?(true);
										ditem.SubItems[5].BackColor = Color.Red;
										flashSuccess = new bool?(false);
									}
									if (d.Status.ToLower().IndexOf("error") >= 0 || d.Status.ToLower().IndexOf("fail") >= 0 || d.Status.ToLower() == "missmatching image and device" || d.Status.ToLower() == "missmatching board version")
									{
										d.IsDone = new bool?(true);
										ditem.SubItems[5].BackColor = Color.Red;
										flashSuccess = new bool?(false);
									}
									string usbStatus = "#USB";
									if (d.IsDone == null || !d.IsDone.Value || flashSuccess == null)
									{
										break;
									}
									try
									{
										Log.w(d.Name, string.Format("flashSuccess {0}", (flashSuccess == null) ? "null" : flashSuccess.Value.ToString()), false);
										if (flashSuccess.Value)
										{
											usbStatus += string.Format("{0}OK$", d.Index.ToString());
										}
										else
										{
											usbStatus += string.Format("{0}NG$", d.Index.ToString());
										}
										TimeSpan span = DateTime.Now.Subtract(d.StartTime);
										Log.wFlashStatus(string.Format("{0}    {1}     {2}s     {3}", new object[]
										{
											d.Index.ToString(),
											d.Name,
											span.TotalSeconds.ToString(),
											d.Status
										}));
									}
									catch (Exception ex)
									{
										Log.w(ex.Message + " " + ex.StackTrace);
									}
									Log.w(d.Name, string.Format("isFactory {0} CheckCPUID {1}", this.isFactory.ToString(), d.CheckCPUID), false);
									if (this.isFactory && d.CheckCPUID && flashSuccess != null)
									{
										try
										{
											string msg = " update flash result to facotry server……";
											ToolStripStatusLabel toolStripStatusLabel = this.statusTab;
											toolStripStatusLabel.Text += msg;
											FlashResult flashResult = FactoryCtrl.SetFlashResultD(d.Name, flashSuccess.Value);
											this.statusTab.Text = MiAppConfig.Get("factory");
											if (!flashResult.Result)
											{
												d.IsDone = new bool?(true);
												ditem.SubItems[4].Text = flashResult.Msg;
												ditem.SubItems[5].Text = "error";
												ditem.SubItems[5].BackColor = Color.Red;
											}
											else
											{
												d.IsDone = new bool?(true);
												Log.w(d.Name, "flashResult.Result is true");
											}
										}
										catch (Exception ex2)
										{
											ditem.SubItems[4].Text = ex2.Message;
											ditem.SubItems[5].Text = "error";
											ditem.SubItems[5].BackColor = Color.Red;
											Log.w(ex2.Message + " " + ex2.StackTrace);
										}
									}
									if (flashSuccess != null)
									{
										Log.w(d.Name, string.Format("before:flashSuccess is {0} set IsUpdate:{1} set IsDone {2}", flashSuccess.Value.ToString(), d.IsUpdate.ToString(), d.IsDone.Value.ToString()));
										d.IsUpdate = false;
										d.IsDone = new bool?(true);
										Log.w(d.Name, string.Format("after:flashSuccess is {0} set IsUpdate:false set IsDone true", flashSuccess.Value.ToString()));
										d.StatusList.Clear();
									}
									if (this.m_aryClients.Count > 0 && this.m_aryClients[0].Sock != null)
									{
										this.SetLogText(string.Format("send :{0}\r\n", usbStatus));
										this.m_aryClients[0].Sock.Send(Encoding.ASCII.GetBytes(usbStatus));
										if (FlashingDevice.IsAllDone())
										{
											string sendMsg = "OPEN\r\n";
											this.m_aryClients[0].Sock.Send(Encoding.ASCII.GetBytes(sendMsg));
											this.SetLogText(sendMsg);
										}
									}
									if (this.clientSocket != null)
									{
										break;
									}
									break;
								}
							}
						}
					}
					if (foundDevice != null && !foundDevice.Value)
					{
						Log.w(string.Format("couldn't find {0} in FlashingDevice.flashDeviceList", ditem.SubItems[1].Text.ToLower()));
					}
				}
			}
			catch (Exception ex3)
			{
				Log.w(ex3.Message + " " + ex3.StackTrace);
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000A78C File Offset: 0x0000898C
		private void cleanMiFlashTmp()
		{
			try
			{
				if (Directory.Exists(this.txtPath.Text))
				{
					string[] files = Directory.GetFiles(this.txtPath.Text);
					foreach (string file in files)
					{
						if (File.Exists(file))
						{
							FileInfo fileInfo = new FileInfo(file);
							if (fileInfo.Name.IndexOf("miflashTmp_") == 0)
							{
								File.Delete(file);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x0000A820 File Offset: 0x00008A20
		private void cleanMiFlashTmp(string prefix)
		{
			try
			{
				if (Directory.Exists(this.txtPath.Text))
				{
					string[] files = Directory.GetFiles(this.txtPath.Text);
					foreach (string file in files)
					{
						if (File.Exists(file))
						{
							FileInfo fileInfo = new FileInfo(file);
							if (fileInfo.Name.IndexOf(prefix) == 0)
							{
								File.Delete(file);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000A8B0 File Offset: 0x00008AB0
		private void devicelist_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
		{
			Rectangle SizeR = default(Rectangle);
			int newWidth = e.NewWidth;
			foreach (object obj in this.devicelist.Controls)
			{
				Control item = (Control)obj;
				if (item.Name.IndexOf("progressbar") >= 0)
				{
					ProgressBar bar = (ProgressBar)item;
					SizeR = bar.Bounds;
					SizeR.Width = this.devicelist.Columns[2].Width;
					bar.SetBounds(this.devicelist.Items[0].SubItems[2].Bounds.X, SizeR.Y, SizeR.Width, SizeR.Height);
				}
			}
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000A9A4 File Offset: 0x00008BA4
		private void MainFrm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				MiAppConfig.SetValue("swPath", this.txtPath.Text.ToString());
				if (this.serverSocket != null)
				{
					this.serverSocket.Close();
				}
				if (this.clientSocket != null)
				{
					this.clientSocket.Close();
				}
				if (this.m_aryClients.Count > 0 && this.m_aryClients[0].Sock != null)
				{
					this.m_aryClients[0].Sock.Close();
				}
				if (MiAppConfig.Get("chip") == "MTK")
				{
					MiProcess.KillProcess("SP_download_tool");
				}
				MiProcess.KillProcess("fh_loader");
				this.cleanMiFlashTmp();
				this.miUsb.RemoveUSBEventWatcher();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				Log.w(ex.Message + " " + ex.StackTrace);
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000AAA0 File Offset: 0x00008CA0
		private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
		{
			Environment.Exit(Environment.ExitCode);
			base.Dispose();
			base.Close();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x0000AAB8 File Offset: 0x00008CB8
		public override void SetLanguage()
		{
			base.SetLanguage();
			string lan = CultureInfo.InstalledUICulture.Name;
			if (lan.ToLower().IndexOf("zh") >= 0)
			{
				base.LanID = LanguageType.chn_s;
			}
			else
			{
				base.LanID = LanguageType.eng;
			}
			LanguageProvider lp = new LanguageProvider(base.LanID);
			this.btnBrwDic.Text = lp.GetLanguage("MainFrm.btnBrwDic");
			this.btnRefresh.Text = lp.GetLanguage("MainFrm.btnRefresh");
			this.btnFlash.Text = lp.GetLanguage("MainFrm.btnFlash");
			this.devicelist.Columns[0].Text = lp.GetLanguage("MainFrm.devicelist.cln0");
			this.devicelist.Columns[1].Text = lp.GetLanguage("MainFrm.devicelist.cln1");
			this.devicelist.Columns[2].Text = lp.GetLanguage("MainFrm.devicelist.cln2");
			this.devicelist.Columns[3].Text = lp.GetLanguage("MainFrm.devicelist.cln3");
			this.devicelist.Columns[4].Text = lp.GetLanguage("MainFrm.devicelist.cln4");
			this.devicelist.Columns[5].Text = lp.GetLanguage("MainFrm.devicelist.cln5");
			this.rdoCleanAll.Text = lp.GetLanguage("MainFrm.rdoCleanAll");
			this.rdoSaveUserData.Text = lp.GetLanguage("MainFrm.rdoSaveUserData");
			this.rdoCleanAllAndLock.Text = lp.GetLanguage("MainFrm.rdoCleanAllAndLock");
		}

		// Token: 0x0600009A RID: 154 RVA: 0x0000AC58 File Offset: 0x00008E58
		private void miFlashConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ConfigurationFrm
			{
				Owner = this
			}.Show();
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000AC78 File Offset: 0x00008E78
		private void driverTsmItem_Click(object sender, EventArgs e)
		{
			DriverFrm dFrm = new DriverFrm();
			dFrm.Show();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x0000AC94 File Offset: 0x00008E94
		private void comportToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ComPortConfig
			{
				Owner = this
			}.Show();
		}

		// Token: 0x0600009D RID: 157 RVA: 0x0000ACB4 File Offset: 0x00008EB4
		private void reDrawProgress()
		{
			Rectangle SizeR = default(Rectangle);
			foreach (object obj in this.devicelist.Items)
			{
				ListViewItem ditem = (ListViewItem)obj;
				string deviceName = ditem.SubItems[1].Text;
				foreach (object obj2 in this.devicelist.Controls)
				{
					Control item = (Control)obj2;
					if (item.Name.IndexOf("progressbar") >= 0)
					{
						ProgressBar bar = (ProgressBar)item;
						if (bar.Name == deviceName + "progressbar")
						{
							SizeR = bar.Bounds;
							SizeR.Width = this.devicelist.Columns[2].Width;
							SizeR.Y = ditem.Bounds.Y;
							bar.SetBounds(this.devicelist.Items[0].SubItems[2].Bounds.X, SizeR.Y, SizeR.Width, SizeR.Height);
						}
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x0000AE58 File Offset: 0x00009058
		private void checkSha256ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ValidationFrm
			{
				Owner = this
			}.Show();
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000AE78 File Offset: 0x00009078
		public void CheckSha256()
		{
			this.timer_updateStatus.Enabled = true;
			foreach (Device d in FlashingDevice.flashDeviceList)
			{
				if (d.IsDone == null || d.IsDone.Value)
				{
					d.StartTime = DateTime.Now;
					d.Status = "flashing";
					d.Progress = 0f;
					d.IsDone = new bool?(false);
					d.IsUpdate = true;
					DeviceCtrl deviceCtrl = d.DeviceCtrl;
					deviceCtrl.deviceName = d.Name;
					deviceCtrl.swPath = this.txtPath.Text.Trim();
					deviceCtrl.sha256Path = this.ValidateSpecifyXml;
					new Thread(new ThreadStart(deviceCtrl.CheckSha256))
					{
						IsBackground = true
					}.Start();
				}
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000AF84 File Offset: 0x00009184
		private void flashLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string logPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log\\";
			if (Directory.Exists(logPath))
			{
				Process.Start("Explorer.exe", logPath);
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000AFC0 File Offset: 0x000091C0
		private void fastbootLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!this.timer_updateStatus.Enabled)
			{
				this.timer_updateStatus.Enabled = true;
			}
			this.deviceArr = UsbDevice.GetDevice();
			foreach (Device d in FlashingDevice.flashDeviceList)
			{
				d.IsDone = new bool?(false);
				d.IsUpdate = true;
				d.Status = "grabbing log";
				d.Progress = 0f;
				d.StartTime = DateTime.Now;
				Thread thread = new Thread(new ThreadStart(new ScriptDevice
				{
					deviceName = d.Name
				}.GrapLog));
				thread.Start();
				thread.IsBackground = true;
				FlashingDevice.UpdateDeviceStatus(d.Name, default(float?), "start grab log", "grabbing log", false);
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000B0BC File Offset: 0x000092BC
		private void authenticationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				PInvokeResultArg arg = new PInvokeResultArg();
				MiUserInfo info = new MiUserInfo();
				arg = EDL_SLA_Challenge.GetUserInfo(info);
				if (arg.result == 0)
				{
					this.lblAccount.Text = info.name.Trim();
					arg = EDL_SLA_Challenge.AuthFlash();
					if (arg.result == 1)
					{
						this.canFlash = true;
					}
					else
					{
						string msg = "Authentication required.";
						MessageBox.Show(msg);
						Log.w(string.Format("{0} errcode:{1} lasterr:{2}", msg, arg.lastErrCode, arg.lastErrMsg));
					}
				}
				else
				{
					string msg = "Login failed.";
					MessageBox.Show(msg);
					Log.w(string.Format("{0} errcode:{1} lasterr:{2}", msg, arg.lastErrCode, arg.lastErrMsg));
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message;
				Log.w(string.Format("authentication edl error:{0}", msg));
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000B1A4 File Offset: 0x000093A4
		private void rdoCleanAll_Click(object sender, EventArgs e)
		{
			if (this.rdoCleanAll.IsChecked)
			{
				this.script = FlashType.CleanAll;
				MiAppConfig.SetValue("script", this.script);
			}
			this.cmbScriptItem.SetText(this.script);
			if (Directory.Exists(this.txtPath.Text))
			{
				this.SetScriptItems(this.txtPath.Text);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000B210 File Offset: 0x00009410
		private void rdoSaveUserData_Click(object sender, EventArgs e)
		{
			if (this.rdoSaveUserData.IsChecked)
			{
				if (!Directory.Exists(this.txtPath.Text))
				{
					return;
				}
				string[] searchFiles = FileSearcher.SearchFiles(this.txtPath.Text, FlashType.SaveUserData);
				if (searchFiles.Length == 0)
				{
					MessageBox.Show("couldn't find script.");
					return;
				}
				this.script = Path.GetFileName(searchFiles[0]);
				MiAppConfig.SetValue("script", this.script);
			}
			this.cmbScriptItem.SetText(this.script);
			if (Directory.Exists(this.txtPath.Text))
			{
				this.SetScriptItems(this.txtPath.Text);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000B2B8 File Offset: 0x000094B8
		private void rdoCleanAllAndLock_Click(object sender, EventArgs e)
		{
			if (this.rdoCleanAllAndLock.IsChecked)
			{
				this.script = FlashType.CleanAllAndLock;
				MiAppConfig.SetValue("script", this.script);
			}
			this.cmbScriptItem.SetText(this.script);
			if (Directory.Exists(this.txtPath.Text))
			{
				this.SetScriptItems(this.txtPath.Text);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000B324 File Offset: 0x00009524
		private void cmbScriptItem_TextChanged(object sender, EventArgs e)
		{
			this.script = this.cmbScriptItem.SelectedText;
			FileSearcher.SearchFiles(this.txtPath.Text, this.script);
			if (this.script == FlashType.CleanAll)
			{
				this.rdoCleanAll.IsChecked = true;
			}
			else if (this.script.IndexOf("flash_all_except") >= 0)
			{
				this.rdoSaveUserData.IsChecked = true;
			}
			else if (this.script == FlashType.CleanAllAndLock)
			{
				this.rdoCleanAllAndLock.IsChecked = true;
			}
			else
			{
				this.rdoCleanAll.IsChecked = false;
				this.rdoSaveUserData.IsChecked = false;
				this.rdoCleanAllAndLock.IsChecked = false;
			}
			MiAppConfig.SetValue("script", this.script);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000B3EE File Offset: 0x000095EE
		private void txtPath_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000B3F0 File Offset: 0x000095F0
		private void StartAutoFlash()
		{
			try
			{
				IPAddress ip = IPAddress.Any;
				this.serverSocket = new Socket(2, 1, 6);
				this.serverSocket.Bind(new IPEndPoint(ip, this.myProt));
				this.serverSocket.Listen(10);
				this.SetLogText(string.Format("start listen {0} successfully", this.serverSocket.LocalEndPoint.ToString()));
				this.serverSocket.BeginAccept(new AsyncCallback(this.OnConnectRequest), this.serverSocket);
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000B494 File Offset: 0x00009694
		private void cmbDlType_SelectedValueChanged(object sender, EventArgs e)
		{
			MiAppConfig.SetValue("dlType", this.cmbDlType.SelectedItem.ToString());
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000B4B0 File Offset: 0x000096B0
		private void cmbChkSum_SelectedValueChanged(object sender, EventArgs e)
		{
			MiAppConfig.SetValue("chkSum", this.cmbChkSum.SelectedItem.ToString());
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000B508 File Offset: 0x00009708
		private void RecMsg(string recStr)
		{
			string actStr = recStr.Replace("$", "#");
			foreach (string recStr in actStr.Split(new char[]
			{
				'#'
			}))
			{
				if (!string.IsNullOrEmpty(recStr))
				{
					if (recStr.IndexOf("READY") >= 0)
					{
						this.SetLogText("start load devices");
						this.btnRefresh.BeginInvoke(delegate(string msg)
						{
							this.btnRefresh_Click(this.btnRefresh, new EventArgs());
						}, new object[]
						{
							""
						});
					}
					else if (recStr.IndexOf("START") >= 0)
					{
						this.SetLogText("start load devices");
						this.deviceArr = UsbDevice.GetDevice();
						this.btnRefresh.BeginInvoke(delegate(string msg)
						{
							this.btnRefresh_Click(this.btnRefresh, new EventArgs());
						}, new object[]
						{
							""
						});
						string pos = recStr.Replace("START", "");
						if (!string.IsNullOrEmpty(pos))
						{
							this.SetLogText("check usb " + pos);
							int index = 0;
							if (!string.IsNullOrEmpty(pos))
							{
								try
								{
									index = int.Parse(pos);
								}
								catch (Exception e)
								{
									this.SetLogText(e.Message);
								}
							}
							bool usbFound = false;
							foreach (Device d in this.deviceArr)
							{
								if (d.Index == index)
								{
									usbFound = true;
									break;
								}
							}
							string usbStatus = "#USB" + index.ToString();
							if (usbFound)
							{
								usbStatus += "ON$";
							}
							else
							{
								usbStatus += "OFF$";
							}
							this.SetLogText(string.Format("send :{0}", usbStatus));
							this.m_aryClients[0].Sock.Send(Encoding.ASCII.GetBytes(usbStatus));
						}
					}
					else if (recStr.IndexOf("LOAD") >= 0)
					{
						this.SetLogText("start flash");
						this.btnFlash.BeginInvoke(delegate(string msg)
						{
							this.btnFlash_Click(this.btnFlash, new EventArgs());
						}, new object[]
						{
							""
						});
					}
				}
			}
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000B780 File Offset: 0x00009980
		private void MainFrm_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == 65)
			{
				this.devicelist.Height = 270;
				this.txtLog.Visible = true;
				if (this.serverSocket == null)
				{
					this.isAutoFlash = true;
					this.StartAutoFlash();
					return;
				}
			}
			else
			{
				if (e.KeyCode == 82 && e.Alt)
				{
					e.Handled = true;
					this.btnRefresh.PerformClick();
					return;
				}
				if (e.KeyCode == 70 && e.Alt)
				{
					e.Handled = true;
					this.btnFlash.PerformClick();
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000B814 File Offset: 0x00009A14
		public void OnConnectRequest(IAsyncResult ar)
		{
			try
			{
				Socket listener = (Socket)ar.AsyncState;
				this.NewConnection(listener.EndAccept(ar));
				listener.BeginAccept(new AsyncCallback(this.OnConnectRequest), listener);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000B864 File Offset: 0x00009A64
		public void NewConnection(Socket sockClient)
		{
			SocketChatClient client = new SocketChatClient(sockClient);
			this.m_aryClients.Add(client);
			this.SetLogText(string.Format("Client {0}, joined", client.Sock.RemoteEndPoint));
			string strDateLine = "Welcome " + DateTime.Now.ToString("G") + "\n\r";
			byte[] byteDateLine = Encoding.ASCII.GetBytes(strDateLine.ToCharArray());
			client.Sock.Send(byteDateLine, byteDateLine.Length, 0);
			client.SetupRecieveCallback(this);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000B8EC File Offset: 0x00009AEC
		public void OnRecievedData(IAsyncResult ar)
		{
			SocketChatClient client = (SocketChatClient)ar.AsyncState;
			byte[] aryRet = client.GetRecievedData(ar);
			if (aryRet.Length < 1)
			{
				this.SetLogText(string.Format("Client {0}, disconnected", client.Sock.RemoteEndPoint));
				client.Sock.Close();
				this.m_aryClients.Remove(client);
				return;
			}
			string recStr = Encoding.ASCII.GetString(aryRet);
			this.RecMsg(recStr);
			client.SetupRecieveCallback(this);
		}

		// Token: 0x04000060 RID: 96
		private USB miUsb = new USB();

		// Token: 0x04000061 RID: 97
		private string validateSpecifyXml;

		// Token: 0x04000062 RID: 98
		private bool readbackverify;

		// Token: 0x04000063 RID: 99
		private bool openwritedump;

		// Token: 0x04000064 RID: 100
		private bool openreaddump;

		// Token: 0x04000065 RID: 101
		private bool verbose;

		// Token: 0x04000066 RID: 102
		public string chip;

		// Token: 0x04000067 RID: 103
		private int downloadEclipse;

		// Token: 0x04000068 RID: 104
		private Timer eclipseTimer;

		// Token: 0x04000069 RID: 105
		private List<string> comportList = new List<string>();

		// Token: 0x0400006A RID: 106
		private bool autodetectdevice;

		// Token: 0x0400006B RID: 107
		private string script = "";

		// Token: 0x0400006C RID: 108
		private List<Device> deviceArr = new List<Device>();

		// Token: 0x0400006D RID: 109
		private byte[] result = new byte[1024];

		// Token: 0x0400006E RID: 110
		private int myProt = 6002;

		// Token: 0x0400006F RID: 111
		public bool isAutoFlash;

		// Token: 0x04000070 RID: 112
		private bool isFactory;

		// Token: 0x04000071 RID: 113
		public string factory = string.Empty;

		// Token: 0x04000072 RID: 114
		private Socket serverSocket;

		// Token: 0x04000073 RID: 115
		private Socket clientSocket;

		// Token: 0x04000074 RID: 116
		private bool canFlash = true;

		// Token: 0x04000075 RID: 117
		private List<Thread> threads = new List<Thread>();

		// Token: 0x04000076 RID: 118
		private List<SocketChatClient> m_aryClients = new List<SocketChatClient>();

		// Token: 0x04000077 RID: 119
		private ProcessFrm frm = new ProcessFrm();
	}
}

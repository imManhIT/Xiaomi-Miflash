namespace XiaoMiFlash
{
	// Token: 0x02000013 RID: 19
	public partial class MainFrm : global::XiaoMiFlash.MiBaseFrm
	{
		// Token: 0x0600006B RID: 107 RVA: 0x0000696D File Offset: 0x00004B6D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000698C File Offset: 0x00004B8C
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::XiaoMiFlash.MainFrm));
			this.txtPath = new global::System.Windows.Forms.TextBox();
			this.btnBrwDic = new global::System.Windows.Forms.Button();
			this.fbdSelect = new global::System.Windows.Forms.FolderBrowserDialog();
			this.btnRefresh = new global::System.Windows.Forms.Button();
			this.btnFlash = new global::System.Windows.Forms.Button();
			this.devicelist = new global::System.Windows.Forms.ListView();
			this.clnID = new global::System.Windows.Forms.ColumnHeader();
			this.clnDevice = new global::System.Windows.Forms.ColumnHeader();
			this.clnProgress = new global::System.Windows.Forms.ColumnHeader();
			this.clnTime = new global::System.Windows.Forms.ColumnHeader();
			this.clnStatus = new global::System.Windows.Forms.ColumnHeader();
			this.clnResult = new global::System.Windows.Forms.ColumnHeader();
			this.txtLog = new global::System.Windows.Forms.RichTextBox();
			this.timer_updateStatus = new global::System.Windows.Forms.Timer(this.components);
			this.statusStrp = new global::System.Windows.Forms.StatusStrip();
			this.lblAccount = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.statusTab = new global::System.Windows.Forms.ToolStripStatusLabel();
			this.rdoCleanAll = new global::XiaoMiFlash.code.MiControl.RadioStripItem();
			this.rdoSaveUserData = new global::XiaoMiFlash.code.MiControl.RadioStripItem();
			this.rdoCleanAllAndLock = new global::XiaoMiFlash.code.MiControl.RadioStripItem();
			this.cmbScriptItem = new global::XiaoMiFlash.code.MiControl.ComboBoxStripItem();
			this.lblMD5 = new global::System.Windows.Forms.Label();
			this.mnsAuth = new global::System.Windows.Forms.MenuStrip();
			this.miConfiguration = new global::System.Windows.Forms.ToolStripMenuItem();
			this.miFlashConfigurationToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.driverTsmItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.otherToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.checkSha256ToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.logToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.flashLogToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.fastbootLogToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.comportToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.authenticationToolStripMenuItem = new global::System.Windows.Forms.ToolStripMenuItem();
			this.pnlQcom = new global::System.Windows.Forms.Panel();
			this.btnAutoFlash = new global::System.Windows.Forms.Button();
			this.pnlMTK = new global::System.Windows.Forms.Panel();
			this.cmbChkSum = new global::System.Windows.Forms.ComboBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label1 = new global::System.Windows.Forms.Label();
			this.btnDa = new global::System.Windows.Forms.Button();
			this.txtDa = new global::System.Windows.Forms.TextBox();
			this.btnStop = new global::System.Windows.Forms.Button();
			this.cmbDlType = new global::System.Windows.Forms.ComboBox();
			this.btnDownload = new global::System.Windows.Forms.Button();
			this.txtScatter = new global::System.Windows.Forms.TextBox();
			this.btnSelectScatter = new global::System.Windows.Forms.Button();
			this.statusStrp.SuspendLayout();
			this.mnsAuth.SuspendLayout();
			this.pnlQcom.SuspendLayout();
			this.pnlMTK.SuspendLayout();
			base.SuspendLayout();
			this.txtPath.Anchor = 15;
			this.txtPath.Location = new global::System.Drawing.Point(86, 23);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new global::System.Drawing.Size(699, 21);
			this.txtPath.TabIndex = 0;
			this.txtPath.TextChanged += new global::System.EventHandler(this.txtPath_TextChanged);
			this.btnBrwDic.Location = new global::System.Drawing.Point(9, 21);
			this.btnBrwDic.Name = "btnBrwDic";
			this.btnBrwDic.Size = new global::System.Drawing.Size(75, 23);
			this.btnBrwDic.TabIndex = 1;
			this.btnBrwDic.Text = "选择";
			this.btnBrwDic.UseVisualStyleBackColor = true;
			this.btnBrwDic.Click += new global::System.EventHandler(this.btnBrwDic_Click);
			this.fbdSelect.Description = "Please select sw path";
			this.btnRefresh.Anchor = 9;
			this.btnRefresh.Location = new global::System.Drawing.Point(827, 18);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new global::System.Drawing.Size(89, 23);
			this.btnRefresh.TabIndex = 2;
			this.btnRefresh.Text = "加载设备(R)";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new global::System.EventHandler(this.btnRefresh_Click);
			this.btnFlash.Anchor = 9;
			this.btnFlash.Location = new global::System.Drawing.Point(970, 18);
			this.btnFlash.Name = "btnFlash";
			this.btnFlash.Size = new global::System.Drawing.Size(84, 23);
			this.btnFlash.TabIndex = 3;
			this.btnFlash.Text = "刷机(F)";
			this.btnFlash.UseVisualStyleBackColor = true;
			this.btnFlash.Click += new global::System.EventHandler(this.btnFlash_Click);
			this.devicelist.Anchor = 14;
			this.devicelist.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.clnID,
				this.clnDevice,
				this.clnProgress,
				this.clnTime,
				this.clnStatus,
				this.clnResult
			});
			this.devicelist.GridLines = true;
			this.devicelist.Location = new global::System.Drawing.Point(21, 155);
			this.devicelist.Name = "devicelist";
			this.devicelist.Size = new global::System.Drawing.Size(1066, 428);
			this.devicelist.TabIndex = 4;
			this.devicelist.UseCompatibleStateImageBehavior = false;
			this.devicelist.View = 1;
			this.devicelist.ColumnWidthChanging += new global::System.Windows.Forms.ColumnWidthChangingEventHandler(this.devicelist_ColumnWidthChanging);
			this.clnID.Text = "编号";
			this.clnDevice.Text = "设备";
			this.clnDevice.Width = 90;
			this.clnProgress.Text = "进度";
			this.clnProgress.Width = 107;
			this.clnTime.Text = "时间";
			this.clnStatus.Text = "状态";
			this.clnStatus.Width = 500;
			this.clnResult.Text = "结果";
			this.clnResult.Width = 126;
			this.txtLog.Anchor = 14;
			this.txtLog.Location = new global::System.Drawing.Point(21, 449);
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.Size = new global::System.Drawing.Size(1066, 134);
			this.txtLog.TabIndex = 6;
			this.txtLog.Text = global::XiaoMiFlash.Properties.Resources.txtPath;
			this.txtLog.Visible = false;
			this.timer_updateStatus.Interval = 500;
			this.timer_updateStatus.Tick += new global::System.EventHandler(this.timer_updateStatus_Tick);
			this.statusStrp.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.lblAccount,
				this.statusTab,
				this.rdoCleanAll,
				this.rdoSaveUserData,
				this.rdoCleanAllAndLock,
				this.cmbScriptItem
			});
			this.statusStrp.Location = new global::System.Drawing.Point(0, 601);
			this.statusStrp.Name = "statusStrp";
			this.statusStrp.Size = new global::System.Drawing.Size(1111, 27);
			this.statusStrp.TabIndex = 7;
			this.statusStrp.Text = "statusStrip1";
			this.lblAccount.AutoSize = false;
			this.lblAccount.Name = "lblAccount";
			this.lblAccount.Size = new global::System.Drawing.Size(100, 22);
			this.statusTab.AutoSize = false;
			this.statusTab.Name = "statusTab";
			this.statusTab.Size = new global::System.Drawing.Size(466, 22);
			this.statusTab.Spring = true;
			this.rdoCleanAll.IsChecked = false;
			this.rdoCleanAll.Name = "rdoCleanAll";
			this.rdoCleanAll.Size = new global::System.Drawing.Size(98, 25);
			this.rdoCleanAll.Text = "清除所有数据";
			this.rdoCleanAll.Click += new global::System.EventHandler(this.rdoCleanAll_Click);
			this.rdoSaveUserData.IsChecked = false;
			this.rdoSaveUserData.Name = "rdoSaveUserData";
			this.rdoSaveUserData.Size = new global::System.Drawing.Size(98, 25);
			this.rdoSaveUserData.Text = "保留用户数据";
			this.rdoSaveUserData.Click += new global::System.EventHandler(this.rdoSaveUserData_Click);
			this.rdoCleanAllAndLock.IsChecked = true;
			this.rdoCleanAllAndLock.Name = "rdoCleanAllAndLock";
			this.rdoCleanAllAndLock.Size = new global::System.Drawing.Size(134, 25);
			this.rdoCleanAllAndLock.Text = "清除全部数据并lock";
			this.rdoCleanAllAndLock.Click += new global::System.EventHandler(this.rdoCleanAllAndLock_Click);
			this.cmbScriptItem.Name = "cmbScriptItem";
			this.cmbScriptItem.Size = new global::System.Drawing.Size(200, 25);
			this.cmbScriptItem.TextChanged += new global::System.EventHandler(this.cmbScriptItem_TextChanged);
			this.lblMD5.AutoSize = true;
			this.lblMD5.BorderStyle = 2;
			this.lblMD5.Font = new global::System.Drawing.Font("宋体", 9f, 1, 3, 134);
			this.lblMD5.ForeColor = global::System.Drawing.Color.FromArgb(0, 192, 0);
			this.lblMD5.Location = new global::System.Drawing.Point(84, 54);
			this.lblMD5.Name = "lblMD5";
			this.lblMD5.Size = new global::System.Drawing.Size(2, 14);
			this.lblMD5.TabIndex = 8;
			this.mnsAuth.BackColor = global::System.Drawing.SystemColors.ControlLight;
			this.mnsAuth.GripStyle = 1;
			this.mnsAuth.Items.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.miConfiguration,
				this.driverTsmItem,
				this.otherToolStripMenuItem,
				this.logToolStripMenuItem,
				this.comportToolStripMenuItem,
				this.authenticationToolStripMenuItem
			});
			this.mnsAuth.Location = new global::System.Drawing.Point(0, 0);
			this.mnsAuth.Name = "mnsAuth";
			this.mnsAuth.RenderMode = 1;
			this.mnsAuth.Size = new global::System.Drawing.Size(1111, 25);
			this.mnsAuth.TabIndex = 9;
			this.mnsAuth.Text = "Authentication";
			this.miConfiguration.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.miFlashConfigurationToolStripMenuItem
			});
			this.miConfiguration.Name = "miConfiguration";
			this.miConfiguration.Size = new global::System.Drawing.Size(99, 21);
			this.miConfiguration.Text = "Configuration";
			this.miConfiguration.Visible = false;
			this.miFlashConfigurationToolStripMenuItem.Name = "miFlashConfigurationToolStripMenuItem";
			this.miFlashConfigurationToolStripMenuItem.Size = new global::System.Drawing.Size(203, 22);
			this.miFlashConfigurationToolStripMenuItem.Text = "MiFlash Configuration";
			this.miFlashConfigurationToolStripMenuItem.Click += new global::System.EventHandler(this.miFlashConfigurationToolStripMenuItem_Click);
			this.driverTsmItem.Name = "driverTsmItem";
			this.driverTsmItem.Size = new global::System.Drawing.Size(55, 21);
			this.driverTsmItem.Text = "Driver";
			this.driverTsmItem.Click += new global::System.EventHandler(this.driverTsmItem_Click);
			this.otherToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.checkSha256ToolStripMenuItem
			});
			this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
			this.otherToolStripMenuItem.Size = new global::System.Drawing.Size(53, 21);
			this.otherToolStripMenuItem.Text = "Other";
			this.otherToolStripMenuItem.Visible = false;
			this.checkSha256ToolStripMenuItem.Name = "checkSha256ToolStripMenuItem";
			this.checkSha256ToolStripMenuItem.Size = new global::System.Drawing.Size(157, 22);
			this.checkSha256ToolStripMenuItem.Text = "Check Sha256";
			this.checkSha256ToolStripMenuItem.Click += new global::System.EventHandler(this.checkSha256ToolStripMenuItem_Click);
			this.logToolStripMenuItem.DropDownItems.AddRange(new global::System.Windows.Forms.ToolStripItem[]
			{
				this.flashLogToolStripMenuItem,
				this.fastbootLogToolStripMenuItem
			});
			this.logToolStripMenuItem.Name = "logToolStripMenuItem";
			this.logToolStripMenuItem.Size = new global::System.Drawing.Size(42, 21);
			this.logToolStripMenuItem.Text = "Log";
			this.flashLogToolStripMenuItem.Name = "flashLogToolStripMenuItem";
			this.flashLogToolStripMenuItem.Size = new global::System.Drawing.Size(150, 22);
			this.flashLogToolStripMenuItem.Text = "Flash log";
			this.flashLogToolStripMenuItem.Click += new global::System.EventHandler(this.flashLogToolStripMenuItem_Click);
			this.fastbootLogToolStripMenuItem.Name = "fastbootLogToolStripMenuItem";
			this.fastbootLogToolStripMenuItem.Size = new global::System.Drawing.Size(150, 22);
			this.fastbootLogToolStripMenuItem.Text = "Fastboot log";
			this.fastbootLogToolStripMenuItem.Click += new global::System.EventHandler(this.fastbootLogToolStripMenuItem_Click);
			this.comportToolStripMenuItem.Name = "comportToolStripMenuItem";
			this.comportToolStripMenuItem.Size = new global::System.Drawing.Size(72, 21);
			this.comportToolStripMenuItem.Text = "Comport";
			this.comportToolStripMenuItem.Visible = false;
			this.comportToolStripMenuItem.Click += new global::System.EventHandler(this.comportToolStripMenuItem_Click);
			this.authenticationToolStripMenuItem.Name = "authenticationToolStripMenuItem";
			this.authenticationToolStripMenuItem.Size = new global::System.Drawing.Size(102, 21);
			this.authenticationToolStripMenuItem.Text = "Authentication";
			this.authenticationToolStripMenuItem.Visible = false;
			this.authenticationToolStripMenuItem.Click += new global::System.EventHandler(this.authenticationToolStripMenuItem_Click);
			this.pnlQcom.Anchor = 15;
			this.pnlQcom.Controls.Add(this.btnAutoFlash);
			this.pnlQcom.Controls.Add(this.txtPath);
			this.pnlQcom.Controls.Add(this.lblMD5);
			this.pnlQcom.Controls.Add(this.btnBrwDic);
			this.pnlQcom.Controls.Add(this.btnRefresh);
			this.pnlQcom.Controls.Add(this.btnFlash);
			this.pnlQcom.Location = new global::System.Drawing.Point(21, 28);
			this.pnlQcom.Name = "pnlQcom";
			this.pnlQcom.Size = new global::System.Drawing.Size(1063, 79);
			this.pnlQcom.TabIndex = 10;
			this.btnAutoFlash.Location = new global::System.Drawing.Point(828, 47);
			this.btnAutoFlash.Name = "btnAutoFlash";
			this.btnAutoFlash.Size = new global::System.Drawing.Size(75, 23);
			this.btnAutoFlash.TabIndex = 9;
			this.btnAutoFlash.Text = "AutoFlash";
			this.btnAutoFlash.UseVisualStyleBackColor = true;
			this.btnAutoFlash.Visible = false;
			this.btnAutoFlash.Click += new global::System.EventHandler(this.btnAutoFlash_Click);
			this.pnlMTK.Anchor = 15;
			this.pnlMTK.Controls.Add(this.cmbChkSum);
			this.pnlMTK.Controls.Add(this.label2);
			this.pnlMTK.Controls.Add(this.label1);
			this.pnlMTK.Controls.Add(this.btnDa);
			this.pnlMTK.Controls.Add(this.txtDa);
			this.pnlMTK.Controls.Add(this.btnStop);
			this.pnlMTK.Controls.Add(this.cmbDlType);
			this.pnlMTK.Controls.Add(this.btnDownload);
			this.pnlMTK.Controls.Add(this.txtScatter);
			this.pnlMTK.Controls.Add(this.btnSelectScatter);
			this.pnlMTK.Location = new global::System.Drawing.Point(21, 28);
			this.pnlMTK.Name = "pnlMTK";
			this.pnlMTK.Size = new global::System.Drawing.Size(1066, 117);
			this.pnlMTK.TabIndex = 11;
			this.pnlMTK.Visible = false;
			this.cmbChkSum.FormattingEnabled = true;
			this.cmbChkSum.Location = new global::System.Drawing.Point(384, 85);
			this.cmbChkSum.Name = "cmbChkSum";
			this.cmbChkSum.Size = new global::System.Drawing.Size(149, 20);
			this.cmbChkSum.TabIndex = 9;
			this.cmbChkSum.SelectedValueChanged += new global::System.EventHandler(this.cmbChkSum_SelectedValueChanged);
			this.label2.AutoSize = true;
			this.label2.Font = new global::System.Drawing.Font("宋体", 9f, 1, 3, 134);
			this.label2.Location = new global::System.Drawing.Point(310, 88);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(68, 12);
			this.label2.TabIndex = 8;
			this.label2.Text = "Check-Sum";
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("宋体", 9f, 1, 3, 134);
			this.label1.Location = new global::System.Drawing.Point(7, 88);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(96, 12);
			this.label1.TabIndex = 8;
			this.label1.Text = "Download-type";
			this.btnDa.Location = new global::System.Drawing.Point(9, 47);
			this.btnDa.Name = "btnDa";
			this.btnDa.Size = new global::System.Drawing.Size(92, 23);
			this.btnDa.TabIndex = 7;
			this.btnDa.Text = "选择DA";
			this.btnDa.UseVisualStyleBackColor = true;
			this.btnDa.Click += new global::System.EventHandler(this.btnDa_Click);
			this.txtDa.Location = new global::System.Drawing.Point(107, 48);
			this.txtDa.Name = "txtDa";
			this.txtDa.Size = new global::System.Drawing.Size(678, 21);
			this.txtDa.TabIndex = 6;
			this.btnStop.Location = new global::System.Drawing.Point(937, 8);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new global::System.Drawing.Size(75, 21);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new global::System.EventHandler(this.btnStop_Click);
			this.cmbDlType.FormattingEnabled = true;
			this.cmbDlType.Location = new global::System.Drawing.Point(107, 85);
			this.cmbDlType.Name = "cmbDlType";
			this.cmbDlType.Size = new global::System.Drawing.Size(152, 20);
			this.cmbDlType.TabIndex = 3;
			this.cmbDlType.SelectedValueChanged += new global::System.EventHandler(this.cmbDlType_SelectedValueChanged);
			this.btnDownload.Location = new global::System.Drawing.Point(810, 8);
			this.btnDownload.Name = "btnDownload";
			this.btnDownload.Size = new global::System.Drawing.Size(93, 23);
			this.btnDownload.TabIndex = 2;
			this.btnDownload.Text = "Download";
			this.btnDownload.UseVisualStyleBackColor = true;
			this.btnDownload.Click += new global::System.EventHandler(this.btnDownload_Click);
			this.txtScatter.Anchor = 15;
			this.txtScatter.Location = new global::System.Drawing.Point(107, 11);
			this.txtScatter.Name = "txtScatter";
			this.txtScatter.Size = new global::System.Drawing.Size(678, 21);
			this.txtScatter.TabIndex = 0;
			this.btnSelectScatter.Location = new global::System.Drawing.Point(8, 11);
			this.btnSelectScatter.Name = "btnSelectScatter";
			this.btnSelectScatter.Size = new global::System.Drawing.Size(93, 23);
			this.btnSelectScatter.TabIndex = 1;
			this.btnSelectScatter.Text = "选择Scatter";
			this.btnSelectScatter.UseVisualStyleBackColor = true;
			this.btnSelectScatter.Click += new global::System.EventHandler(this.btnSelectScatter_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = 1;
			base.ClientSize = new global::System.Drawing.Size(1111, 628);
			base.Controls.Add(this.pnlMTK);
			base.Controls.Add(this.pnlQcom);
			base.Controls.Add(this.statusStrp);
			base.Controls.Add(this.mnsAuth);
			base.Controls.Add(this.txtLog);
			base.Controls.Add(this.devicelist);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.MainMenuStrip = this.mnsAuth;
			base.Name = "MainFrm";
			base.StartPosition = 1;
			this.Text = "MiFlash 2017.4.25.0";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
			base.FormClosed += new global::System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
			base.Load += new global::System.EventHandler(this.MainFrm_Load);
			base.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.MainFrm_KeyDown);
			this.statusStrp.ResumeLayout(false);
			this.statusStrp.PerformLayout();
			this.mnsAuth.ResumeLayout(false);
			this.mnsAuth.PerformLayout();
			this.pnlQcom.ResumeLayout(false);
			this.pnlQcom.PerformLayout();
			this.pnlMTK.ResumeLayout(false);
			this.pnlMTK.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000031 RID: 49
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000032 RID: 50
		private global::System.Windows.Forms.TextBox txtPath;

		// Token: 0x04000033 RID: 51
		private global::System.Windows.Forms.Button btnBrwDic;

		// Token: 0x04000034 RID: 52
		private global::System.Windows.Forms.FolderBrowserDialog fbdSelect;

		// Token: 0x04000035 RID: 53
		private global::System.Windows.Forms.Button btnRefresh;

		// Token: 0x04000036 RID: 54
		private global::System.Windows.Forms.Button btnFlash;

		// Token: 0x04000037 RID: 55
		private global::System.Windows.Forms.ListView devicelist;

		// Token: 0x04000038 RID: 56
		private global::System.Windows.Forms.ColumnHeader clnID;

		// Token: 0x04000039 RID: 57
		private global::System.Windows.Forms.ColumnHeader clnDevice;

		// Token: 0x0400003A RID: 58
		private global::System.Windows.Forms.ColumnHeader clnProgress;

		// Token: 0x0400003B RID: 59
		private global::System.Windows.Forms.ColumnHeader clnTime;

		// Token: 0x0400003C RID: 60
		private global::System.Windows.Forms.ColumnHeader clnStatus;

		// Token: 0x0400003D RID: 61
		private global::System.Windows.Forms.ColumnHeader clnResult;

		// Token: 0x0400003E RID: 62
		private global::System.Windows.Forms.RichTextBox txtLog;

		// Token: 0x0400003F RID: 63
		private global::System.Windows.Forms.Timer timer_updateStatus;

		// Token: 0x04000040 RID: 64
		private global::System.Windows.Forms.StatusStrip statusStrp;

		// Token: 0x04000041 RID: 65
		private global::System.Windows.Forms.ToolStripStatusLabel statusTab;

		// Token: 0x04000042 RID: 66
		private global::XiaoMiFlash.code.MiControl.RadioStripItem rdoCleanAll;

		// Token: 0x04000043 RID: 67
		private global::XiaoMiFlash.code.MiControl.RadioStripItem rdoSaveUserData;

		// Token: 0x04000044 RID: 68
		private global::XiaoMiFlash.code.MiControl.RadioStripItem rdoCleanAllAndLock;

		// Token: 0x04000045 RID: 69
		private global::System.Windows.Forms.Label lblMD5;

		// Token: 0x04000046 RID: 70
		private global::System.Windows.Forms.MenuStrip mnsAuth;

		// Token: 0x04000047 RID: 71
		private global::System.Windows.Forms.ToolStripMenuItem miConfiguration;

		// Token: 0x04000048 RID: 72
		private global::System.Windows.Forms.ToolStripMenuItem miFlashConfigurationToolStripMenuItem;

		// Token: 0x04000049 RID: 73
		private global::System.Windows.Forms.ToolStripMenuItem driverTsmItem;

		// Token: 0x0400004A RID: 74
		private global::System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;

		// Token: 0x0400004B RID: 75
		private global::System.Windows.Forms.ToolStripMenuItem checkSha256ToolStripMenuItem;

		// Token: 0x0400004C RID: 76
		private global::System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;

		// Token: 0x0400004D RID: 77
		private global::XiaoMiFlash.code.MiControl.ComboBoxStripItem cmbScriptItem;

		// Token: 0x0400004E RID: 78
		private global::System.Windows.Forms.Panel pnlQcom;

		// Token: 0x0400004F RID: 79
		private global::System.Windows.Forms.Panel pnlMTK;

		// Token: 0x04000050 RID: 80
		private global::System.Windows.Forms.TextBox txtScatter;

		// Token: 0x04000051 RID: 81
		private global::System.Windows.Forms.Button btnSelectScatter;

		// Token: 0x04000052 RID: 82
		private global::System.Windows.Forms.Button btnDownload;

		// Token: 0x04000053 RID: 83
		private global::System.Windows.Forms.ComboBox cmbDlType;

		// Token: 0x04000054 RID: 84
		private global::System.Windows.Forms.Button btnStop;

		// Token: 0x04000055 RID: 85
		private global::System.Windows.Forms.ToolStripMenuItem comportToolStripMenuItem;

		// Token: 0x04000056 RID: 86
		private global::System.Windows.Forms.Button btnAutoFlash;

		// Token: 0x04000057 RID: 87
		private global::System.Windows.Forms.Button btnDa;

		// Token: 0x04000058 RID: 88
		private global::System.Windows.Forms.TextBox txtDa;

		// Token: 0x04000059 RID: 89
		private global::System.Windows.Forms.Label label1;

		// Token: 0x0400005A RID: 90
		private global::System.Windows.Forms.ComboBox cmbChkSum;

		// Token: 0x0400005B RID: 91
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400005C RID: 92
		private global::System.Windows.Forms.ToolStripMenuItem flashLogToolStripMenuItem;

		// Token: 0x0400005D RID: 93
		private global::System.Windows.Forms.ToolStripMenuItem fastbootLogToolStripMenuItem;

		// Token: 0x0400005E RID: 94
		private global::System.Windows.Forms.ToolStripStatusLabel lblAccount;

		// Token: 0x0400005F RID: 95
		private global::System.Windows.Forms.ToolStripMenuItem authenticationToolStripMenuItem;
	}
}

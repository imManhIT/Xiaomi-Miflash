namespace XiaoMiFlash.form
{
	// Token: 0x0200003F RID: 63
	public partial class ConfigurationFrm : global::System.Windows.Forms.Form
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x00010EC4 File Offset: 0x0000F0C4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00010EE4 File Offset: 0x0000F0E4
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::XiaoMiFlash.form.ConfigurationFrm));
			this.chkMD5 = new global::System.Windows.Forms.CheckBox();
			this.chkRbv = new global::System.Windows.Forms.CheckBox();
			this.chkWriteDump = new global::System.Windows.Forms.CheckBox();
			this.chkReadDump = new global::System.Windows.Forms.CheckBox();
			this.chkVerbose = new global::System.Windows.Forms.CheckBox();
			this.btnOK = new global::System.Windows.Forms.Button();
			this.chkAutoDetect = new global::System.Windows.Forms.CheckBox();
			this.label1 = new global::System.Windows.Forms.Label();
			this.cmbFactory = new global::System.Windows.Forms.ComboBox();
			this.cmbChip = new global::System.Windows.Forms.ComboBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.cmbMainProgram = new global::System.Windows.Forms.ComboBox();
			base.SuspendLayout();
			this.chkMD5.AutoSize = true;
			this.chkMD5.Location = new global::System.Drawing.Point(53, 42);
			this.chkMD5.Name = "chkMD5";
			this.chkMD5.Size = new global::System.Drawing.Size(156, 16);
			this.chkMD5.TabIndex = 0;
			this.chkMD5.Text = "check MD5 before flash";
			this.chkMD5.UseVisualStyleBackColor = true;
			this.chkMD5.CheckedChanged += new global::System.EventHandler(this.chkMD5_CheckedChanged);
			this.chkRbv.AutoSize = true;
			this.chkRbv.Location = new global::System.Drawing.Point(53, 109);
			this.chkRbv.Name = "chkRbv";
			this.chkRbv.Size = new global::System.Drawing.Size(120, 16);
			this.chkRbv.TabIndex = 1;
			this.chkRbv.Text = "Read Back Verify";
			this.chkRbv.UseVisualStyleBackColor = true;
			this.chkRbv.CheckedChanged += new global::System.EventHandler(this.chkRbv_CheckedChanged);
			this.chkWriteDump.AutoSize = true;
			this.chkWriteDump.Location = new global::System.Drawing.Point(53, 143);
			this.chkWriteDump.Name = "chkWriteDump";
			this.chkWriteDump.Size = new global::System.Drawing.Size(114, 16);
			this.chkWriteDump.TabIndex = 2;
			this.chkWriteDump.Text = "Open Write Dump";
			this.chkWriteDump.UseVisualStyleBackColor = true;
			this.chkWriteDump.CheckedChanged += new global::System.EventHandler(this.chkWriteDump_CheckedChanged);
			this.chkReadDump.AutoSize = true;
			this.chkReadDump.Location = new global::System.Drawing.Point(53, 74);
			this.chkReadDump.Name = "chkReadDump";
			this.chkReadDump.Size = new global::System.Drawing.Size(108, 16);
			this.chkReadDump.TabIndex = 2;
			this.chkReadDump.Text = "Open Read Dump";
			this.chkReadDump.UseVisualStyleBackColor = true;
			this.chkReadDump.CheckedChanged += new global::System.EventHandler(this.chkReadDump_CheckedChanged);
			this.chkVerbose.AutoSize = true;
			this.chkVerbose.Location = new global::System.Drawing.Point(53, 177);
			this.chkVerbose.Name = "chkVerbose";
			this.chkVerbose.Size = new global::System.Drawing.Size(66, 16);
			this.chkVerbose.TabIndex = 3;
			this.chkVerbose.Text = "Verbose";
			this.chkVerbose.UseVisualStyleBackColor = true;
			this.chkVerbose.CheckedChanged += new global::System.EventHandler(this.chkVerbose_CheckedChanged);
			this.btnOK.Location = new global::System.Drawing.Point(53, 228);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new global::System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new global::System.EventHandler(this.btnOK_Click);
			this.chkAutoDetect.AutoSize = true;
			this.chkAutoDetect.Location = new global::System.Drawing.Point(275, 42);
			this.chkAutoDetect.Name = "chkAutoDetect";
			this.chkAutoDetect.Size = new global::System.Drawing.Size(186, 16);
			this.chkAutoDetect.TabIndex = 5;
			this.chkAutoDetect.Text = "Detect Device Automatically";
			this.chkAutoDetect.UseVisualStyleBackColor = true;
			this.chkAutoDetect.Visible = false;
			this.chkAutoDetect.CheckedChanged += new global::System.EventHandler(this.checkBox1_CheckedChanged);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(275, 74);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(53, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "Factory:";
			this.cmbFactory.FormattingEnabled = true;
			this.cmbFactory.Items.AddRange(new object[]
			{
				"please choose",
				"Foxconn",
				"Inventec"
			});
			this.cmbFactory.Location = new global::System.Drawing.Point(334, 72);
			this.cmbFactory.Name = "cmbFactory";
			this.cmbFactory.Size = new global::System.Drawing.Size(121, 20);
			this.cmbFactory.TabIndex = 7;
			this.cmbFactory.SelectedValueChanged += new global::System.EventHandler(this.cmbFactory_SelectedValueChanged);
			this.cmbChip.FormattingEnabled = true;
			this.cmbChip.Items.AddRange(new object[]
			{
				"Qualcomm",
				"MTK"
			});
			this.cmbChip.Location = new global::System.Drawing.Point(334, 109);
			this.cmbChip.Name = "cmbChip";
			this.cmbChip.Size = new global::System.Drawing.Size(121, 20);
			this.cmbChip.TabIndex = 8;
			this.cmbChip.SelectedValueChanged += new global::System.EventHandler(this.cmbChip_SelectedValueChanged);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(293, 112);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(35, 12);
			this.label2.TabIndex = 9;
			this.label2.Text = "Chip:";
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(251, 143);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(77, 12);
			this.label3.TabIndex = 10;
			this.label3.Text = "Main program";
			this.cmbMainProgram.FormattingEnabled = true;
			this.cmbMainProgram.Items.AddRange(new object[]
			{
				"xiaomi",
				"fh_loader"
			});
			this.cmbMainProgram.Location = new global::System.Drawing.Point(335, 143);
			this.cmbMainProgram.Name = "cmbMainProgram";
			this.cmbMainProgram.Size = new global::System.Drawing.Size(121, 20);
			this.cmbMainProgram.TabIndex = 11;
			this.cmbMainProgram.SelectedValueChanged += new global::System.EventHandler(this.cmbMainProgram_SelectedValueChanged);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = 1;
			base.ClientSize = new global::System.Drawing.Size(548, 300);
			base.Controls.Add(this.cmbMainProgram);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.cmbChip);
			base.Controls.Add(this.cmbFactory);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.chkAutoDetect);
			base.Controls.Add(this.btnOK);
			base.Controls.Add(this.chkVerbose);
			base.Controls.Add(this.chkReadDump);
			base.Controls.Add(this.chkWriteDump);
			base.Controls.Add(this.chkRbv);
			base.Controls.Add(this.chkMD5);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "ConfigurationFrm";
			base.StartPosition = 1;
			this.Text = "Configuration";
			base.Load += new global::System.EventHandler(this.ConfigurationFrm_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400013E RID: 318
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400013F RID: 319
		private global::System.Windows.Forms.CheckBox chkMD5;

		// Token: 0x04000140 RID: 320
		private global::System.Windows.Forms.CheckBox chkRbv;

		// Token: 0x04000141 RID: 321
		private global::System.Windows.Forms.CheckBox chkWriteDump;

		// Token: 0x04000142 RID: 322
		private global::System.Windows.Forms.CheckBox chkReadDump;

		// Token: 0x04000143 RID: 323
		private global::System.Windows.Forms.CheckBox chkVerbose;

		// Token: 0x04000144 RID: 324
		private global::System.Windows.Forms.Button btnOK;

		// Token: 0x04000145 RID: 325
		private global::System.Windows.Forms.CheckBox chkAutoDetect;

		// Token: 0x04000146 RID: 326
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000147 RID: 327
		private global::System.Windows.Forms.ComboBox cmbFactory;

		// Token: 0x04000148 RID: 328
		private global::System.Windows.Forms.ComboBox cmbChip;

		// Token: 0x04000149 RID: 329
		private global::System.Windows.Forms.Label label2;

		// Token: 0x0400014A RID: 330
		private global::System.Windows.Forms.Label label3;

		// Token: 0x0400014B RID: 331
		private global::System.Windows.Forms.ComboBox cmbMainProgram;
	}
}

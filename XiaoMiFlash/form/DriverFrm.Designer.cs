namespace XiaoMiFlash.form
{
	// Token: 0x0200000A RID: 10
	public partial class DriverFrm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002FBD File Offset: 0x000011BD
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002FDC File Offset: 0x000011DC
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::XiaoMiFlash.form.DriverFrm));
			this.DriverBox = new global::System.Windows.Forms.GroupBox();
			this.lblDriver = new global::System.Windows.Forms.Label();
			this.btnInstall = new global::System.Windows.Forms.Button();
			this.DriverBox.SuspendLayout();
			base.SuspendLayout();
			this.DriverBox.Controls.Add(this.btnInstall);
			this.DriverBox.Controls.Add(this.lblDriver);
			this.DriverBox.Location = new global::System.Drawing.Point(72, 52);
			this.DriverBox.Name = "DriverBox";
			this.DriverBox.Size = new global::System.Drawing.Size(534, 227);
			this.DriverBox.TabIndex = 0;
			this.DriverBox.TabStop = false;
			this.DriverBox.Text = "Driver";
			this.lblDriver.AutoSize = true;
			this.lblDriver.Location = new global::System.Drawing.Point(42, 42);
			this.lblDriver.Name = "lblDriver";
			this.lblDriver.Size = new global::System.Drawing.Size(0, 12);
			this.lblDriver.TabIndex = 0;
			this.btnInstall.Location = new global::System.Drawing.Point(44, 192);
			this.btnInstall.Name = "btnInstall";
			this.btnInstall.Size = new global::System.Drawing.Size(75, 23);
			this.btnInstall.TabIndex = 1;
			this.btnInstall.Text = "Reinstall";
			this.btnInstall.UseVisualStyleBackColor = true;
			this.btnInstall.Click += new global::System.EventHandler(this.btnInstall_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = 1;
			base.ClientSize = new global::System.Drawing.Size(729, 409);
			base.Controls.Add(this.DriverBox);
			base.Icon = (global::System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "DriverFrm";
			base.StartPosition = 1;
			this.Text = "Driver";
			base.Load += new global::System.EventHandler(this.DriverFrm_Load);
			this.DriverBox.ResumeLayout(false);
			this.DriverBox.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000010 RID: 16
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.GroupBox DriverBox;

		// Token: 0x04000012 RID: 18
		private global::System.Windows.Forms.Label lblDriver;

		// Token: 0x04000013 RID: 19
		private global::System.Windows.Forms.Button btnInstall;
	}
}

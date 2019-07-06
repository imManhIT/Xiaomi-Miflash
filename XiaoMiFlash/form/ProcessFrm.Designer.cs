namespace XiaoMiFlash.form
{
	// Token: 0x02000026 RID: 38
	public partial class ProcessFrm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000105 RID: 261 RVA: 0x0000D889 File Offset: 0x0000BA89
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000D8A8 File Offset: 0x0000BAA8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager resources = new global::System.ComponentModel.ComponentResourceManager(typeof(global::XiaoMiFlash.form.ProcessFrm));
			this.label1 = new global::System.Windows.Forms.Label();
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.pictureBox1.BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(91, 29);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(89, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "MD5 validation";
			this.pictureBox1.Image = (global::System.Drawing.Image)resources.GetObject("pictureBox1.Image");
			this.pictureBox1.Location = new global::System.Drawing.Point(103, 92);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(100, 50);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = 1;
			this.AutoSize = true;
			base.ClientSize = new global::System.Drawing.Size(284, 262);
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = 0;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "processFrm";
			base.StartPosition = 1;
			this.Text = "processFrm";
			base.TopMost = true;
			this.pictureBox1.EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040000BA RID: 186
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040000BB RID: 187
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040000BC RID: 188
		private global::System.Windows.Forms.PictureBox pictureBox1;
	}
}

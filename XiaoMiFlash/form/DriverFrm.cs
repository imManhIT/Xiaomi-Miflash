using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using XiaoMiFlash.code.bl;

namespace XiaoMiFlash.form
{
	// Token: 0x0200000A RID: 10
	public partial class DriverFrm : Form
	{
		// Token: 0x06000034 RID: 52 RVA: 0x00002EDA File Offset: 0x000010DA
		public DriverFrm()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002EE8 File Offset: 0x000010E8
		private void DriverFrm_Load(object sender, EventArgs e)
		{
			MiDriver miDriver = new MiDriver();
			for (int i = 0; i < miDriver.infFiles.Length; i++)
			{
				Label label = this.lblDriver;
				label.Text += string.Format("({0}) {1}\r\n", i + 1, miDriver.infFiles[i]);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F40 File Offset: 0x00001140
		private void btnInstall_Click(object sender, EventArgs e)
		{
			string text = this.btnInstall.Text;
			this.btnInstall.Text = "Wait......";
			this.btnInstall.Enabled = false;
			string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			MiDriver miDriver = new MiDriver();
			miDriver.CopyFiles(path);
			miDriver.InstallAllDriver(path, true);
			this.btnInstall.Text = text;
			this.btnInstall.Enabled = true;
			MessageBox.Show("Install done");
		}
	}
}

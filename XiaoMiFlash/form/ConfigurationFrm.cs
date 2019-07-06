using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using XiaoMiFlash.code.bl;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.form
{
	// Token: 0x0200003F RID: 63
	public partial class ConfigurationFrm : Form
	{
		// Token: 0x060001B4 RID: 436 RVA: 0x0001178D File Offset: 0x0000F98D
		public ConfigurationFrm()
		{
			this.InitializeComponent();
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0001179C File Offset: 0x0000F99C
		private void chkMD5_CheckedChanged(object sender, EventArgs e)
		{
			MiAppConfig.SetValue("checkMD5", this.chkMD5.Checked.ToString());
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x000117C8 File Offset: 0x0000F9C8
		private void ConfigurationFrm_Load(object sender, EventArgs e)
		{
			this.chkMD5.Checked = (MiAppConfig.GetAppConfig("checkMD5").ToLower() == "true");
			this.mainFrm = (MainFrm)base.Owner;
			this.chkRbv.Checked = this.mainFrm.ReadBackVerify;
			this.chkWriteDump.Checked = this.mainFrm.OpenWriteDump;
			this.chkReadDump.Checked = this.mainFrm.OpenReadDump;
			this.chkVerbose.Checked = this.mainFrm.Verbose;
			this.chkAutoDetect.Checked = this.mainFrm.AutoDetectDevice;
			this.cmbFactory.SelectedItem = MiAppConfig.Get("factory").ToString();
			this.cmbChip.SelectedItem = MiAppConfig.Get("chip").ToString();
			this.cmbMainProgram.SelectedItem = MiAppConfig.Get("mainProgram").ToString();
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000118C6 File Offset: 0x0000FAC6
		private void chkRbv_CheckedChanged(object sender, EventArgs e)
		{
			this.mainFrm.ReadBackVerify = this.chkRbv.Checked;
			if (this.mainFrm.ReadBackVerify)
			{
				this.chkReadDump.Checked = true;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000118F7 File Offset: 0x0000FAF7
		private void chkWriteDump_CheckedChanged(object sender, EventArgs e)
		{
			this.mainFrm.OpenWriteDump = this.chkWriteDump.Checked;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0001190F File Offset: 0x0000FB0F
		private void chkReadDump_CheckedChanged(object sender, EventArgs e)
		{
			this.mainFrm.OpenReadDump = this.chkReadDump.Checked;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00011927 File Offset: 0x0000FB27
		private void chkVerbose_CheckedChanged(object sender, EventArgs e)
		{
			this.mainFrm.Verbose = this.chkVerbose.Checked;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0001193F File Offset: 0x0000FB3F
		private void btnOK_Click(object sender, EventArgs e)
		{
			base.Close();
			base.Dispose();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00011950 File Offset: 0x0000FB50
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.mainFrm.AutoDetectDevice = this.chkAutoDetect.Checked;
			this.mainFrm.AutoDetectUsb();
			Log.w("set AutoDetectDevice " + this.chkAutoDetect.Checked.ToString());
		}

		// Token: 0x060001BD RID: 445 RVA: 0x000119A0 File Offset: 0x0000FBA0
		private void cmbFactory_SelectedValueChanged(object sender, EventArgs e)
		{
			string factory = this.cmbFactory.SelectedItem.ToString();
			if (!(factory != "please choose"))
			{
				MiAppConfig.SetValue("factory", "");
				this.mainFrm.factory = "";
				this.mainFrm.SetFactory("");
				return;
			}
			if (FactoryCtrl.SetFactory(factory))
			{
				MiAppConfig.SetValue("factory", factory);
				this.mainFrm.factory = factory;
				this.mainFrm.SetFactory(factory);
				return;
			}
			MessageBox.Show("set factory failed!");
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00011A34 File Offset: 0x0000FC34
		private void cmbChip_SelectedValueChanged(object sender, EventArgs e)
		{
			string chip = this.cmbChip.SelectedItem.ToString();
			MiAppConfig.SetValue("chip", chip);
			this.mainFrm.chip = chip;
			this.mainFrm.SetChip(chip);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00011A78 File Offset: 0x0000FC78
		private void cmbMainProgram_SelectedValueChanged(object sender, EventArgs e)
		{
			string mainProgram = this.cmbMainProgram.SelectedItem.ToString();
			MiAppConfig.SetValue("mainProgram", mainProgram);
		}

		// Token: 0x0400014C RID: 332
		private MainFrm mainFrm;
	}
}

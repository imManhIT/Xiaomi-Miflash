using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace XiaoMiFlash.form
{
	// Token: 0x02000003 RID: 3
	public partial class ValidationFrm : Form
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000232D File Offset: 0x0000052D
		public ValidationFrm()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000233C File Offset: 0x0000053C
		private void btnCheckAll_Click(object sender, EventArgs e)
		{
			MainFrm mFrm = (MainFrm)base.Owner;
			mFrm.ValidateSpecifyXml = "";
			mFrm.RefreshDevice();
			mFrm.CheckSha256();
			base.Close();
			base.Dispose();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002378 File Offset: 0x00000578
		private void btnSpecify_Click(object sender, EventArgs e)
		{
			MainFrm mFrm = (MainFrm)base.Owner;
			this.openXmlFile.InitialDirectory = mFrm.SwPath;
			DialogResult result = this.openXmlFile.ShowDialog();
			if (result == 1)
			{
				string path = this.openXmlFile.FileName;
				mFrm.ValidateSpecifyXml = path;
				mFrm.RefreshDevice();
				mFrm.CheckSha256();
				base.Close();
				base.Dispose();
				return;
			}
			MessageBox.Show("Please select a xml file.");
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000023E9 File Offset: 0x000005E9
		private void ValidationFrm_Load(object sender, EventArgs e)
		{
		}
	}
}

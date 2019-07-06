using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.form
{
	// Token: 0x0200003A RID: 58
	public partial class ComPortConfig : Form
	{
		// Token: 0x0600017F RID: 383 RVA: 0x00010094 File Offset: 0x0000E294
		public ComPortConfig()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000180 RID: 384 RVA: 0x000100A4 File Offset: 0x0000E2A4
		private void ComPortConfig_Load(object sender, EventArgs e)
		{
			this.mainFrm = (MainFrm)base.Owner;
			string mtkComs = MiAppConfig.Get("mtkComs");
			string[] coms = mtkComs.Split(new char[]
			{
				','
			});
			int i = 0;
			foreach (object obj in this.groupBoxComs.Controls)
			{
				Control item = (Control)obj;
				if (i > coms.Length - 1)
				{
					break;
				}
				if (item is TextBox)
				{
					TextBox txtCom = item as TextBox;
					if (string.IsNullOrEmpty(txtCom.Text))
					{
						txtCom.Text = coms[i];
						i++;
					}
				}
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001016C File Offset: 0x0000E36C
		private void btnOK_Click(object sender, EventArgs e)
		{
			List<string> comList = new List<string>();
			foreach (object obj in this.groupBoxComs.Controls)
			{
				Control item = (Control)obj;
				if (item is TextBox)
				{
					TextBox txtCom = item as TextBox;
					if (!string.IsNullOrEmpty(txtCom.Text))
					{
						comList.Add(txtCom.Text.Trim());
					}
				}
			}
			comList.Sort();
			string mtkComs = string.Join(",", comList.ToArray());
			MiAppConfig.SetValue("mtkComs", mtkComs);
			base.Close();
		}

		// Token: 0x04000112 RID: 274
		private MainFrm mainFrm;
	}
}

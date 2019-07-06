using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace XiaoMiFlash.code.MiControl
{
	// Token: 0x0200005D RID: 93
	[ToolStripItemDesignerAvailability(14)]
	public class ComboBoxStripItem : ToolStripControlHost
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x000128AE File Offset: 0x00010AAE
		public ComboBoxStripItem() : base(new ComboBox())
		{
			this.comboBox = (base.Control as ComboBox);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x000128CC File Offset: 0x00010ACC
		public void SetItem(string[] items)
		{
			this.comboBox.Items.Clear();
			for (int i = 0; i < items.Length; i++)
			{
				this.comboBox.Items.Add(items[i]);
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0001290B File Offset: 0x00010B0B
		public string SelectedText
		{
			get
			{
				if (this.comboBox.SelectedItem != null)
				{
					return this.comboBox.SelectedItem.ToString();
				}
				return "";
			}
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00012930 File Offset: 0x00010B30
		public void SetText(string text)
		{
			bool flag = false;
			foreach (object item in this.comboBox.Items)
			{
				if (item.ToString() == text)
				{
					this.comboBox.SelectedItem = item;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.comboBox.SelectedItem = null;
			}
		}

		// Token: 0x040001D8 RID: 472
		private ComboBox comboBox;
	}
}

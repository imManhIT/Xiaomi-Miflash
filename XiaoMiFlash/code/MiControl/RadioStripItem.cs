using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace XiaoMiFlash.code.MiControl
{
	// Token: 0x02000025 RID: 37
	[ToolStripItemDesignerAvailability(14)]
	public class RadioStripItem : ToolStripControlHost
	{
		// Token: 0x06000100 RID: 256 RVA: 0x0000D840 File Offset: 0x0000BA40
		public RadioStripItem() : base(new RadioButton())
		{
			this.radio = (base.Control as RadioButton);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000D85E File Offset: 0x0000BA5E
		// (set) Token: 0x06000102 RID: 258 RVA: 0x0000D86B File Offset: 0x0000BA6B
		public bool IsChecked
		{
			get
			{
				return this.radio.Checked;
			}
			set
			{
				this.radio.Checked = value;
			}
		}

		// Token: 0x040000B9 RID: 185
		private RadioButton radio;
	}
}

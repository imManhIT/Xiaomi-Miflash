using System;
using System.Windows.Forms;
using XiaoMiFlash.code.lan;

namespace XiaoMiFlash
{
	// Token: 0x02000012 RID: 18
	public partial class MiBaseFrm : Form, ILanguageSupport
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00006947 File Offset: 0x00004B47
		// (set) Token: 0x06000068 RID: 104 RVA: 0x0000694F File Offset: 0x00004B4F
		public string LanID
		{
			get
			{
				return this.lanid;
			}
			set
			{
				this.lanid = value;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00006958 File Offset: 0x00004B58
		public virtual void SetLanguage()
		{
		}

		// Token: 0x04000030 RID: 48
		private string lanid = "";
	}
}

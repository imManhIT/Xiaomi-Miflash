using System;
using System.Windows.Forms;

namespace XiaoMiFlash
{
	// Token: 0x02000020 RID: 32
	internal static class Program
	{
		// Token: 0x060000EB RID: 235 RVA: 0x0000CE16 File Offset: 0x0000B016
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainFrm());
		}
	}
}

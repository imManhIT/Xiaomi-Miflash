using System;
using System.Diagnostics;
using System.Linq;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000040 RID: 64
	public class MiProcess
	{
		// Token: 0x060001C0 RID: 448 RVA: 0x00011AA4 File Offset: 0x0000FCA4
		public static void KillProcess(string processName)
		{
			try
			{
				Process[] processes = Process.GetProcessesByName(processName);
				foreach (Process item in Enumerable.ToList<Process>(processes))
				{
					item.Kill();
					item.Dispose();
				}
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}
	}
}

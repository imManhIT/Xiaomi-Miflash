using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using XiaoMiFlash.code.bl;

namespace XiaoMiFlash
{
	// Token: 0x02000002 RID: 2
	[RunInstaller(true)]
	public class MiInstaller : Installer
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000206F File Offset: 0x0000026F
		private void InitializeComponent()
		{
			this.components = new Container();
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000207C File Offset: 0x0000027C
		public MiInstaller()
		{
			this.InitializeComponent();
			base.BeforeInstall += new InstallEventHandler(this.MiInstaller_BeforeInstall);
			base.AfterInstall += new InstallEventHandler(this.MiInstaller_AfterInstall);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020B0 File Offset: 0x000002B0
		private void MiInstaller_AfterInstall(object sender, InstallEventArgs e)
		{
			MiDriver miDriver = new MiDriver();
			miDriver.CopyFiles(base.Context.Parameters["assemblypath"]);
			miDriver.InstallAllDriver(base.Context.Parameters["assemblypath"], false);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000020FC File Offset: 0x000002FC
		public override void Install(IDictionary savedState)
		{
			try
			{
				base.Install(savedState);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002128 File Offset: 0x00000328
		private void MiInstaller_BeforeInstall(object sender, InstallEventArgs e)
		{
		}

		// Token: 0x04000001 RID: 1
		private IContainer components;
	}
}

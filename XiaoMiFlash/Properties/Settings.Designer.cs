using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XiaoMiFlash.Properties
{
	// Token: 0x02000009 RID: 9
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[CompilerGenerated]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002E95 File Offset: 0x00001095
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002E9C File Offset: 0x0000109C
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002EAE File Offset: 0x000010AE
		[DefaultSettingValue("")]
		[UserScopedSetting]
		[DebuggerNonUserCode]
		public string txtPath
		{
			get
			{
				return (string)this["txtPath"];
			}
			set
			{
				this["txtPath"] = value;
			}
		}

		// Token: 0x0400000F RID: 15
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}

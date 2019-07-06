using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace XiaoMiFlash.Properties
{
	// Token: 0x02000039 RID: 57
	[DebuggerNonUserCode]
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	public class Resources
	{
		// Token: 0x0600017A RID: 378 RVA: 0x00010025 File Offset: 0x0000E225
		internal Resources()
		{
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00010030 File Offset: 0x0000E230
		[EditorBrowsable(2)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager temp = new ResourceManager("XiaoMiFlash.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = temp;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0001006F File Offset: 0x0000E26F
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00010076 File Offset: 0x0000E276
		[EditorBrowsable(2)]
		public static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0001007E File Offset: 0x0000E27E
		public static string txtPath
		{
			get
			{
				return Resources.ResourceManager.GetString("txtPath", Resources.resourceCulture);
			}
		}

		// Token: 0x04000110 RID: 272
		private static ResourceManager resourceMan;

		// Token: 0x04000111 RID: 273
		private static CultureInfo resourceCulture;
	}
}

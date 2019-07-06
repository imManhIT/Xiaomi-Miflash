using System;

namespace XiaoMiFlash.code.lan
{
	// Token: 0x02000011 RID: 17
	public interface ILanguageSupport
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000064 RID: 100
		// (set) Token: 0x06000065 RID: 101
		string LanID { get; set; }

		// Token: 0x06000066 RID: 102
		void SetLanguage();
	}
}

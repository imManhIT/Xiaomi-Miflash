using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000034 RID: 52
	public class FlashResult
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000155 RID: 341 RVA: 0x0000FD28 File Offset: 0x0000DF28
		// (set) Token: 0x06000156 RID: 342 RVA: 0x0000FD30 File Offset: 0x0000DF30
		public bool Result
		{
			get
			{
				return this._result;
			}
			set
			{
				this._result = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000FD39 File Offset: 0x0000DF39
		// (set) Token: 0x06000158 RID: 344 RVA: 0x0000FD41 File Offset: 0x0000DF41
		public string Msg
		{
			get
			{
				return this._msg;
			}
			set
			{
				this._msg = value;
			}
		}

		// Token: 0x04000109 RID: 265
		private bool _result;

		// Token: 0x0400010A RID: 266
		private string _msg;
	}
}

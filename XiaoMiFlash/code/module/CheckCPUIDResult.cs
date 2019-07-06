using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000017 RID: 23
	public class CheckCPUIDResult
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x0000BBF6 File Offset: 0x00009DF6
		// (set) Token: 0x060000CA RID: 202 RVA: 0x0000BBFE File Offset: 0x00009DFE
		public string Device
		{
			get
			{
				return this._device;
			}
			set
			{
				this._device = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000CB RID: 203 RVA: 0x0000BC07 File Offset: 0x00009E07
		// (set) Token: 0x060000CC RID: 204 RVA: 0x0000BC0F File Offset: 0x00009E0F
		public bool Result
		{
			get
			{
				return this._bool;
			}
			set
			{
				this._bool = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000CD RID: 205 RVA: 0x0000BC18 File Offset: 0x00009E18
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000BC20 File Offset: 0x00009E20
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000CF RID: 207 RVA: 0x0000BC29 File Offset: 0x00009E29
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000BC31 File Offset: 0x00009E31
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

		// Token: 0x04000086 RID: 134
		private string _device;

		// Token: 0x04000087 RID: 135
		private bool _bool;

		// Token: 0x04000088 RID: 136
		private string _path;

		// Token: 0x04000089 RID: 137
		private string _msg;
	}
}

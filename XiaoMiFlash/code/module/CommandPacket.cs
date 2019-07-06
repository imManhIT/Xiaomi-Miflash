using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000041 RID: 65
	public class CommandPacket
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00011B28 File Offset: 0x0000FD28
		// (set) Token: 0x060001C3 RID: 451 RVA: 0x00011B30 File Offset: 0x0000FD30
		public int Command
		{
			get
			{
				return this._command;
			}
			set
			{
				this._command = value;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00011B39 File Offset: 0x0000FD39
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00011B41 File Offset: 0x0000FD41
		public int Length
		{
			get
			{
				return this._Lengthgth;
			}
			set
			{
				this._Lengthgth = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00011B4A File Offset: 0x0000FD4A
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x00011B52 File Offset: 0x0000FD52
		public int VersionNumber
		{
			get
			{
				return this._Versionnumber;
			}
			set
			{
				this._Versionnumber = value;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00011B5B File Offset: 0x0000FD5B
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x00011B63 File Offset: 0x0000FD63
		public int VersionCompatible
		{
			get
			{
				return this._Versioncompatible;
			}
			set
			{
				this._Versioncompatible = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00011B6C File Offset: 0x0000FD6C
		// (set) Token: 0x060001CB RID: 459 RVA: 0x00011B74 File Offset: 0x0000FD74
		public int CommandPacketLengthgth
		{
			get
			{
				return this._commandpacketLengthgth;
			}
			set
			{
				this._commandpacketLengthgth = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00011B7D File Offset: 0x0000FD7D
		// (set) Token: 0x060001CD RID: 461 RVA: 0x00011B85 File Offset: 0x0000FD85
		public int Mode
		{
			get
			{
				return this._Mode;
			}
			set
			{
				this._Mode = value;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00011B8E File Offset: 0x0000FD8E
		public CommandPacket()
		{
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00011B98 File Offset: 0x0000FD98
		public CommandPacket(byte[] arr)
		{
			if (arr.Length >= 48)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					if (i % 4 == 0)
					{
						int num = i;
						if (num <= 8)
						{
							if (num != 0)
							{
								if (num != 4)
								{
									if (num == 8)
									{
										this._Versionnumber = (int)arr[i];
									}
								}
								else
								{
									this._Lengthgth = (int)arr[i];
								}
							}
							else
							{
								this._command = (int)arr[i];
							}
						}
						else if (num != 12)
						{
							if (num != 16)
							{
								if (num == 20)
								{
									this._Mode = (int)arr[i];
								}
							}
							else
							{
								this._commandpacketLengthgth = (int)arr[i];
							}
						}
						else
						{
							this._Versioncompatible = (int)arr[i];
						}
					}
				}
			}
		}

		// Token: 0x0400014D RID: 333
		private int _command;

		// Token: 0x0400014E RID: 334
		private int _Lengthgth;

		// Token: 0x0400014F RID: 335
		private int _Versionnumber;

		// Token: 0x04000150 RID: 336
		private int _Versioncompatible;

		// Token: 0x04000151 RID: 337
		private int _commandpacketLengthgth;

		// Token: 0x04000152 RID: 338
		private int _Mode;
	}
}

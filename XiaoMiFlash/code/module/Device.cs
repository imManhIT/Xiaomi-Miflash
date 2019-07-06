using System;
using System.Collections.Generic;
using System.Threading;
using XiaoMiFlash.code.bl;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200003B RID: 59
	public class Device
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00010B49 File Offset: 0x0000ED49
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00010B51 File Offset: 0x0000ED51
		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00010B5A File Offset: 0x0000ED5A
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00010B62 File Offset: 0x0000ED62
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00010B6B File Offset: 0x0000ED6B
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00010B73 File Offset: 0x0000ED73
		public float Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				this._progress = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00010B7C File Offset: 0x0000ED7C
		// (set) Token: 0x0600018B RID: 395 RVA: 0x00010B84 File Offset: 0x0000ED84
		public DateTime StartTime
		{
			get
			{
				return this._startTime;
			}
			set
			{
				this._startTime = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00010B8D File Offset: 0x0000ED8D
		// (set) Token: 0x0600018D RID: 397 RVA: 0x00010B95 File Offset: 0x0000ED95
		public float Elapse
		{
			get
			{
				return this._elapse;
			}
			set
			{
				this._elapse = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00010B9E File Offset: 0x0000ED9E
		// (set) Token: 0x0600018F RID: 399 RVA: 0x00010BA6 File Offset: 0x0000EDA6
		public string Status
		{
			get
			{
				return this._status;
			}
			set
			{
				this._status = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00010BAF File Offset: 0x0000EDAF
		// (set) Token: 0x06000191 RID: 401 RVA: 0x00010BB7 File Offset: 0x0000EDB7
		public List<string> StatusList
		{
			get
			{
				return this._statuslist;
			}
			set
			{
				this._statuslist = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00010BC0 File Offset: 0x0000EDC0
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00010BC8 File Offset: 0x0000EDC8
		public string Result
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00010BD1 File Offset: 0x0000EDD1
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00010BD9 File Offset: 0x0000EDD9
		public bool? IsDone
		{
			get
			{
				return this._isdone;
			}
			set
			{
				this._isdone = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00010BE2 File Offset: 0x0000EDE2
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00010BEA File Offset: 0x0000EDEA
		public bool IsUpdate
		{
			get
			{
				return this._isupdate;
			}
			set
			{
				this._isupdate = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00010BF3 File Offset: 0x0000EDF3
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00010BFB File Offset: 0x0000EDFB
		public DeviceCtrl DeviceCtrl
		{
			get
			{
				return this._devicectrl;
			}
			set
			{
				this._devicectrl = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00010C04 File Offset: 0x0000EE04
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00010C0C File Offset: 0x0000EE0C
		public Thread DThread
		{
			get
			{
				return this._thread;
			}
			set
			{
				this._thread = value;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00010C15 File Offset: 0x0000EE15
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00010C1D File Offset: 0x0000EE1D
		public Cmd DCmd
		{
			get
			{
				return this._cmd;
			}
			set
			{
				this._cmd = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00010C26 File Offset: 0x0000EE26
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00010C2E File Offset: 0x0000EE2E
		public Comm DComm
		{
			get
			{
				return this._comm;
			}
			set
			{
				this._comm = value;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00010C37 File Offset: 0x0000EE37
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00010C3F File Offset: 0x0000EE3F
		public bool CheckCPUID
		{
			get
			{
				return this._checkcpuid;
			}
			set
			{
				this._checkcpuid = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00010C48 File Offset: 0x0000EE48
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00010C50 File Offset: 0x0000EE50
		public string DeviceType
		{
			get
			{
				return this._devicetype;
			}
			set
			{
				this._devicetype = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00010C59 File Offset: 0x0000EE59
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00010C61 File Offset: 0x0000EE61
		public ushort IdProduct
		{
			get
			{
				return this._idproduct;
			}
			set
			{
				this._idproduct = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00010C6A File Offset: 0x0000EE6A
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00010C72 File Offset: 0x0000EE72
		public ushort IdVendor
		{
			get
			{
				return this._idvendor;
			}
			set
			{
				this._idvendor = value;
			}
		}

		// Token: 0x04000126 RID: 294
		private int _index;

		// Token: 0x04000127 RID: 295
		private string _name;

		// Token: 0x04000128 RID: 296
		private float _progress;

		// Token: 0x04000129 RID: 297
		private DateTime _startTime;

		// Token: 0x0400012A RID: 298
		private float _elapse;

		// Token: 0x0400012B RID: 299
		private string _status;

		// Token: 0x0400012C RID: 300
		private List<string> _statuslist = new List<string>();

		// Token: 0x0400012D RID: 301
		private string _result = "";

		// Token: 0x0400012E RID: 302
		private bool? _isdone;

		// Token: 0x0400012F RID: 303
		private bool _isupdate;

		// Token: 0x04000130 RID: 304
		private DeviceCtrl _devicectrl;

		// Token: 0x04000131 RID: 305
		private Thread _thread;

		// Token: 0x04000132 RID: 306
		private Cmd _cmd;

		// Token: 0x04000133 RID: 307
		private Comm _comm;

		// Token: 0x04000134 RID: 308
		private bool _checkcpuid;

		// Token: 0x04000135 RID: 309
		private string _devicetype;

		// Token: 0x04000136 RID: 310
		private ushort _idproduct;

		// Token: 0x04000137 RID: 311
		private ushort _idvendor;
	}
}

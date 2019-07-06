using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using XiaoMiFlash.code.data;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x0200005C RID: 92
	public class Comm
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x00011DE4 File Offset: 0x0000FFE4
		public Comm()
		{
			this.serialPort = new SerialPort();
			this._keepReading = false;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00011E40 File Offset: 0x00010040
		public bool isKeepReading()
		{
			return this._keepReading;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00011E48 File Offset: 0x00010048
		public bool IsOpen
		{
			get
			{
				int count = 20;
				while (count-- > 0 && !this.serialPort.IsOpen)
				{
					Log.w(this.serialPort.PortName, "wait for port open.");
					Thread.Sleep(50);
				}
				return this.serialPort.IsOpen;
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x00011E96 File Offset: 0x00010096
		public void StartReading()
		{
			if (!this._keepReading)
			{
				this._keepReading = true;
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00011EA7 File Offset: 0x000100A7
		public void StopReading()
		{
			if (this._keepReading)
			{
				this._keepReading = false;
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00011EB8 File Offset: 0x000100B8
		public byte[] ReadPortData()
		{
			byte[] readBuffer = null;
			if (this.serialPort.IsOpen)
			{
				int count = this.serialPort.BytesToRead;
				if (count > 0)
				{
					readBuffer = new byte[count];
					try
					{
						this.serialPort.Read(readBuffer, 0, count);
					}
					catch (TimeoutException ex)
					{
						Log.w(this.serialPort.PortName, ex, false);
					}
				}
			}
			return readBuffer;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x00011F24 File Offset: 0x00010124
		public byte[] ReadPortData(int offset, int count)
		{
			byte[] readBuffer = new byte[count];
			try
			{
				this.serialPort.Read(readBuffer, offset, count);
			}
			catch (TimeoutException ex)
			{
				Log.w(this.serialPort.PortName, ex, false);
			}
			return readBuffer;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00011F70 File Offset: 0x00010170
		public void Open()
		{
			this.Close();
			this.serialPort.Open();
			if (this.serialPort.IsOpen)
			{
				return;
			}
			string msg = "open serial port failed!";
			Log.w(this.serialPort.PortName, msg);
			FlashingDevice.UpdateDeviceStatus(this.serialPort.PortName, default(float?), msg, "error", true);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00011FD4 File Offset: 0x000101D4
		private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			int i = this.serialPort.BytesToRead;
			this.recData = new byte[i];
			this.received_count += (long)i;
			this.serialPort.Read(this.recData, 0, i);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0001201C File Offset: 0x0001021C
		public void Close()
		{
			this.serialPort.Close();
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00012029 File Offset: 0x00010229
		public void CleanBuffer()
		{
			this.serialPort.DiscardOutBuffer();
			this.serialPort.DiscardInBuffer();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00012044 File Offset: 0x00010244
		public void WritePort(byte[] send, int offSet, int count)
		{
			if (this.IsOpen)
			{
				bool keepReading = this._keepReading;
				int sendcount = 0;
				Exception ex = new TimeoutException();
				bool writeDone = false;
				while (sendcount++ <= 50 && ex != null && ex.GetType() == typeof(TimeoutException))
				{
					try
					{
						this.serialPort.WriteTimeout = 1000;
						this.serialPort.Write(send, offSet, count);
						writeDone = true;
						if (this.isWriteDump)
						{
							Log.w(this.serialPort.PortName, "write to port:");
							this.Dump(send);
						}
						ex = null;
					}
					catch (TimeoutException timeoutEx)
					{
						ex = timeoutEx;
						Log.w(this.serialPort.PortName, "write time out try agian " + sendcount);
						Thread.Sleep(500);
					}
					catch (Exception exp)
					{
						Log.w(this.serialPort.PortName, "write failed:" + exp.Message);
					}
				}
				if (!writeDone)
				{
					Log.w(this.serialPort.PortName, ex, true);
				}
			}
			this.writeCount++;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00012170 File Offset: 0x00010370
		public bool SendCommand(string command)
		{
			return this.SendCommand(command, false);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0001217C File Offset: 0x0001037C
		public bool SendCommand(string command, bool checkAck)
		{
			byte[] cmd = Encoding.Default.GetBytes(command);
			if (this.isReadDump || checkAck)
			{
				Log.w(this.serialPort.PortName, "send command:" + command);
			}
			this.WritePort(cmd, 0, cmd.Length);
			return checkAck && this.GetResponse(checkAck);
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000121D4 File Offset: 0x000103D4
		private int SubstringCount(string str, string substring)
		{
			if (str.Contains(substring))
			{
				string strRepace = str.Replace(substring, "");
				return (str.Length - strRepace.Length) / substring.Length;
			}
			return 0;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00012210 File Offset: 0x00010410
		public bool chkRspAck(out string msg)
		{
			msg = null;
			byte[] data = this.ReadDataFromPort();
			string[] rsps = this.Dump(data, true);
			string chkStr = "<response value=\"ACK\"";
			int count = 10;
			while ((rsps.Length != 2 || rsps[1].IndexOf(chkStr) < 0) && count-- >= 0)
			{
				Thread.Sleep(10);
				data = this.ReadDataFromPort();
				rsps = this.Dump(data, true);
			}
			if (rsps.Length == 2 && rsps[1].IndexOf(chkStr) >= 0)
			{
				this.CleanBuffer();
				return true;
			}
			msg = "did not detect ACK from target.";
			return false;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00012290 File Offset: 0x00010490
		public bool chkRspAck(out string msg, int chunkCount)
		{
			msg = null;
			byte[] data = this.ReadDataFromPort();
			string[] rsps = this.Dump(data, true);
			string chkStr = "<response value=\"ACK\"";
			int count = 10;
			while ((rsps.Length != 2 || rsps[1].IndexOf(chkStr) < 0) && count-- >= 0)
			{
				Thread.Sleep(10);
				data = this.ReadDataFromPort();
				rsps = this.Dump(data, true);
			}
			if (rsps.Length != 2 || rsps[1].IndexOf(chkStr) < 0)
			{
				msg = rsps[1];
				return false;
			}
			int ackCount = this.SubstringCount(rsps[1], chkStr);
			count = 10;
			while (ackCount < chunkCount * 2 && count-- > 0)
			{
				Thread.Sleep(10);
				data = this.ReadDataFromPort();
				rsps = this.Dump(data, true);
				ackCount += this.SubstringCount(rsps[1], chkStr);
			}
			if (chunkCount * 2 > ackCount)
			{
				Log.w(this.serialPort.PortName, "ACK count don't match!");
				throw new Exception("ACK count don't match!");
			}
			Log.w(this.serialPort.PortName, string.Format("{0} chunks match {1} ack", chunkCount, ackCount));
			this.CleanBuffer();
			this.writeCount = 0;
			return true;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x000123B0 File Offset: 0x000105B0
		public byte[] getRecDataIgnoreExcep()
		{
			byte[] data = this.ReadDataFromPort();
			if (data != null && data.Length > 0 && this.isReadDump)
			{
				Log.w(this.serialPort.PortName, "read from port:");
				this.Dump(data);
			}
			return data;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000123F4 File Offset: 0x000105F4
		public byte[] getRecData()
		{
			byte[] data = this.ReadDataFromPort();
			if (data == null)
			{
				throw new Exception("can not read from port " + this.serialPort.PortName);
			}
			if (data.Length > 0 && this.isReadDump)
			{
				Log.w(this.serialPort.PortName, "read from port:");
				this.Dump(data);
			}
			return data;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00012454 File Offset: 0x00010654
		private byte[] ReadDataFromPort()
		{
			int count = 10;
			this.recData = null;
			this.recData = this.ReadPortData();
			while (count-- >= 0 && this.recData == null)
			{
				Thread.Sleep(50);
				this.recData = this.ReadPortData();
			}
			return this.recData;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x000124A4 File Offset: 0x000106A4
		private bool WaitForAck()
		{
			bool hasACK = false;
			int count = 10;
			while (count-- > 0 && !hasACK)
			{
				byte[] data = this.ReadDataFromPort();
				string[] results = this.Dump(data);
				hasACK = (results.Length == 2 && results[1].IndexOf("<response value=\"ACK\" />") >= 0);
				Thread.Sleep(50);
			}
			return hasACK;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000124F8 File Offset: 0x000106F8
		public bool GetResponse(bool waiteACK)
		{
			bool hasACK = false;
			Log.w(this.serialPort.PortName, "get response from target");
			if (!waiteACK)
			{
				return this.ReadDataFromPort() != null;
			}
			int count = 2;
			if (waiteACK)
			{
				count = 10;
			}
			while (count-- > 0 && !hasACK)
			{
				List<XmlDocument> docList = this.GetResponseXml(waiteACK);
				int count2 = docList.Count;
				foreach (XmlDocument item in docList)
				{
					XmlNode dataNode = item.SelectSingleNode("data");
					XmlNodeList list = dataNode.ChildNodes;
					foreach (object obj in list)
					{
						XmlNode node = (XmlNode)obj;
						XmlElement element = (XmlElement)node;
						if (element.Name.ToLower() == "sig")
						{
							this.auth = element.OuterXml.Replace("blob", "sig");
						}
						foreach (object obj2 in element.Attributes)
						{
							XmlAttribute attr = (XmlAttribute)obj2;
							if (attr.Name.ToLower() == "maxpayloadsizetotargetinbytes")
							{
								this.m_dwBufferSectors = Convert.ToInt32(attr.Value) / this.intSectorSize;
							}
							if (attr.Value.ToLower() == "ack")
							{
								hasACK = true;
							}
						}
					}
				}
				if (waiteACK)
				{
					Thread.Sleep(50);
				}
			}
			return hasACK;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00012704 File Offset: 0x00010904
		private List<XmlDocument> GetResponseXml()
		{
			return this.GetResponseXml(false);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00012710 File Offset: 0x00010910
		public List<XmlDocument> GetResponseXml(bool waiteACK)
		{
			List<XmlDocument> docList = new List<XmlDocument>();
			byte[] data = this.ReadDataFromPort();
			string[] rsps = this.Dump(data, waiteACK);
			if (rsps.Length >= 2)
			{
				string[] arr = Regex.Split(rsps[1], "\\<\\?xml");
				foreach (string item in Enumerable.ToList<string>(arr))
				{
					if (!string.IsNullOrEmpty(item))
					{
						XmlDocument doc = new XmlDocument();
						doc.LoadXml("<?xml " + item);
						docList.Add(doc);
					}
				}
			}
			return docList;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000127B4 File Offset: 0x000109B4
		private string GetResponseXmlStr()
		{
			byte[] data = this.ReadDataFromPort();
			return this.Dump(data)[1];
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000127D1 File Offset: 0x000109D1
		private string[] Dump(byte[] binary)
		{
			return this.Dump(binary, false);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000127DC File Offset: 0x000109DC
		private string[] Dump(byte[] binary, bool waitACK)
		{
			if (binary == null)
			{
				Log.w(this.serialPort.PortName, "no Binary dump");
				return new string[]
				{
					"",
					""
				};
			}
			StringBuilder hex = new StringBuilder();
			StringBuilder ascii = new StringBuilder();
			new StringBuilder();
			new StringBuilder();
			for (int i = 0; i < binary.Length; i++)
			{
				ascii.Append(Convert.ToChar(binary[i]).ToString());
			}
			if ((this.isReadDump || waitACK) && !this._keepReading)
			{
				Log.w(this.serialPort.PortName, "dump:" + ascii.ToString() + "\r\n\r\n");
			}
			return new string[]
			{
				hex.ToString(),
				ascii.ToString()
			};
		}

		// Token: 0x040001CA RID: 458
		public int writeCount;

		// Token: 0x040001CB RID: 459
		public bool isReadDump = true;

		// Token: 0x040001CC RID: 460
		public bool isWriteDump;

		// Token: 0x040001CD RID: 461
		public bool ignoreResponse = true;

		// Token: 0x040001CE RID: 462
		public SerialPort serialPort;

		// Token: 0x040001CF RID: 463
		private bool _keepReading;

		// Token: 0x040001D0 RID: 464
		public byte[] recData;

		// Token: 0x040001D1 RID: 465
		private long received_count;

		// Token: 0x040001D2 RID: 466
		public int MAX_SECTOR_STR_LEN = 20;

		// Token: 0x040001D3 RID: 467
		public int SECTOR_SIZE_UFS = 4096;

		// Token: 0x040001D4 RID: 468
		public int SECTOR_SIZE_EMMC = 512;

		// Token: 0x040001D5 RID: 469
		public int m_dwBufferSectors;

		// Token: 0x040001D6 RID: 470
		public int intSectorSize;

		// Token: 0x040001D7 RID: 471
		public string auth = "";
	}
}

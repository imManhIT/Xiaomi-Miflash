using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Runtime.InteropServices;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x02000019 RID: 25
	public class FileTransfer
	{
		// Token: 0x060000D7 RID: 215 RVA: 0x0000C0E0 File Offset: 0x0000A2E0
		public FileTransfer(string port, string filePath)
		{
			this.portName = port;
			this.filePath = filePath;
			FlashingDevice.UpdateDeviceStatus(this.portName, new float?(0f), "flashing " + filePath, "flashing", false);
			this.openFile(filePath);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000C13C File Offset: 0x0000A33C
		private bool openFile(string filePath)
		{
			this.filePath = filePath;
			bool result = false;
			if (this.isShareMemory)
			{
				if (!MemImg.shareMemTable.ContainsKey(filePath))
				{
					ShareMemory sm = new ShareMemory(filePath);
					MemImg.shareMemTable[filePath] = sm;
					this.fileLength = sm.m_FileSize;
				}
				return true;
			}
			try
			{
				FileInfo file = new FileInfo(filePath);
				this.fileLength = file.Length;
				this.fileStream = File.OpenRead(filePath);
				result = true;
			}
			catch (Exception err)
			{
				result = false;
				throw new Exception(err.Message);
			}
			return result;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000C1CC File Offset: 0x0000A3CC
		public int transfer(SerialPort port, int offset, int size)
		{
			if (!port.IsOpen)
			{
				string.Format("{0} is not open", port.PortName);
				return 0;
			}
			int i = 0;
			byte[] sendData = this.GetBytesFromFile((long)offset, size, out i);
			port.Write(sendData, 0, size);
			return i;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000C20C File Offset: 0x0000A40C
		public void WriteFile(SerialPortDevice portCnn, string strPartitionStartSector, string strPartitionSectorNumber, string pszImageFile, string strFileStartSector, string strFileSectorOffset, string sector_size, string physical_partition_number, string addtionalFirehose, bool chkAck, int? chunkCount)
		{
			long ullPartitionSectorNumber = Convert.ToInt64(strPartitionSectorNumber);
			if (ullPartitionSectorNumber == 0L)
			{
				ullPartitionSectorNumber = 2147483647L;
			}
			long ullFileStartSector = Convert.ToInt64(strFileStartSector);
			long ullFileSectorOffset = Convert.ToInt64(strFileSectorOffset);
			long ullSectorSizeInBytes = Convert.ToInt64(sector_size);
			long ullPhysicalParitionNumber = Convert.ToInt64(physical_partition_number);
			Log.w(portCnn.comm.serialPort.PortName, string.Format("write file legnth {0} to partition {1}", this.getFileSize(), strPartitionStartSector));
			long ullFileEndSector = (this.getFileSize() + ullSectorSizeInBytes - 1L) / ullSectorSizeInBytes;
			if (ullFileEndSector - ullFileStartSector > ullPartitionSectorNumber)
			{
				ullFileEndSector = ullFileStartSector + ullPartitionSectorNumber;
			}
			else
			{
				ullPartitionSectorNumber = ullFileEndSector - ullFileStartSector;
			}
			string cmd = string.Format(Firehose.FIREHOSE_PROGRAM, new object[]
			{
				ullSectorSizeInBytes,
				ullPartitionSectorNumber,
				strPartitionStartSector,
				ullPhysicalParitionNumber,
				addtionalFirehose
			});
			portCnn.comm.SendCommand(cmd);
			for (long ullSector = ullFileStartSector; ullSector < ullFileEndSector; ullSector += (long)portCnn.comm.m_dwBufferSectors)
			{
				long ulWrittenSectors = ullFileEndSector - ullSector;
				ulWrittenSectors = ((ulWrittenSectors < (long)portCnn.comm.m_dwBufferSectors) ? ulWrittenSectors : ((long)portCnn.comm.m_dwBufferSectors));
				Log.w(portCnn.comm.serialPort.PortName, string.Format("WriteFile position {0}, size {1}", (ullSectorSizeInBytes * ullSector).ToString("X"), ullSectorSizeInBytes * ulWrittenSectors));
				long offset = ullFileSectorOffset + ullSectorSizeInBytes * ullSector;
				int size = (int)(ullSectorSizeInBytes * ulWrittenSectors);
				int i = 0;
				byte[] sendData = this.GetBytesFromFile(offset, size, out i);
				portCnn.comm.WritePort(sendData, 0, sendData.Length);
			}
			if (chkAck)
			{
				string rspMsg = null;
				if (!portCnn.comm.chkRspAck(out rspMsg, chunkCount.Value))
				{
					throw new Exception(rspMsg);
				}
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000C3C8 File Offset: 0x0000A5C8
		public void WriteSparseFileToDevice(SerialPortDevice portCnn, string pszPartitionStartSector, string pszPartitionSectorNumber, string pszImageFile, string pszFileStartSector, string pszSectorSizeInBytes, string pszPhysicalPartitionNumber, string addtionalFirehose)
		{
			int ullPartitionStartSector = Convert.ToInt32(pszPartitionStartSector);
			int ullPartitionSectorNumber = Convert.ToInt32(pszPartitionSectorNumber);
			int ullFileStartSector = Convert.ToInt32(pszFileStartSector);
			long ullFileOffset = 0L;
			int ullSectorSizeInBytes = Convert.ToInt32(pszSectorSizeInBytes);
			Convert.ToInt32(pszPhysicalPartitionNumber);
			SparseImageHeader imageHeader = default(SparseImageHeader);
			string msg = "";
			if (ullFileStartSector != 0)
			{
				msg = "ERROR_BAD_FORMAT";
				Log.w(portCnn.comm.serialPort.PortName, msg);
			}
			if (ullSectorSizeInBytes == 0)
			{
				msg = "ERROR_BAD_FORMAT";
				Log.w(portCnn.comm.serialPort.PortName, "ERROR_BAD_FORMAT");
			}
			int size = Marshal.SizeOf(imageHeader);
			int i = 0;
			byte[] data = this.GetBytesFromFile(ullFileOffset, size, out i);
			imageHeader = (SparseImageHeader)CommandFormat.BytesToStuct(data, typeof(SparseImageHeader));
			ullFileOffset += (long)((ulong)imageHeader.uFileHeaderSize);
			if (imageHeader.uMagic != 3978755898u)
			{
				msg = "ERROR_BAD_FORMAT";
				Log.w(portCnn.comm.serialPort.PortName, string.Format("ERROR_BAD_FORMAT {0}", imageHeader.uMagic.ToString()));
			}
			if (imageHeader.uMajorVersion != 1)
			{
				msg = "ERROR_UNSUPPORTED_TYPE";
				Log.w(portCnn.comm.serialPort.PortName, string.Format("ERROR_UNSUPPORTED_TYPE {0}", imageHeader.uMajorVersion.ToString()));
			}
			if ((ulong)imageHeader.uBlockSize % (ulong)((long)ullSectorSizeInBytes) != 0UL)
			{
				msg = "ERROR_BAD_FORMAT";
				Log.w(portCnn.comm.serialPort.PortName, string.Format("ERROR_BAD_FORMAT {0}", imageHeader.uBlockSize.ToString()));
			}
			if (ullPartitionSectorNumber != 0 && (ulong)(imageHeader.uBlockSize * imageHeader.uTotalBlocks) / (ulong)((long)ullSectorSizeInBytes) > (ulong)((long)ullPartitionSectorNumber))
			{
				msg = "ERROR_FILE_TOO_LARGE";
				Log.w(portCnn.comm.serialPort.PortName, string.Format("ERROR_FILE_TOO_LARGE size {0} ullPartitionSectorNumber {1}", ((long)((ulong)(imageHeader.uBlockSize * imageHeader.uTotalBlocks) / (ulong)((long)ullSectorSizeInBytes))).ToString(), ullPartitionSectorNumber.ToString()));
			}
			if (!string.IsNullOrEmpty(msg))
			{
				throw new Exception(msg);
			}
			int sendCount = 0;
			int j = 1;
			while ((long)j <= (long)((ulong)imageHeader.uTotalChunks))
			{
				Log.w(portCnn.comm.serialPort.PortName, string.Format("total chunks {0}, current chunk {1}", imageHeader.uTotalChunks, j));
				SparseChunkHeader chunkHeader = default(SparseChunkHeader);
				size = Marshal.SizeOf(chunkHeader);
				float percent = 0f;
				data = this.GetBytesFromFile(ullFileOffset, size, out i, out percent);
				chunkHeader = (SparseChunkHeader)CommandFormat.BytesToStuct(data, typeof(SparseChunkHeader));
				ullFileOffset += (long)((ulong)imageHeader.uChunkHeaderSize);
				int uChunkBytes = (int)(imageHeader.uBlockSize * chunkHeader.uChunkSize);
				int uChunkSectors = uChunkBytes / ullSectorSizeInBytes;
				switch (chunkHeader.uChunkType)
				{
				case 51905:
				{
					if ((ulong)chunkHeader.uTotalSize != (ulong)((long)((int)imageHeader.uChunkHeaderSize + uChunkBytes)))
					{
						Log.w(portCnn.comm.serialPort.PortName, "ERROR_BAD_FORMAT");
						throw new Exception("ERROR_BAD_FORMAT");
					}
					string strPartitionStartSector = ullPartitionStartSector.ToString();
					string strPartitionSectorNumber = uChunkSectors.ToString();
					string strFileStartSector = (ullFileOffset / (long)ullSectorSizeInBytes).ToString();
					string strFileSectorOffset = (ullFileOffset % (long)ullSectorSizeInBytes).ToString();
					sendCount++;
					bool chkAck = false;
					int sendChunks = 0;
					if (imageHeader.uTotalChunks <= 10u)
					{
						if ((long)j == (long)((ulong)imageHeader.uTotalChunks))
						{
							sendChunks = sendCount;
							chkAck = true;
						}
					}
					else
					{
						if (sendCount % 10 == 0)
						{
							sendChunks = 10;
							chkAck = true;
						}
						if ((long)j == (long)((ulong)imageHeader.uTotalChunks))
						{
							sendChunks = sendCount % 10;
							chkAck = true;
						}
					}
					this.WriteFile(portCnn, strPartitionStartSector, strPartitionSectorNumber, pszImageFile, strFileStartSector, strFileSectorOffset, pszSectorSizeInBytes, pszPhysicalPartitionNumber, addtionalFirehose, chkAck, new int?(sendChunks));
					ullFileOffset += (long)(ullSectorSizeInBytes * uChunkSectors);
					ullPartitionStartSector += uChunkSectors;
					break;
				}
				case 51906:
					goto IL_414;
				case 51907:
					if (chunkHeader.uTotalSize != (uint)imageHeader.uChunkHeaderSize)
					{
						Log.w(portCnn.comm.serialPort.PortName, "ERROR_BAD_FORMAT");
					}
					ullPartitionStartSector += uChunkSectors;
					if ((long)j == (long)((ulong)imageHeader.uTotalChunks))
					{
						int sendChunks2 = sendCount % 10;
						if (sendChunks2 > 0)
						{
							string rspMsg = null;
							if (!portCnn.comm.chkRspAck(out rspMsg, sendChunks2))
							{
								throw new Exception(rspMsg);
							}
						}
					}
					break;
				default:
					goto IL_414;
				}
				IL_43F:
				j++;
				continue;
				IL_414:
				Log.w(portCnn.comm.serialPort.PortName, string.Format("ERROR_UNSUPPORTED_TYPE {0}", chunkHeader.uChunkType.ToString()));
				goto IL_43F;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000C82C File Offset: 0x0000AA2C
		public byte[] GetBytesFromFile(long offset, int size, out int n)
		{
			if (this.isShareMemory)
			{
				ShareMemory sm = (ShareMemory)MemImg.shareMemTable[this.filePath];
				byte[] byteData = new byte[size];
				sm.Read(ref byteData, (int)offset, size);
				n = byteData.Length;
				return byteData;
			}
			long totalSize = this.fileStream.Length;
			byte[] sendData = new byte[size];
			this.fileStream.Seek(offset, 0);
			n = this.fileStream.Read(sendData, 0, size);
			float percent = (float)offset / (float)totalSize;
			FlashingDevice.UpdateDeviceStatus(this.portName, new float?(percent), null, "flashing", false);
			return sendData;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000C8C4 File Offset: 0x0000AAC4
		public byte[] GetBytesFromFile(long offset, int size, out int n, out float percent)
		{
			long totalSize = this.fileStream.Length;
			byte[] sendData = new byte[size];
			this.fileStream.Seek(offset, 0);
			n = this.fileStream.Read(sendData, 0, size);
			percent = (float)offset / (float)totalSize;
			FlashingDevice.UpdateDeviceStatus(this.portName, new float?(percent), null, "flashing", false);
			return sendData;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000C928 File Offset: 0x0000AB28
		public long getFileSize()
		{
			if (this.fileLength != 0L)
			{
				return this.fileLength;
			}
			FileInfo file = new FileInfo(this.filePath);
			return file.Length;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000C958 File Offset: 0x0000AB58
		public void closeTransfer()
		{
			if (this.isShareMemory)
			{
				ShareMemory sm = (ShareMemory)MemImg.shareMemTable[this.filePath];
				sm.Close();
			}
			if (this.fileStream != null)
			{
				this.fileStream.Close();
				this.fileStream.Dispose();
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000C9A8 File Offset: 0x0000ABA8
		~FileTransfer()
		{
			if (this.fileStream != null)
			{
				this.fileStream.Close();
				this.fileStream.Dispose();
			}
		}

		// Token: 0x0400008A RID: 138
		protected FileStream fileStream;

		// Token: 0x0400008B RID: 139
		public string filePath;

		// Token: 0x0400008C RID: 140
		public string portName;

		// Token: 0x0400008D RID: 141
		private long fileLength;

		// Token: 0x0400008E RID: 142
		public bool isShareMemory;

		// Token: 0x0400008F RID: 143
		private List<ShareMemory> shareMemList = new List<ShareMemory>();
	}
}

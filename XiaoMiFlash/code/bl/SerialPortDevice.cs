using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using XiaoMiFlash.code.authFlash;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x0200000D RID: 13
	public class SerialPortDevice : DeviceCtrl
	{
		// Token: 0x06000042 RID: 66 RVA: 0x00003424 File Offset: 0x00001624
		public override void flash()
		{
			if (string.IsNullOrEmpty(this.deviceName))
			{
				return;
			}
			try
			{
				if (!Directory.Exists(this.swPath))
				{
					throw new Exception("sw path is not valid");
				}
				this.comm.isReadDump = this.openReadDump;
				this.comm.isWriteDump = this.openWriteDump;
				DirectoryInfo info = new DirectoryInfo(this.swPath);
				DirectoryInfo[] dics = info.GetDirectories();
				foreach (DirectoryInfo item in dics)
				{
					if (item.Name.ToLower().IndexOf("images") >= 0)
					{
						this.swPath = item.FullName;
						break;
					}
				}
				if (this.NeedProvision(this.swPath))
				{
					this.m_iSkipStorageInit = 1;
				}
				FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(0f), "start flash", "flashing", false);
				this.registerPort(this.deviceName);
				this.SaharaDownloadProgrammer();
				Thread.Sleep(1000);
				this.PropareFirehose();
				this.ConfigureDDR(this.comm.intSectorSize, this.BUFFER_SECTORS, this.storageType, this.m_iSkipStorageInit);
				if (this.Provision(this.swPath))
				{
					this.SaharaDownloadProgrammer();
					this.PropareFirehose();
					this.m_iSkipStorageInit = 0;
					this.ConfigureDDR(this.comm.intSectorSize, this.BUFFER_SECTORS, this.storageType, this.m_iSkipStorageInit);
				}
				if (string.IsNullOrEmpty(MiAppConfig.Get("mainProgram")) || MiAppConfig.Get("mainProgram").ToString() == "xiaomi")
				{
					if (this.storageType == Storage.ufs)
					{
						this.SetBootPartition();
					}
					this.comm.StartReading();
					this.FirehoseDownloadImg(this.swPath);
					this.comm.StopReading();
				}
				else
				{
					this.comm.serialPort.DiscardInBuffer();
					this.comm.serialPort.DiscardOutBuffer();
					this.comm.serialPort.Close();
					this.comm.serialPort.Dispose();
					Cmd cmd = new Cmd(this.deviceName, "");
					string[] programs = FileSearcher.SearchFiles(this.swPath, SoftwareImage.RawProgramPattern);
					string[] patchs = FileSearcher.SearchFiles(this.swPath, SoftwareImage.PatchPattern);
					for (int i = 0; i < programs.Length; i++)
					{
						programs[i] = Path.GetFileName(programs[i]);
					}
					for (int j = 0; j < patchs.Length; j++)
					{
						patchs[j] = Path.GetFileName(patchs[j]);
					}
					string verboseCmd = "";
					if (this.verbose)
					{
						verboseCmd = " --verbose";
					}
					string rawprogramCmd = string.Format("fh_loader.exe --port=\\\\.\\{0} --sendxml={1} --search_path={2} --noprompt --showpercentagecomplete --maxpayloadsizeinbytes={3} --zlpawarehost=1 --memoryname={4} {5} {6}", new object[]
					{
						this.deviceName,
						string.Join(",", programs),
						this.swPath,
						this.comm.intSectorSize * this.BUFFER_SECTORS,
						this.storageType,
						(this.m_iSkipStorageInit == 1) ? " --skipstorageinit " : "",
						verboseCmd
					});
					string patchCmd = string.Format(" & fh_loader.exe --port=\\\\.\\{0} --sendxml={1} --search_path={2} --noprompt --showpercentagecomplete --maxpayloadsizeinbytes={3} --zlpawarehost=1 --memoryname={4} {5} {6}", new object[]
					{
						this.deviceName,
						string.Join(",", patchs),
						this.swPath,
						this.comm.intSectorSize * this.BUFFER_SECTORS,
						this.storageType,
						(this.m_iSkipStorageInit == 1) ? " --skipstorageinit " : "",
						verboseCmd
					});
					string setactivepartitionCmd = string.Format(" & fh_loader.exe --port=\\\\.\\{0} --setactivepartition={1} --noprompt --showpercentagecomplete --maxpayloadsizeinbytes={2} --zlpawarehost=1 --memoryname={3} {4} {5}", new object[]
					{
						this.deviceName,
						(this.storageType.ToLower() == "ufs") ? 1 : 0,
						this.comm.intSectorSize * this.BUFFER_SECTORS,
						(this.m_iSkipStorageInit == 1) ? " --skipstorageinit " : "",
						this.storageType,
						verboseCmd
					});
					string resetCmd = string.Format(" & fh_loader.exe --port=\\\\.\\{0} --reset --noprompt --showpercentagecomplete --maxpayloadsizeinbytes={1} --zlpawarehost=1 --memoryname={2} {3} {4}", new object[]
					{
						this.deviceName,
						this.comm.intSectorSize * this.BUFFER_SECTORS,
						this.storageType,
						(this.m_iSkipStorageInit == 1) ? " --skipstorageinit " : "",
						verboseCmd
					});
					string cmdStr = rawprogramCmd + patchCmd + setactivepartitionCmd + resetCmd;
					cmd.Execute_returnLine(this.deviceName, cmdStr, 1);
				}
				FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(1f), "flash done", "success", true);
			}
			catch (Exception ex)
			{
				FlashingDevice.UpdateDeviceStatus(this.deviceName, default(float?), ex.Message, "error", true);
				Log.w(this.deviceName, ex, false);
			}
			finally
			{
				this.comm.serialPort.Close();
				this.comm.serialPort.Dispose();
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003984 File Offset: 0x00001B84
		public override void CheckSha256()
		{
			if (string.IsNullOrEmpty(this.deviceName))
			{
				return;
			}
			try
			{
				if (!Directory.Exists(this.swPath))
				{
					throw new Exception("sw path is not valid");
				}
				DirectoryInfo info = new DirectoryInfo(this.swPath);
				DirectoryInfo[] dics = info.GetDirectories();
				foreach (DirectoryInfo item in dics)
				{
					if (item.Name.ToLower().IndexOf("images") >= 0)
					{
						this.swPath = item.FullName;
						break;
					}
				}
				FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(0f), "start flash", "flashing", false);
				this.registerPort(this.deviceName);
				this.SaharaDownloadProgrammer();
				this.PropareFirehose();
				this.ConfigureDDR(this.comm.intSectorSize, this.BUFFER_SECTORS, this.storageType, 0);
				this.GetSha256(this.swPath);
				FlashingDevice.UpdateDeviceStatus(this.deviceName, new float?(1f), "check done", "success", true);
			}
			catch (Exception ex)
			{
				FlashingDevice.UpdateDeviceStatus(this.deviceName, default(float?), ex.Message, "error", true);
				Log.w(this.deviceName, ex, true);
			}
			finally
			{
				this.comm.serialPort.Close();
				this.comm.serialPort.Dispose();
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003B18 File Offset: 0x00001D18
		private void registerPort(string port)
		{
			if (this.comm.serialPort != null)
			{
				this.comm.serialPort.Close();
			}
			this.comm.serialPort.PortName = port;
			this.comm.serialPort.BaudRate = 9600;
			this.comm.serialPort.Parity = 0;
			this.comm.serialPort.ReadTimeout = 100;
			this.comm.serialPort.WriteTimeout = 100;
			this.comm.Open();
			foreach (Device item in FlashingDevice.flashDeviceList)
			{
				if (item.Name == this.deviceName)
				{
					item.DComm = this.comm;
				}
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003C08 File Offset: 0x00001E08
		private void SaharaDownloadProgrammer()
		{
			if (!this.comm.IsOpen)
			{
				Log.w(this.comm.serialPort.PortName, string.Format("port {0} is not open.", this.comm.serialPort.PortName));
				return;
			}
			string msg = string.Format("[{0}]:{1}", this.comm.serialPort.PortName, "start flash.");
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "read hello packet", "flashing", false);
			Log.w(this.comm.serialPort.PortName, msg);
			this.comm.getRecDataIgnoreExcep();
			if (this.comm.recData == null || this.comm.recData.Length == 0)
			{
				this.comm.recData = new byte[48];
			}
			sahara_packet packet = default(sahara_packet);
			sahara_hello_packet helloPacket = (sahara_hello_packet)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_hello_packet));
			helloPacket.Reserved = new uint[6];
			default(sahara_hello_response).Reserved = new uint[6];
			sahara_switch_Mode_packet swicthModePacket = default(sahara_switch_Mode_packet);
			sahara_readdata_packet readDataPacket = default(sahara_readdata_packet);
			sahara_64b_readdata_packet readData64bPacket = default(sahara_64b_readdata_packet);
			sahara_end_transfer_packet end_packet = default(sahara_end_transfer_packet);
			sahara_done_response doneRsp = default(sahara_done_response);
			int count = 10;
			byte[] sendData;
			while (count-- > 0 && helloPacket.Command != 1u)
			{
				msg = "cannot receive hello packet,MiFlash is trying to reset status!";
				Log.w(this.comm.serialPort.PortName, msg);
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), msg, "flashing", false);
				this.comm.getRecDataIgnoreExcep();
				if (this.comm.recData == null || this.comm.recData.Length == 0)
				{
					this.comm.recData = new byte[48];
				}
				helloPacket = (sahara_hello_packet)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_hello_packet));
				Thread.Sleep(500);
				if (count <= 2 && helloPacket.Command != 1u)
				{
					msg = "try to reset status.";
					Log.w(this.comm.serialPort.PortName, msg);
					FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), msg, "flashing", false);
					this.comm.serialPort.DiscardInBuffer();
					this.comm.serialPort.DiscardOutBuffer();
					sendData = CommandFormat.StructToBytes(new sahara_hello_response
					{
						Reserved = new uint[6],
						Command = 2u,
						Length = 48u,
						Version = 2u,
						Version_min = 1u,
						Mode = 3u
					});
					this.comm.WritePort(sendData, 0, sendData.Length);
					this.comm.getRecDataIgnoreExcep();
					msg = "Switch mode back";
					Log.w(this.comm.serialPort.PortName, msg);
					FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), msg, "flashing", false);
					swicthModePacket.Command = 12u;
					swicthModePacket.Length = 12u;
					swicthModePacket.Mode = 0u;
					sendData = CommandFormat.StructToBytes(swicthModePacket);
					byte[] switchData = new byte[12];
					Array.ConstrainedCopy(sendData, 0, switchData, 0, 12);
					this.comm.WritePort(switchData, 0, switchData.Length);
				}
			}
			if (helloPacket.Command != 1u)
			{
				msg = "cannot receive hello packet";
				throw new Exception(msg);
			}
			msg = "received hello packet";
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), msg, "flashing", false);
			Log.w(this.comm.serialPort.PortName, msg);
			sendData = CommandFormat.StructToBytes(new sahara_hello_response
			{
				Reserved = new uint[6],
				Command = 2u,
				Length = 48u,
				Version = 2u,
				Version_min = 1u
			});
			this.comm.WritePort(sendData, 0, sendData.Length);
			string[] files = FileSearcher.SearchFiles(this.swPath, SoftwareImage.ProgrammerPattern);
			if (files.Length <= 0)
			{
				throw new Exception("can not found programmer file.");
			}
			string programmer = files[0];
			FileInfo fileInfo = new FileInfo(programmer);
			if (fileInfo.Name.ToLower().IndexOf("firehose") >= 0)
			{
				this.programmerType = Programmer.firehose;
			}
			if (fileInfo.Name.ToLower().IndexOf("ufs") >= 0)
			{
				this.storageType = Storage.ufs;
			}
			else if (fileInfo.Name.ToLower().IndexOf("emmc") >= 0)
			{
				this.storageType = Storage.emmc;
			}
			if (fileInfo.Name.ToLower().IndexOf("lite") >= 0)
			{
				this.isLite = true;
			}
			this.comm.intSectorSize = ((this.storageType == Storage.ufs) ? this.comm.SECTOR_SIZE_UFS : this.comm.SECTOR_SIZE_EMMC);
			Log.w(this.comm.serialPort.PortName, "download programmer " + programmer);
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "download programmer " + programmer, "flashing", false);
			FileTransfer transfer = new FileTransfer(this.comm.serialPort.PortName, programmer);
			bool done;
			do
			{
				done = false;
				this.comm.getRecData();
				byte[] recData = this.comm.recData;
				packet = (sahara_packet)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_packet));
				uint uCommand = packet.Command;
				uint num = uCommand;
				switch (num)
				{
				case 3u:
					readDataPacket = (sahara_readdata_packet)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_readdata_packet));
					msg = string.Format("sahara read data:imgID {0}, offset {1},length {2}", readDataPacket.Image_id, readDataPacket.Offset, readDataPacket.SLength);
					transfer.transfer(this.comm.serialPort, (int)readDataPacket.Offset, (int)readDataPacket.SLength);
					Log.w(this.comm.serialPort.PortName, msg);
					break;
				case 4u:
					end_packet = (sahara_end_transfer_packet)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_end_transfer_packet));
					msg = string.Format("sahara read end  imgID:{0} status:{1}", end_packet.Image_id, end_packet.Status);
					if (end_packet.Status != 0u)
					{
						Log.w(this.comm.serialPort.PortName, string.Format("sahara read end error with status:{0}", end_packet.Status));
					}
					done = true;
					Log.w(this.comm.serialPort.PortName, msg);
					break;
				default:
					if (num != 18u)
					{
						msg = string.Format("invalid command:{0}", packet.Command);
						Log.w(this.comm.serialPort.PortName, msg);
					}
					else
					{
						readData64bPacket = (sahara_64b_readdata_packet)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_64b_readdata_packet));
						msg = string.Format("sahara read 64b data:imgID {0},offset {1},length {2}", readData64bPacket.Image_id, readData64bPacket.Offset, readData64bPacket.SLength);
						transfer.transfer(this.comm.serialPort, (int)readData64bPacket.Offset, (int)readData64bPacket.SLength);
						Log.w(this.comm.serialPort.PortName, msg);
					}
					break;
				}
			}
			while (!done);
			Log.w(this.comm.serialPort.PortName, "Send done packet");
			packet.Command = 5u;
			packet.Length = 8u;
			byte[] doneData = CommandFormat.StructToBytes(packet, 8);
			for (int i = 8; i < doneData.Length; i++)
			{
				doneData[i] = 0;
			}
			this.comm.WritePort(doneData, 0, doneData.Length);
			this.comm.getRecData();
			if (this.comm.recData.Length == 0)
			{
				this.comm.recData = new byte[48];
			}
			doneRsp = (sahara_done_response)CommandFormat.BytesToStuct(this.comm.recData, typeof(sahara_done_response));
			if (doneRsp.Command == 6u)
			{
				msg = string.Format("file {0} transferred successfully", programmer);
				Log.w(this.comm.serialPort.PortName, msg);
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), msg, "flashing", false);
				Thread.Sleep(1000);
				return;
			}
			msg = "programmer transfer error " + doneRsp.Command;
			throw new Exception(msg);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000452C File Offset: 0x0000272C
		private void DownloadMiniKernel()
		{
			string[] files = FileSearcher.SearchFiles(this.swPath, SoftwareImage.BootImage);
			if (files.Length <= 0)
			{
				throw new Exception("can not found boot file.");
			}
			string bootImage = files[0];
			string hostMagicStr = "QCOM fast download protocol host";
			string targetMagicStr = "QCOM fast download protocol targ";
			byte[] uHostMagic = Encoding.Default.GetBytes(hostMagicStr);
			Encoding.Default.GetBytes(targetMagicStr);
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), string.Format("Open boot file", bootImage), "flashing", false);
			byte[] sendData = CommandFormat.StructToBytes(new HelloCommand
			{
				uVersionNumber = 3,
				uCompatibleVersion = 3,
				uFeatureBits = 9,
				uMagicNumber = uHostMagic
			});
			int i = 1;
			HelloResponse rspHello;
			for (;;)
			{
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "send hello command " + i.ToString(), "flashing", false);
				this.TransmitPacket(sendData);
				Thread.Sleep(500);
				this.comm.getRecData();
				if (this.comm.recData.Length == 0)
				{
					this.comm.recData = new byte[48];
				}
				rspHello = (HelloResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(HelloResponse));
				if (rspHello.uResponse == 2)
				{
					goto IL_176;
				}
				if (i == 60)
				{
					break;
				}
				i++;
			}
			throw new Exception("target not ready.");
			IL_176:
			int uWindowSize = 1;
			int uMaxBlockSize = (int)rspHello.uMaxBlockSize;
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "Enable trusted security mode", "flashing", false);
			sendData = CommandFormat.StructToBytes(new SecurityModeCommand
			{
				uCommand = 23,
				uMode = 1
			});
			this.TransmitPacket(sendData);
			Thread.Sleep(500);
			this.comm.getRecData();
			if (this.comm.recData.Length == 0)
			{
				this.comm.recData = new byte[48];
			}
			if (((SimpleResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(SimpleResponse))).uResponse != 24)
			{
				throw new Exception("invalid state");
			}
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "Open EMMC card USER partition", "flashing", false);
			sendData = CommandFormat.StructToBytes(new OpenMultiImageCommand
			{
				uCommand = 27,
				uType = 33
			});
			this.TransmitPacket(sendData);
			Thread.Sleep(500);
			this.comm.getRecData();
			if (this.comm.recData.Length == 0)
			{
				this.comm.recData = new byte[48];
			}
			OpenMultiImageResponse rspOpenMultiImage = (OpenMultiImageResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(OpenMultiImageResponse));
			if (rspOpenMultiImage.uResponse != 28)
			{
				throw new Exception("invalid state");
			}
			if (rspOpenMultiImage.uStatus != 0)
			{
				throw new Exception("open failed");
			}
			int uNoAck = 0;
			StreamWriteCommand spCmdStreamWrite = default(StreamWriteCommand);
			FileTransfer transfer = new FileTransfer(this.comm.serialPort.PortName, bootImage);
			long uAddress = 0L;
			while (uAddress < transfer.getFileSize())
			{
				long tmp = transfer.getFileSize() - uAddress;
				long uWriteSize = (tmp < (long)uMaxBlockSize) ? tmp : ((long)uMaxBlockSize);
				if (uAddress % (long)(40 * uMaxBlockSize) == 0L)
				{
					FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), string.Format("StreamWrite address {0}, size {1}", uAddress, (tmp < (long)(40 * uMaxBlockSize)) ? tmp : ((long)(40 * uMaxBlockSize))), "flashing", false);
				}
				if (uNoAck == uWindowSize)
				{
					StreamWriteResponse rspStreamWrite = default(StreamWriteResponse);
					long uExpectedAddress = uAddress - (long)(uNoAck * uMaxBlockSize);
					this.comm.getRecData();
					if (this.comm.recData.Length == 0)
					{
						this.comm.recData = new byte[48];
					}
					rspStreamWrite = (StreamWriteResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(StreamWriteResponse));
					if (rspStreamWrite.uResponse != 8)
					{
						throw new Exception("open failed");
					}
					if (uExpectedAddress != (long)rspStreamWrite.uAddress)
					{
						throw new Exception("open failed");
					}
					uNoAck--;
				}
				spCmdStreamWrite.uCommand = 7;
				spCmdStreamWrite.uAddress = (int)uAddress;
				int j = 0;
				byte[] sendFileData = transfer.GetBytesFromFile(uAddress, (int)uWriteSize, out j);
				spCmdStreamWrite.uData = sendFileData;
				sendData = CommandFormat.StructToBytes(spCmdStreamWrite);
				this.TransmitPacket(sendData);
				uAddress += (long)uMaxBlockSize;
				uNoAck++;
			}
			while (uNoAck-- > 0)
			{
				StreamWriteResponse rspStreamWrite2 = default(StreamWriteResponse);
				this.comm.getRecData();
				if (this.comm.recData.Length == 0)
				{
					this.comm.recData = new byte[48];
				}
				if (((StreamWriteResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(StreamWriteResponse))).uResponse != 8)
				{
					throw new Exception("invalid state");
				}
			}
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "Close EMMC card USER partition", "flashing", false);
			SimpleCommand cmdSimple = new SimpleCommand
			{
				uCommand = 21
			};
			sendData = CommandFormat.StructToBytes(cmdSimple);
			this.TransmitPacket(sendData);
			this.comm.getRecData();
			if (this.comm.recData.Length == 0)
			{
				this.comm.recData = new byte[48];
			}
			if (((SimpleResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(SimpleResponse))).uResponse != 22)
			{
				throw new Exception("invalid state");
			}
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "Reboot to mini kernel", "flashing", false);
			cmdSimple.uCommand = 11;
			sendData = CommandFormat.StructToBytes(cmdSimple);
			this.TransmitPacket(sendData);
			this.comm.getRecData();
			if (this.comm.recData.Length == 0)
			{
				this.comm.recData = new byte[48];
			}
			if (((SimpleResponse)CommandFormat.BytesToStuct(this.comm.recData, typeof(SimpleResponse))).uResponse != 12)
			{
				throw new Exception("invalid state");
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004BD8 File Offset: 0x00002DD8
		public void TransmitPacket(byte[] sendData)
		{
			int uCRC16 = 0;
			List<byte> arrPacket = new List<byte>();
			arrPacket.Add(126);
			foreach (byte bOctet in sendData)
			{
				uCRC16 = CRC.BuildCRC16(uCRC16, (int)bOctet);
				this.AddPacketOctet(arrPacket, bOctet);
			}
			this.AddPacketOctet(arrPacket, (byte)(uCRC16 & 255));
			this.AddPacketOctet(arrPacket, (byte)(uCRC16 >> 8));
			arrPacket.Add(126);
			this.comm.WritePort(arrPacket.ToArray(), 0, arrPacket.Count);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004C54 File Offset: 0x00002E54
		public void ReceivePacket(byte[] pvPacket, bool bCheckSize)
		{
			int dwNext = 0;
			int uCRC16 = 0;
			byte[] bBuffer = new byte[256];
			int iState = 0;
			while (iState != 3)
			{
				byte bOctet = 0;
				this.comm.getRecData();
				bool jump = false;
				switch (iState)
				{
				case 0:
					if (bOctet == 126)
					{
						dwNext = 0;
						uCRC16 = 0;
						iState = 1;
					}
					break;
				case 1:
					if (bOctet == 126)
					{
						jump = true;
					}
					else
					{
						iState = 2;
					}
					break;
				case 2:
					if (bOctet != 126)
					{
						if (bOctet == 125)
						{
							bOctet ^= 32;
						}
						uCRC16 = CRC.BuildCRC16(uCRC16, (int)bOctet);
						if (dwNext < bBuffer.Length - 1)
						{
							bBuffer[dwNext] = bOctet;
						}
						if (dwNext++ < pvPacket.Length)
						{
							pvPacket[dwNext - 1] = bOctet;
						}
					}
					break;
				}
				if (jump)
				{
					return;
				}
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00004D05 File Offset: 0x00002F05
		public void AddPacketOctet(List<byte> arrPacket, byte bOctet)
		{
			if (bOctet == 126 || bOctet == 125)
			{
				arrPacket.Add(125);
				bOctet ^= 32;
			}
			arrPacket.Add(bOctet);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00004D27 File Offset: 0x00002F27
		private void PropareFirehose()
		{
			this.ping();
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004D30 File Offset: 0x00002F30
		private void ping()
		{
			Log.w(this.comm.serialPort.PortName, "send nop command");
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "ping target via firehose", "flashing", false);
			string cmd = string.Format(Firehose.Nop, this.verbose ? "1" : "0");
			if (!this.comm.SendCommand(cmd, true))
			{
				throw new Exception("ping target failed");
			}
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), "ping target via firehose", "flashing", false);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004DE4 File Offset: 0x00002FE4
		private string displace(string myStr, string displaceA, string displaceB)
		{
			string[] strArrayA = Regex.Split(myStr, displaceA);
			for (int i = 0; i < strArrayA.Length - 1; i++)
			{
				string[] array;
				IntPtr intPtr;
				(array = strArrayA)[(int)(intPtr = (IntPtr)i)] = array[(int)intPtr] + displaceB;
			}
			string returnStr = "";
			foreach (string var in strArrayA)
			{
				returnStr += var;
			}
			return returnStr;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004E48 File Offset: 0x00003048
		private string str2Hex(string str)
		{
			char[] strChars = str.ToCharArray();
			StringBuilder sb = new StringBuilder();
			foreach (char item in strChars)
			{
				int value = Convert.ToInt32(item);
				sb.Append(string.Format("{0:X}", value));
			}
			return sb.ToString();
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004EA4 File Offset: 0x000030A4
		private string getsigKey(string sig)
		{
			string strURL = "http://unlock.update.miui.com/test/rsa?r=0&data=" + sig;
			Log.w(this.comm.serialPort.PortName, "request URL:" + strURL);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader myreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
			string responseText = myreader.ReadToEnd();
			myreader.Close();
			return responseText;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004F18 File Offset: 0x00003118
		private bool dlAuth()
		{
			Log.w(this.comm.serialPort.PortName, "authentication edl.");
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, default(float?), "edl authentication", "authentication", false);
			string reqCmd = Firehose.REQ_AUTH;
			string authCmd = Firehose.AUTHP;
			bool result = this.comm.SendCommand(reqCmd, true);
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(this.comm.auth);
			XmlNode sigNode = doc.SelectSingleNode("sig");
			XmlElement sigMnt = (XmlElement)sigNode;
			string originKey = sigMnt.Attributes.get_ItemOf("value").Value;
			Log.w(this.comm.serialPort.PortName, string.Format("origin:{0}", originKey));
			string sig_key = "";
			PInvokeResultArg resultArg = EDL_SLA_Challenge.SignEdl(originKey, out sig_key);
			if (resultArg.result != 0)
			{
				throw new Exception("authentication failed " + resultArg.lastErrMsg);
			}
			Log.w(this.comm.serialPort.PortName, string.Format("siged:{0}", sig_key));
			List<byte> byteList = new List<byte>();
			for (int i = 0; i < sig_key.Length; i += 2)
			{
				string hex = sig_key.Substring(i, 2);
				byteList.Add(byte.Parse(hex, 512));
			}
			byte[] sig_arr = byteList.ToArray();
			string sigCmd = string.Format(authCmd, sig_arr.Length);
			result = this.comm.SendCommand(sigCmd, true);
			if (!result)
			{
				throw new Exception("authentication failed");
			}
			StringBuilder sb = new StringBuilder();
			StringBuilder sbhex = new StringBuilder();
			foreach (byte item in sig_arr)
			{
				sb.Append(item.ToString() + " ");
				sbhex.Append("0x" + item.ToString("X2") + " ");
			}
			this.comm.WritePort(sig_arr, 0, sig_arr.Length);
			if (!this.comm.GetResponse(true))
			{
				throw new Exception("authentication failed");
			}
			return result;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00005148 File Offset: 0x00003348
		private void ConfigureDDR(int intSectorSize, int buffer_sectors, string ddrType, int m_iSkipStorageInit)
		{
			Log.w(this.comm.serialPort.PortName, "send configure command");
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "send configure command", "flashing", false);
			string cmd = string.Format(Firehose.Configure, new object[]
			{
				this.verbose ? "1" : "0",
				intSectorSize * buffer_sectors,
				ddrType,
				m_iSkipStorageInit
			});
			bool result = this.comm.SendCommand(cmd, true);
			if (Storage.ufs.ToLower() == ddrType && !this.isLite && !result)
			{
				throw new Exception("send configure command failed");
			}
			Log.w(this.comm.serialPort.PortName, "max buffer sector is " + this.comm.m_dwBufferSectors);
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), "send command command", "flashing", false);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000526C File Offset: 0x0000346C
		private bool NeedProvision(string swpath)
		{
			bool needProvision = false;
			string[] files = FileSearcher.SearchFiles(swpath, SoftwareImage.ProvisionPattern);
			if (files.Length > 0)
			{
				needProvision = true;
			}
			return needProvision;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00005290 File Offset: 0x00003490
		private bool Provision(string swpath)
		{
			string[] files = FileSearcher.SearchFiles(swpath, SoftwareImage.ProvisionPattern);
			if (files.Length == 0)
			{
				return false;
			}
			string provisionFile = files[0];
			string msg = string.Format("start provision:{0}", provisionFile);
			Log.w(this.comm.serialPort.PortName, msg);
			XmlDocument provision_xml = new XmlDocument();
			XmlReader reader = XmlReader.Create(provisionFile, new XmlReaderSettings
			{
				IgnoreComments = true
			});
			provision_xml.Load(reader);
			XmlNode dataNode = provision_xml.SelectSingleNode("data");
			XmlNodeList list = dataNode.ChildNodes;
			int count = 0;
			try
			{
				foreach (object obj in list)
				{
					XmlNode item = (XmlNode)obj;
					XmlElement elemnt = (XmlElement)item;
					if (!(elemnt.Name.ToLower() != "ufs"))
					{
						StringBuilder strCmdProvision = new StringBuilder("<ufs ");
						foreach (object obj2 in elemnt.Attributes)
						{
							XmlAttribute attr = (XmlAttribute)obj2;
							if (!(attr.Name.ToLower() == "desc"))
							{
								strCmdProvision.Append(string.Format("{0}=\"{1}\" ", attr.Name, attr.Value));
							}
						}
						if (this.verbose)
						{
							strCmdProvision.Append(" verbose=\"1\" ");
						}
						strCmdProvision.Append("/>");
						string strCmd = string.Format("<?xml version=\"1.0\" ?>\n<data>\n{0}\n</data>", strCmdProvision.ToString());
						if (!this.comm.SendCommand(strCmd, true))
						{
							Log.w(this.comm.serialPort.PortName, "Provision failed :" + strCmd);
						}
						FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?((float)count / (float)list.Count), provisionFile, "provisioning", false);
						count++;
					}
				}
				Log.w(this.comm.serialPort.PortName, "Provision done.");
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), "provisiong done", "provisioning", false);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				reader.Close();
			}
			return this.Reboot(this.comm.serialPort.PortName);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00005584 File Offset: 0x00003784
		private bool Reboot(string portName)
		{
			bool rebootDone = false;
			Log.w(this.comm.serialPort.PortName, "restart target");
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), "restart target", "flashing", false);
			string cmd = string.Format(Firehose.Reset_To_Edl, this.verbose ? "1" : "0");
			if (!this.comm.SendCommand(cmd, true))
			{
				throw new Exception("restart target failed");
			}
			this.comm.serialPort.Close();
			this.comm.serialPort.Dispose();
			Thread.Sleep(5000);
			List<string> ds = Enumerable.ToList<string>(this.getDevice());
			int count = 100;
			string msg = "";
			while (count-- > 0 && ds.IndexOf(portName) < 0)
			{
				Thread.Sleep(100);
				ds = Enumerable.ToList<string>(this.getDevice());
				msg = string.Format("waiting for {0} restart", portName);
				Log.w(this.comm.serialPort.PortName, msg);
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, default(float?), msg, "restart", false);
			}
			if (ds.IndexOf(portName) >= 0)
			{
				rebootDone = true;
				msg = string.Format("{0} restart successfully", portName);
				Log.w(this.comm.serialPort.PortName, msg);
				Thread.Sleep(800);
				bool openSuccess = false;
				while (count-- > 0 && !openSuccess)
				{
					try
					{
						this.comm.serialPort.Open();
						openSuccess = true;
						Log.w(this.comm.serialPort.PortName, string.Format(" serial port {0} opend successfully", portName));
					}
					catch (Exception)
					{
						Log.w(this.comm.serialPort.PortName, string.Format("open serial port {0} ", portName));
						Thread.Sleep(800);
					}
				}
				rebootDone = (rebootDone && this.comm.IsOpen);
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), msg, "restart", false);
				return rebootDone;
			}
			msg = string.Format("{0} restart failed", portName);
			Log.w(this.comm.serialPort.PortName, msg);
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), msg, "restart", false);
			rebootDone = false;
			throw new Exception(msg);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005814 File Offset: 0x00003A14
		private void SetBootPartition()
		{
			string msg = "Set Boot Partition ";
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), msg, "flashing", false);
			string cmd = string.Format(Firehose.SetBootPartition, this.verbose ? "1" : "0");
			if (!this.comm.SendCommand(cmd, true))
			{
				throw new Exception("set boot partition failed");
			}
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), msg, "flashing", false);
			Log.w(this.comm.serialPort.PortName, msg);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000058C4 File Offset: 0x00003AC4
		private void FirehoseDownloadImg(string swPath)
		{
			string[] programs = FileSearcher.SearchFiles(swPath, SoftwareImage.RawProgramPattern);
			string[] patchs = FileSearcher.SearchFiles(swPath, SoftwareImage.PatchPattern);
			for (int i = 0; i < programs.Length; i++)
			{
				if (this.WriteFilesToDevice(this.comm.serialPort.PortName, swPath, programs[i]))
				{
					this.ApplyPatchesToDevice(this.comm.serialPort.PortName, patchs[i]);
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00005930 File Offset: 0x00003B30
		private void GetSha256(string swPath)
		{
			if (string.IsNullOrEmpty(this.sha256Path))
			{
				string[] programs = FileSearcher.SearchFiles(swPath, SoftwareImage.RawProgramPattern);
				for (int i = 0; i < programs.Length; i++)
				{
					Log.w(this.comm.serialPort.PortName, string.Format("sha256 {0}", programs[i]));
					this.GetSha256Digest(this.comm.serialPort.PortName, swPath, programs[i]);
				}
				return;
			}
			Log.w(this.comm.serialPort.PortName, string.Format("sha256 {0}", this.sha256Path));
			this.GetSha256Digest(this.comm.serialPort.PortName, swPath, this.sha256Path);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000059E4 File Offset: 0x00003BE4
		private bool WriteFilesToDevice(string portName, string swPath, string rawFilePath)
		{
			bool result = true;
			Log.w(this.comm.serialPort.PortName, string.Format("open program file {0}", rawFilePath));
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, default(float?), rawFilePath, "flashing", false);
			XmlDocument raw_xml = new XmlDocument();
			XmlReader reader = XmlReader.Create(rawFilePath, new XmlReaderSettings
			{
				IgnoreComments = true
			});
			raw_xml.Load(reader);
			XmlNode dataNode = raw_xml.SelectSingleNode("data");
			XmlNodeList list = dataNode.ChildNodes;
			try
			{
				bool bSparse = false;
				string strImageFile = "";
				string strFileStartSector = "0";
				string start_sector = "0";
				string strPartitionSectorNumber = "0";
				string strPhysicalPartitionNumber = "0";
				string strSectorSizeInBytes = "512";
				string partition_name = "";
				foreach (object obj in list)
				{
					XmlNode item = (XmlNode)obj;
					XmlElement elemnt = (XmlElement)item;
					if (!(elemnt.Name.ToLower() != "program"))
					{
						foreach (object obj2 in elemnt.Attributes)
						{
							XmlAttribute attr = (XmlAttribute)obj2;
							string text;
							if ((text = attr.Name.ToLower()) != null)
							{
								if (<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000053-1 == null)
								{
									Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
									dictionary.Add("file_sector_offset", 0);
									dictionary.Add("filename", 1);
									dictionary.Add("num_partition_sectors", 2);
									dictionary.Add("start_sector", 3);
									dictionary.Add("sparse", 4);
									dictionary.Add("sector_size_in_bytes", 5);
									dictionary.Add("physical_partition_number", 6);
									dictionary.Add("label", 7);
									<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000053-1 = dictionary;
								}
								int num;
								if (<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000053-1.TryGetValue(text, ref num))
								{
									switch (num)
									{
									case 0:
										strFileStartSector = attr.Value;
										break;
									case 1:
										strImageFile = attr.Value;
										break;
									case 2:
										strPartitionSectorNumber = attr.Value;
										break;
									case 3:
										start_sector = attr.Value;
										break;
									case 4:
										bSparse = (attr.Value == "true");
										break;
									case 5:
										strSectorSizeInBytes = attr.Value;
										break;
									case 6:
										strPhysicalPartitionNumber = attr.Value;
										break;
									case 7:
										partition_name = attr.Value;
										break;
									}
								}
							}
						}
						if (!string.IsNullOrEmpty(strImageFile))
						{
							this.comm.writeCount = 0;
							this.comm.CleanBuffer();
							strImageFile = swPath + "\\" + strImageFile;
							if (!File.Exists(strImageFile))
							{
								throw new Exception(string.Format("file {0} not found.", strImageFile));
							}
							if (strImageFile.IndexOf("gpt_main1") >= 0 || strImageFile.IndexOf("gpt_main2") >= 0)
							{
								Thread.Sleep(1000);
							}
							string addtionalFirehose = "";
							if (this.readBackVerify)
							{
								addtionalFirehose = "read_back_verify=\"1\"";
							}
							if (bSparse)
							{
								Log.w(this.comm.serialPort.PortName, string.Format("Write sparse file {0} to partition[{1}] sector {2}", strImageFile, partition_name, start_sector));
								FileTransfer ft = new FileTransfer(this.comm.serialPort.PortName, strImageFile);
								ft.WriteSparseFileToDevice(this, start_sector, strPartitionSectorNumber, strImageFile, strFileStartSector, strSectorSizeInBytes, strPhysicalPartitionNumber, addtionalFirehose);
								ft.closeTransfer();
							}
							else
							{
								Log.w(this.comm.serialPort.PortName, string.Format("Write file {0} to partition[{1}] sector {2}", strImageFile, partition_name, start_sector));
								FileTransfer ft2 = new FileTransfer(this.comm.serialPort.PortName, strImageFile);
								ft2.WriteFile(this, start_sector, strPartitionSectorNumber, strImageFile, strFileStartSector, "0", strSectorSizeInBytes, strPhysicalPartitionNumber, addtionalFirehose, true, new int?(1));
								ft2.closeTransfer();
							}
							Log.w(this.comm.serialPort.PortName, string.Format("Image {0} transferred successfully", strImageFile));
							this.comm.writeCount = 0;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				reader.Close();
			}
			return result;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00005E78 File Offset: 0x00004078
		private void GetSha256Digest(string portName, string swPath, string rawFilePath)
		{
			Log.w(this.comm.serialPort.PortName, string.Format("open file {0}", rawFilePath));
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, default(float?), rawFilePath, "flashing", false);
			XmlDocument raw_xml = new XmlDocument();
			XmlReader reader = XmlReader.Create(rawFilePath, new XmlReaderSettings
			{
				IgnoreComments = true
			});
			raw_xml.Load(reader);
			XmlNode dataNode = raw_xml.SelectSingleNode("data");
			XmlNodeList list = dataNode.ChildNodes;
			try
			{
				string strImageFile = "";
				string strFileStartSector = "0";
				string start_sector = "0";
				string strPartitionSectorNumber = "0";
				string strPhysicalPartitionNumber = "0";
				string strSectorSizeInBytes = "512";
				foreach (object obj in list)
				{
					XmlNode item = (XmlNode)obj;
					XmlElement elemnt = (XmlElement)item;
					if (!(elemnt.Name.ToLower() != "program"))
					{
						foreach (object obj2 in elemnt.Attributes)
						{
							XmlAttribute attr = (XmlAttribute)obj2;
							string text;
							if ((text = attr.Name.ToLower()) != null)
							{
								if (<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000054-1 == null)
								{
									Dictionary<string, int> dictionary = new Dictionary<string, int>(8);
									dictionary.Add("file_sector_offset", 0);
									dictionary.Add("filename", 1);
									dictionary.Add("num_partition_sectors", 2);
									dictionary.Add("start_sector", 3);
									dictionary.Add("sparse", 4);
									dictionary.Add("sector_size_in_bytes", 5);
									dictionary.Add("physical_partition_number", 6);
									dictionary.Add("label", 7);
									<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000054-1 = dictionary;
								}
								int num;
								if (<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000054-1.TryGetValue(text, ref num))
								{
									switch (num)
									{
									case 0:
										strFileStartSector = attr.Value;
										break;
									case 1:
										strImageFile = attr.Value;
										break;
									case 2:
										strPartitionSectorNumber = attr.Value;
										break;
									case 3:
										start_sector = attr.Value;
										break;
									case 4:
										attr.Value == "true";
										break;
									case 5:
										strSectorSizeInBytes = attr.Value;
										break;
									case 6:
										strPhysicalPartitionNumber = attr.Value;
										break;
									case 7:
									{
										string value = attr.Value;
										break;
									}
									}
								}
							}
						}
						if (!string.IsNullOrEmpty(strImageFile))
						{
							long ullPartitionSectorNumber = Convert.ToInt64(strPartitionSectorNumber);
							if (ullPartitionSectorNumber == 0L)
							{
								ullPartitionSectorNumber = 2147483647L;
							}
							Convert.ToInt64(strFileStartSector);
							long ullSectorSizeInBytes = Convert.ToInt64(strSectorSizeInBytes);
							long ullPhysicalParitionNumber = Convert.ToInt64(strPhysicalPartitionNumber);
							Log.w(this.comm.serialPort.PortName, string.Format("checking sha256 {0}", strImageFile));
							FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, default(float?), string.Format("checking sha256 {0}", strImageFile), "checking sha256", false);
							string cmd = string.Format(Firehose.FIREHOSE_SHA256DIGEST, new object[]
							{
								ullSectorSizeInBytes,
								ullPartitionSectorNumber,
								start_sector,
								ullPhysicalParitionNumber
							});
							if (!this.comm.SendCommand(cmd, true))
							{
								bool hasACK = false;
								bool hasNAK = false;
								while (!hasNAK && !hasACK)
								{
									List<XmlDocument> docs = this.comm.GetResponseXml(true);
									foreach (XmlDocument sitem in docs)
									{
										XmlNode sdataNode = sitem.SelectSingleNode("data");
										XmlNodeList slist = sdataNode.ChildNodes;
										foreach (object obj3 in slist)
										{
											XmlNode node = (XmlNode)obj3;
											XmlElement selement = (XmlElement)node;
											foreach (object obj4 in selement.Attributes)
											{
												XmlAttribute attr2 = (XmlAttribute)obj4;
												if (attr2.Value.ToLower() == "ack")
												{
													hasACK = true;
												}
												else if (attr2.Value.ToLower() == "nak")
												{
													hasNAK = true;
												}
											}
										}
									}
									Thread.Sleep(50);
									docs = this.comm.GetResponseXml(false);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				reader.Close();
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000063E8 File Offset: 0x000045E8
		private bool ApplyPatchesToDevice(string portName, string patchFilePath)
		{
			bool result = true;
			Log.w(this.comm.serialPort.PortName, string.Format("open patch file {0}", patchFilePath));
			FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(0f), patchFilePath, "flashing", false);
			XmlDocument raw_xml = new XmlDocument();
			XmlReader reader = XmlReader.Create(patchFilePath, new XmlReaderSettings
			{
				IgnoreComments = true
			});
			raw_xml.Load(reader);
			XmlNode dataNode = raw_xml.SelectSingleNode("patches");
			XmlNodeList list = dataNode.ChildNodes;
			string strImageFile = "";
			string strPatchSize = "0";
			string strPatchValue = "0";
			string strDiskOffsetSector = "0";
			string strSectorOffsetByte = "0";
			string strPhysicalPartitionNumber = "0";
			string strSectorSizeInBytes = "512";
			try
			{
				foreach (object obj in list)
				{
					XmlNode item = (XmlNode)obj;
					XmlElement elemnt = (XmlElement)item;
					if (!(elemnt.Name.ToLower() != "patch"))
					{
						foreach (object obj2 in elemnt.Attributes)
						{
							XmlAttribute attr = (XmlAttribute)obj2;
							string text;
							if ((text = attr.Name.ToLower()) != null)
							{
								if (<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000055-1 == null)
								{
									Dictionary<string, int> dictionary = new Dictionary<string, int>(7);
									dictionary.Add("byte_offset", 0);
									dictionary.Add("filename", 1);
									dictionary.Add("size_in_bytes", 2);
									dictionary.Add("start_sector", 3);
									dictionary.Add("value", 4);
									dictionary.Add("sector_size_in_bytes", 5);
									dictionary.Add("physical_partition_number", 6);
									<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000055-1 = dictionary;
								}
								int num;
								if (<PrivateImplementationDetails>{3E4AFA2A-C512-4350-9A93-E9FAB26EB066}.$$method0x6000055-1.TryGetValue(text, ref num))
								{
									switch (num)
									{
									case 0:
										strSectorOffsetByte = attr.Value;
										break;
									case 1:
										strImageFile = attr.Value;
										break;
									case 2:
										strPatchSize = attr.Value;
										break;
									case 3:
										strDiskOffsetSector = attr.Value;
										break;
									case 4:
										strPatchValue = attr.Value;
										break;
									case 5:
										strSectorSizeInBytes = attr.Value;
										break;
									case 6:
										strPhysicalPartitionNumber = attr.Value;
										break;
									}
								}
							}
						}
						if (strImageFile.ToLower() == "disk")
						{
							this.ApplyPatch(strDiskOffsetSector, strSectorOffsetByte, strPatchValue, strPatchSize, strSectorSizeInBytes, strPhysicalPartitionNumber);
						}
					}
				}
				FlashingDevice.UpdateDeviceStatus(this.comm.serialPort.PortName, new float?(1f), patchFilePath, "flashing", false);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			finally
			{
				reader.Close();
			}
			return result;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000671C File Offset: 0x0000491C
		private void ApplyPatch(string pszDiskOffsetSector, string pszSectorOffsetByte, string pszPatchValue, string pszPatchSize, string pszSectorSizeInBytes, string pszPhysicalPartitionNumber)
		{
			Log.w(this.comm.serialPort.PortName, string.Format("ApplyPatch sector {0}, offset {1}, value {2}, size {3}", new object[]
			{
				pszDiskOffsetSector,
				pszSectorOffsetByte,
				pszPatchValue,
				pszPatchSize
			}));
			string addtionalFirehose = "";
			if (this.readBackVerify)
			{
				addtionalFirehose = "read_back_verify=\"1\"";
			}
			string cmd = string.Format(Firehose.FIREHOSE_PATCH, new object[]
			{
				pszSectorSizeInBytes,
				pszSectorOffsetByte,
				pszPhysicalPartitionNumber,
				pszPatchSize,
				pszDiskOffsetSector,
				pszPatchValue,
				addtionalFirehose
			});
			this.comm.SendCommand(cmd);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000067B4 File Offset: 0x000049B4
		public override string[] getDevice()
		{
			return ComPortCtrl.getDevicesQc();
		}

		// Token: 0x04000024 RID: 36
		public Comm comm = new Comm();

		// Token: 0x04000025 RID: 37
		private int BUFFER_SECTORS = 256;

		// Token: 0x04000026 RID: 38
		private int programmerType = 1;

		// Token: 0x04000027 RID: 39
		private string storageType = "ufs";

		// Token: 0x04000028 RID: 40
		private bool isLite;

		// Token: 0x04000029 RID: 41
		private int m_iSkipStorageInit;
	}
}

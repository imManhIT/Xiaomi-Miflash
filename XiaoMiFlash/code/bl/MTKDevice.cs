using System;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x0200001F RID: 31
	public class MTKDevice : DeviceCtrl
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x0000CC68 File Offset: 0x0000AE68
		public override void flash()
		{
			try
			{
				string downloadTool = Script.SP_Download_Tool_PATH + "\\SP_download_tool.exe";
				string parameter = " -scatter {0} -da {1} -dl_type {2} -usb_com {3} -chk_sum {4} -s_order {5} ";
				IniFile ini = new IniFile();
				int daCom = ini.GetIniInt("bootrom", string.Format("com{0}", this.comPortIndex), 0);
				string downloadComd;
				if (daCom == 0)
				{
					downloadComd = downloadTool + string.Format(parameter, new object[]
					{
						this.scatter,
						this.da,
						this.dl_type,
						this.comPortIndex,
						this.chkSum.ToString(),
						this.sort
					});
				}
				else
				{
					parameter += " -usb_da_com {6}";
					downloadComd = downloadTool + string.Format(parameter, new object[]
					{
						this.scatter,
						this.da,
						this.dl_type,
						this.comPortIndex,
						this.chkSum.ToString(),
						this.sort,
						daCom
					});
				}
				Log.w(downloadComd);
				Cmd cmd = new Cmd("com" + this.comPortIndex.ToString(), "");
				cmd.consoleMode_Execute_returnLine(this.deviceName, downloadComd, 1);
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000CE00 File Offset: 0x0000B000
		public override string[] getDevice()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000CE07 File Offset: 0x0000B007
		public override void CheckSha256()
		{
			throw new NotImplementedException();
		}

		// Token: 0x040000B2 RID: 178
		public int comPortIndex;

		// Token: 0x040000B3 RID: 179
		public int chkSum;

		// Token: 0x040000B4 RID: 180
		public int sort;
	}
}

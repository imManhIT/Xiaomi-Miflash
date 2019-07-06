using System;
using System.Net.Sockets;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash
{
	// Token: 0x0200003E RID: 62
	internal class SocketChatClient
	{
		// Token: 0x060001AE RID: 430 RVA: 0x00010DF2 File Offset: 0x0000EFF2
		public SocketChatClient(Socket sock)
		{
			this.m_sock = sock;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00010E0E File Offset: 0x0000F00E
		public Socket Sock
		{
			get
			{
				return this.m_sock;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00010E18 File Offset: 0x0000F018
		public void SetupRecieveCallback(MainFrm app)
		{
			try
			{
				AsyncCallback recieveData = new AsyncCallback(app.OnRecievedData);
				this.m_sock.BeginReceive(this.m_byBuff, 0, this.m_byBuff.Length, 0, recieveData, this);
			}
			catch (Exception ex)
			{
				Log.w(string.Format("Recieve callback setup failed! {0}", ex.Message));
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00010E7C File Offset: 0x0000F07C
		public byte[] GetRecievedData(IAsyncResult ar)
		{
			int nBytesRec = 0;
			try
			{
				nBytesRec = this.m_sock.EndReceive(ar);
			}
			catch
			{
			}
			byte[] byReturn = new byte[nBytesRec];
			Array.Copy(this.m_byBuff, byReturn, nBytesRec);
			return byReturn;
		}

		// Token: 0x0400013C RID: 316
		private Socket m_sock;

		// Token: 0x0400013D RID: 317
		private byte[] m_byBuff = new byte[50];
	}
}

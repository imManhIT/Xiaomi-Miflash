using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x0200001A RID: 26
	public class StreamingArg
	{
		// Token: 0x04000090 RID: 144
		public const int HELLO_COMMAND = 1;

		// Token: 0x04000091 RID: 145
		public const int HELLO_RESPONSE = 2;

		// Token: 0x04000092 RID: 146
		public const int READ_PACKET_COMMAND = 3;

		// Token: 0x04000093 RID: 147
		public const int READ_PACKET_RESPONSE = 4;

		// Token: 0x04000094 RID: 148
		public const int STREAM_WRITE_COMMAND = 7;

		// Token: 0x04000095 RID: 149
		public const int STREAM_WRITE_RESPONSE = 8;

		// Token: 0x04000096 RID: 150
		public const int RESET_COMMAND = 11;

		// Token: 0x04000097 RID: 151
		public const int RESET_RESPONSE = 12;

		// Token: 0x04000098 RID: 152
		public const int ERROR_RESPONSE = 13;

		// Token: 0x04000099 RID: 153
		public const int LOG_RESPONSE = 14;

		// Token: 0x0400009A RID: 154
		public const int CLOSE_COMMAND = 21;

		// Token: 0x0400009B RID: 155
		public const int CLOSE_RESPONSE = 22;

		// Token: 0x0400009C RID: 156
		public const int SECURITY_MODE_COMMAND = 23;

		// Token: 0x0400009D RID: 157
		public const int SECURITY_MODE_RESPONSE = 24;

		// Token: 0x0400009E RID: 158
		public const int OPEN_MULTI_IMAGE_COMMAND = 27;

		// Token: 0x0400009F RID: 159
		public const int OPEN_MULTI_IMAGE_RESPONSE = 28;

		// Token: 0x040000A0 RID: 160
		public const int ASYNC_HDLC_FLAG = 126;

		// Token: 0x040000A1 RID: 161
		public const int ASYNC_HDLC_ESC = 125;

		// Token: 0x040000A2 RID: 162
		public const int ASYNC_HDLC_MASK = 32;

		// Token: 0x040000A3 RID: 163
		public const int RCV_HUNT_FLAG = 0;

		// Token: 0x040000A4 RID: 164
		public const int RCV_GOT_FLAG = 1;

		// Token: 0x040000A5 RID: 165
		public const int RCV_GATHER_DATA = 2;

		// Token: 0x040000A6 RID: 166
		public const int RCV_GOT_PACKET = 3;

		// Token: 0x040000A7 RID: 167
		public const int CRC16_SEED = 0;

		// Token: 0x040000A8 RID: 168
		public const int CRC16_OK = 3911;

		// Token: 0x040000A9 RID: 169
		public const int CRC32_SEED = 0;
	}
}

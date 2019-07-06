using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using XiaoMiFlash.code.module;

namespace XiaoMiFlash.code.authFlash
{
	// Token: 0x02000008 RID: 8
	public class EDL_SLA_Challenge
	{
		// Token: 0x06000024 RID: 36
		[DllImport("SLA_Challenge.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int StartLoginProcess();

		// Token: 0x06000025 RID: 37
		[DllImport("SLA_Challenge.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int get_user_info(IntPtr usr_arg, IntPtr user_info);

		// Token: 0x06000026 RID: 38
		[DllImport("SLA_Challenge.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int free_user_info(IntPtr usr_arg, IntPtr user_info);

		// Token: 0x06000027 RID: 39
		[DllImport("SLA_Challenge.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int SLA_Challenge(IntPtr obj, byte[] challenge_in, int in_len, out IntPtr challenge_out, ref int out_len);

		// Token: 0x06000028 RID: 40
		[DllImport("SLA_Challenge.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int SLA_Challenge_End(IntPtr obj, IntPtr challenge_out);

		// Token: 0x06000029 RID: 41
		[DllImport("SLA_Challenge.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern int can_flash(IntPtr obj);

		// Token: 0x0600002A RID: 42 RVA: 0x00002BA4 File Offset: 0x00000DA4
		private static string decodeOut(string str)
		{
			char[] chars = str.ToCharArray();
			Encoder encoder = Encoding.Unicode.GetEncoder();
			byte[] bytes = new byte[encoder.GetByteCount(chars, 0, chars.Length, true)];
			encoder.GetBytes(chars, 0, chars.Length, bytes, 0, true);
			Decoder decoder = Encoding.UTF8.GetDecoder();
			int charSize = decoder.GetCharCount(bytes, 0, bytes.Length);
			char[] chs = new char[charSize];
			decoder.GetChars(bytes, 0, bytes.Length, chs, 0);
			return new string(chs);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002C1C File Offset: 0x00000E1C
		public static PInvokeResultArg GetUserInfo(MiUserInfo miUser)
		{
			IntPtr handle = IntPtr.Zero;
			int size = Marshal.SizeOf(typeof(UserInfo));
			IntPtr infosIntptr = Marshal.AllocHGlobal(size);
			int i = EDL_SLA_Challenge.get_user_info(handle, infosIntptr);
			UserInfo info = (UserInfo)Marshal.PtrToStructure(infosIntptr, typeof(UserInfo));
			miUser.id = Marshal.PtrToStringAnsi(info.user_id);
			string name = Marshal.PtrToStringUni(info.user_name);
			miUser.name = EDL_SLA_Challenge.decodeOut(name);
			PInvokeResultArg arg = new PInvokeResultArg();
			arg.result = i;
			arg.lastErrCode = Marshal.GetLastWin32Error();
			arg.lastErrMsg = new Win32Exception(arg.lastErrCode).Message;
			if (i != 0)
			{
				return arg;
			}
			i = EDL_SLA_Challenge.free_user_info(handle, infosIntptr);
			arg.result = i;
			arg.lastErrCode = Marshal.GetLastWin32Error();
			arg.lastErrMsg = new Win32Exception(arg.lastErrCode).Message;
			return arg;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002D04 File Offset: 0x00000F04
		public static PInvokeResultArg AuthFlash()
		{
			IntPtr handle = IntPtr.Zero;
			int i = EDL_SLA_Challenge.can_flash(handle);
			PInvokeResultArg arg = new PInvokeResultArg();
			arg.result = i;
			arg.lastErrCode = Marshal.GetLastWin32Error();
			arg.lastErrMsg = new Win32Exception(arg.lastErrCode).Message;
			return arg;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002D50 File Offset: 0x00000F50
		public static PInvokeResultArg SignEdl(string orignKey, out string signedKey)
		{
			IntPtr handle = IntPtr.Zero;
			IntPtr pBuff = default(IntPtr);
			int out_size = 0;
			int i = EDL_SLA_Challenge.SLA_Challenge(handle, Encoding.Default.GetBytes(orignKey), orignKey.Length, out pBuff, ref out_size);
			new StringBuilder();
			byte[] result_sig = new byte[out_size];
			for (int j = 0; j < out_size; j++)
			{
				IntPtr pPonitor;
				pPonitor..ctor(pBuff.ToInt64() + (long)(Marshal.SizeOf(typeof(byte)) * j));
				result_sig[j] = Marshal.ReadByte(pPonitor);
			}
			PInvokeResultArg arg = new PInvokeResultArg();
			arg.result = i;
			arg.lastErrCode = Marshal.GetLastWin32Error();
			arg.lastErrMsg = new Win32Exception(arg.lastErrCode).Message;
			if (i != 0)
			{
				signedKey = string.Empty;
				return arg;
			}
			byte[] ouArr = result_sig;
			StringBuilder sb_sig = new StringBuilder();
			for (int k = 0; k < ouArr.Length; k++)
			{
				sb_sig.Append(ouArr[k].ToString("X2"));
			}
			signedKey = sb_sig.ToString();
			i = EDL_SLA_Challenge.SLA_Challenge_End(handle, pBuff);
			arg.result = i;
			arg.lastErrCode = Marshal.GetLastWin32Error();
			arg.lastErrMsg = new Win32Exception(arg.lastErrCode).Message;
			return arg;
		}
	}
}

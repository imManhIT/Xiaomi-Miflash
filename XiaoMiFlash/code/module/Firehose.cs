using System;

namespace XiaoMiFlash.code.module
{
	// Token: 0x02000036 RID: 54
	public class Firehose
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600015D RID: 349 RVA: 0x0000FDE7 File Offset: 0x0000DFE7
		public static string Reset_To_Edl
		{
			get
			{
				return Firehose._reset_to_edl;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000FDEE File Offset: 0x0000DFEE
		public static string SetBootPartition
		{
			get
			{
				return Firehose._set_boot_partition;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000FDF5 File Offset: 0x0000DFF5
		public static string Nop
		{
			get
			{
				return Firehose._nop;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		public static string Configure
		{
			get
			{
				return Firehose._configure;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000FE03 File Offset: 0x0000E003
		public static int payload_size
		{
			get
			{
				return 1048576;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000FE0A File Offset: 0x0000E00A
		public static int MAX_PATCH_VALUE_LEN
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000FE0E File Offset: 0x0000E00E
		public static string FIREHOSE_PROGRAM
		{
			get
			{
				return "<?xml version=\"1.0\" ?><data><program SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" start_sector=\"{2}\" physical_partition_number=\"{3}\" {4}/></data>";
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000FE15 File Offset: 0x0000E015
		public static string FIREHOSE_SHA256DIGEST
		{
			get
			{
				return "<?xml version=\"1.0\" ?><data><getsha256digest SECTOR_SIZE_IN_BYTES=\"{0}\" num_partition_sectors=\"{1}\" start_sector=\"{2}\" physical_partition_number=\"{3}\"/></data>";
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000165 RID: 357 RVA: 0x0000FE1C File Offset: 0x0000E01C
		public static string FIREHOSE_PATCH
		{
			get
			{
				return "<?xml version=\"1.0\" ?><data><patch SECTOR_SIZE_IN_BYTES=\"{0}\" byte_offset=\"{1}\" filename=\"DISK\" physical_partition_number=\"{2}\" size_in_bytes=\"{3}\" start_sector=\"{4}\" value=\"{5}\" what=\"Update\" {6}/></data>";
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000FE23 File Offset: 0x0000E023
		public static string REQ_AUTH
		{
			get
			{
				return " <sig TargetName=\"req\" verbose=\"1\"/>";
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000FE2A File Offset: 0x0000E02A
		public static string AUTH
		{
			get
			{
				return " <sig TargetName=\"sig\" value=\"{0}\" verbose=\"1\"/>";
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000168 RID: 360 RVA: 0x0000FE31 File Offset: 0x0000E031
		public static string AUTHP
		{
			get
			{
				return " <sig TargetName=\"sig\" size_in_bytes=\"{0}\" verbose=\"1\"/>";
			}
		}

		// Token: 0x0400010B RID: 267
		private static string _reset_to_edl = "<?xml version=\"1.0\" ?><data><power verbose=\"{0}\"  value=\"reset_to_edl\"/></data>";

		// Token: 0x0400010C RID: 268
		private static string _set_boot_partition = "<?xml version=\"1.0\" ?><data><setbootablestoragedrive verbose=\"{0}\"  value=\"1\"/></data>";

		// Token: 0x0400010D RID: 269
		private static string _nop = "<?xml version=\"1.0\" ?><data><nop verbose=\"{0}\"  value=\"ping\"/></data>";

		// Token: 0x0400010E RID: 270
		private static string _configure = "<?xml version=\"1.0\" ?><data><configure verbose=\"{0}\" ZlpAwareHost=\"1\" MaxPayloadSizeToTargetInBytes=\"{1}\" MemoryName=\"{2}\" SkipStorageInit=\"{3}\"/></data>";
	}
}

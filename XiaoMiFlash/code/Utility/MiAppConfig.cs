using System;
using System.Configuration;
using System.Windows.Forms;
using System.Xml;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x0200000E RID: 14
	public class MiAppConfig
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000067EC File Offset: 0x000049EC
		public static void Add(string key, string value)
		{
			Configuration cfa = ConfigurationManager.OpenExeConfiguration(0);
			cfa.AppSettings.Settings.Add(key, value);
			cfa.Save();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00006818 File Offset: 0x00004A18
		public static void SetValue(string AppKey, string AppValue)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(0);
			config.AppSettings.Settings[AppKey].Value = AppValue;
			config.Save(0);
			ConfigurationManager.RefreshSection("appSettings");
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00006854 File Offset: 0x00004A54
		public static string GetAppConfig(string appKey)
		{
			XmlDocument xDoc = new XmlDocument();
			xDoc.Load(Application.ExecutablePath + ".config");
			XmlNode xNode = xDoc.SelectSingleNode("//appSettings");
			XmlElement xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
			if (xElem != null)
			{
				return xElem.Attributes.get_ItemOf("value").Value;
			}
			return string.Empty;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000068C2 File Offset: 0x00004AC2
		public static string Get(string key)
		{
			if (ConfigurationManager.AppSettings[key] == null)
			{
				MiAppConfig.Add(key, "");
			}
			if (ConfigurationManager.AppSettings[key] != null)
			{
				return ConfigurationManager.AppSettings[key].ToString();
			}
			return "";
		}
	}
}

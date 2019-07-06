using System;
using System.Xml;

namespace XiaoMiFlash.code.lan
{
	// Token: 0x02000022 RID: 34
	public class LanguageProvider
	{
		// Token: 0x060000F7 RID: 247 RVA: 0x0000D658 File Offset: 0x0000B858
		public LanguageProvider(string lanType)
		{
			this.languageType = lanType;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000D674 File Offset: 0x0000B874
		public string GetLanguage(string ctrlID)
		{
			XmlDocument provision_xml = new XmlDocument();
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.IgnoreComments = true;
			XmlReader reader = XmlReader.Create(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Source\\LanguageLibrary.xml");
			provision_xml.Load(reader);
			XmlNode dataNode = provision_xml.SelectSingleNode("LanguageLibrary");
			XmlNodeList list = dataNode.ChildNodes;
			string text = "";
			foreach (object obj in list)
			{
				XmlNode item = (XmlNode)obj;
				XmlElement elemnt = (XmlElement)item;
				if (!(elemnt.Name.ToLower() != "lan") && elemnt.Attributes.get_ItemOf("CTRLID").Value == ctrlID)
				{
					text = elemnt.Attributes.get_ItemOf(this.languageType).Value;
					break;
				}
			}
			return text;
		}

		// Token: 0x040000B6 RID: 182
		private string languageType = "";
	}
}

using System;
using System.Collections;
using System.IO;
using System.Xml;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x0200001D RID: 29
	public class ImageValidation
	{
		// Token: 0x060000E4 RID: 228 RVA: 0x0000CA28 File Offset: 0x0000AC28
		public static string Validate(string path)
		{
			bool result = true;
			string message = "md5 validate successfully.";
			new Hashtable();
			DirectoryInfo info = new DirectoryInfo(path);
			DirectoryInfo[] dics = info.GetDirectories();
			foreach (DirectoryInfo item in dics)
			{
				if (item.Name.ToLower().IndexOf("images") >= 0)
				{
					info = item;
					break;
				}
			}
			string imgPath = info.FullName;
			new Hashtable();
			string md5sumPath = info.Parent.FullName + "\\md5sum.xml";
			if (File.Exists(md5sumPath))
			{
				XmlDocument provision_xml = new XmlDocument();
				XmlReader reader = XmlReader.Create(md5sumPath, new XmlReaderSettings
				{
					IgnoreComments = true
				});
				provision_xml.Load(reader);
				XmlNode dataNode = provision_xml.SelectSingleNode("root");
				XmlNode digestsNode = dataNode.FirstChild;
				XmlNodeList list = digestsNode.ChildNodes;
				using (IEnumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XmlNode item2 = (XmlNode)obj;
						XmlElement elemnt = (XmlElement)item2;
						foreach (object obj2 in elemnt.Attributes)
						{
							XmlAttribute attr = (XmlAttribute)obj2;
							if (attr.Name.ToLower() == "name")
							{
								string imgName = attr.Value.ToLower();
								string md5value = elemnt.InnerText;
								imgPath = info.FullName + string.Format("\\{0}", imgName);
								string currentMd5 = Utility.GetMD5HashFromFile(imgPath);
								if (currentMd5 != md5value)
								{
									message = string.Format("{0} md5 validate failed!", imgPath);
									result = false;
									break;
								}
								Log.w(string.Format("{0} md5 valide success.", imgPath));
							}
						}
						if (!result)
						{
							break;
						}
					}
					return message;
				}
			}
			message = "not found md5sum.xml.";
			return message;
		}
	}
}

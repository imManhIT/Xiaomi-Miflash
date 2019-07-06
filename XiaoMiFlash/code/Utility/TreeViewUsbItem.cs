using System;
using System.Collections.Generic;
using MiUSB;

namespace XiaoMiFlash.code.Utility
{
	// Token: 0x02000027 RID: 39
	internal class TreeViewUsbItem
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000107 RID: 263 RVA: 0x0000DA62 File Offset: 0x0000BC62
		// (set) Token: 0x06000108 RID: 264 RVA: 0x0000DA6A File Offset: 0x0000BC6A
		public string Name { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000DA73 File Offset: 0x0000BC73
		// (set) Token: 0x0600010A RID: 266 RVA: 0x0000DA7B File Offset: 0x0000BC7B
		public object Data { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000DA84 File Offset: 0x0000BC84
		// (set) Token: 0x0600010C RID: 268 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public List<TreeViewUsbItem> Children { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000DA98 File Offset: 0x0000BC98
		public static List<TreeViewUsbItem> AllUsbDevices
		{
			get
			{
				TreeViewUsbItem.ConnectedHubs = 0;
				TreeViewUsbItem.ConnectedDevices = 0;
				TreeViewUsbItem Root = new TreeViewUsbItem();
				Root.Name = "Computer";
				Root.Data = "Machine Name:" + Environment.MachineName;
				HostControllerInfo[] HostControllersCollection = USB.AllHostControllers;
				if (HostControllersCollection != null)
				{
					List<TreeViewUsbItem> HCNodeCollection = new List<TreeViewUsbItem>(HostControllersCollection.Length);
					foreach (HostControllerInfo item in HostControllersCollection)
					{
						TreeViewUsbItem HCNode = new TreeViewUsbItem();
						HCNode.Name = item.Name;
						HCNode.Data = item;
						string RootHubPath = USB.GetUsbRootHubPath(item.PNPDeviceID);
						HCNode.Children = TreeViewUsbItem.AddHubNode(RootHubPath, "RootHub");
						HCNodeCollection.Add(HCNode);
					}
					Root.Children = HCNodeCollection;
				}
				List<TreeViewUsbItem> list = new List<TreeViewUsbItem>(1);
				list.Add(Root);
				return list;
			}
		}

		// Token: 0x0600010E RID: 270 RVA: 0x0000DB78 File Offset: 0x0000BD78
		private static List<TreeViewUsbItem> AddHubNode(string HubPath, string HubNodeName)
		{
			UsbNodeInformation[] NodeInfoCollection = USB.GetUsbNodeInformation(HubPath);
			if (NodeInfoCollection != null)
			{
				TreeViewUsbItem HubNode = new TreeViewUsbItem();
				if (string.IsNullOrEmpty(NodeInfoCollection[0].Name))
				{
					HubNode.Name = HubNodeName;
				}
				else
				{
					HubNode.Name = NodeInfoCollection[0].Name;
				}
				HubNode.Data = NodeInfoCollection[0];
				if (NodeInfoCollection[0].NodeType == USB_HUB_NODE.UsbHub)
				{
					HubNode.Children = TreeViewUsbItem.AddPortNode(HubPath, NodeInfoCollection[0].NumberOfPorts);
				}
				else
				{
					HubNode.Children = null;
				}
				List<TreeViewUsbItem> list = new List<TreeViewUsbItem>(1);
				list.Add(HubNode);
				return list;
			}
			return null;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000DC20 File Offset: 0x0000BE20
		private static List<TreeViewUsbItem> AddPortNode(string HubPath, int NumberOfPorts)
		{
			UsbNodeConnectionInformation[] NodeConnectionInfoCollection = USB.GetUsbNodeConnectionInformation(HubPath, NumberOfPorts);
			if (NodeConnectionInfoCollection != null)
			{
				List<TreeViewUsbItem> PortNodeCollection = new List<TreeViewUsbItem>(NumberOfPorts);
				foreach (UsbNodeConnectionInformation NodeConnectionInfo in NodeConnectionInfoCollection)
				{
					TreeViewUsbItem PortNode = new TreeViewUsbItem();
					PortNode.Name = string.Concat(new object[]
					{
						"[Port",
						NodeConnectionInfo.ConnectionIndex,
						"]",
						NodeConnectionInfo.ConnectionStatus
					});
					PortNode.Data = NodeConnectionInfo;
					PortNode.Children = null;
					if (NodeConnectionInfo.ConnectionStatus == USB_CONNECTION_STATUS.DeviceConnected)
					{
						TreeViewUsbItem.ConnectedDevices++;
						if (!string.IsNullOrEmpty(NodeConnectionInfo.DeviceDescriptor.Product))
						{
							PortNode.Name = PortNode.Name + ": " + NodeConnectionInfo.DeviceDescriptor.Product;
						}
						if (NodeConnectionInfo.DeviceIsHub)
						{
							string ExternalHubPath = USB.GetExternalHubPath(NodeConnectionInfo.DevicePath, NodeConnectionInfo.ConnectionIndex);
							UsbNodeInformation[] NodeInfoCollection = USB.GetUsbNodeInformation(ExternalHubPath);
							if (NodeInfoCollection != null)
							{
								PortNode.Data = new ExternalHubInfo
								{
									NodeInfo = NodeInfoCollection[0],
									NodeConnectionInfo = NodeConnectionInfo
								};
								if (NodeInfoCollection[0].NodeType == USB_HUB_NODE.UsbHub)
								{
									PortNode.Children = TreeViewUsbItem.AddPortNode(ExternalHubPath, NodeInfoCollection[0].NumberOfPorts);
									foreach (TreeViewUsbItem item in PortNode.Children)
									{
										try
										{
											if (item != null && item.Data != null)
											{
												UsbNodeConnectionInformation info = (UsbNodeConnectionInformation)item.Data;
												int connectionIndex = NodeConnectionInfo.ConnectionIndex;
												info.ConnectionIndex = Convert.ToInt32(connectionIndex.ToString() + info.ConnectionIndex.ToString());
												item.Data = info;
												item.Name = string.Concat(new object[]
												{
													"[Port",
													info.ConnectionIndex,
													"]",
													NodeConnectionInfo.ConnectionStatus
												});
											}
										}
										catch (Exception err)
										{
											Log.w(err.Message + ":" + err.StackTrace);
										}
									}
								}
								if (string.IsNullOrEmpty(NodeConnectionInfo.DeviceDescriptor.Product) && !string.IsNullOrEmpty(NodeInfoCollection[0].Name))
								{
									PortNode.Name = PortNode.Name + ": " + NodeInfoCollection[0].Name;
								}
							}
							TreeViewUsbItem.ConnectedHubs++;
						}
					}
					PortNodeCollection.Add(PortNode);
				}
				return PortNodeCollection;
			}
			return null;
		}

		// Token: 0x040000BD RID: 189
		public static int ConnectedHubs;

		// Token: 0x040000BE RID: 190
		public static int ConnectedDevices;
	}
}

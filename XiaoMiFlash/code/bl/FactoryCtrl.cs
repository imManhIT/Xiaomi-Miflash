using System;
using System.IO;
using System.Reflection;
using PUB_TEST_FUNC_DLL;
using XiaoMiFlash.code.data;
using XiaoMiFlash.code.module;
using XiaoMiFlash.code.Utility;

namespace XiaoMiFlash.code.bl
{
	// Token: 0x02000060 RID: 96
	public class FactoryCtrl
	{
		// Token: 0x060001FC RID: 508 RVA: 0x00012D1C File Offset: 0x00010F1C
		public static bool SetFactory(string factory)
		{
			bool result = false;
			string dllPath = string.Empty;
			string domain = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			if (factory != null)
			{
				if (!(factory == "Foxconn"))
				{
					if (factory == "Inventec")
					{
						dllPath = domain + FactoryCtrl.arrAssemblyFile[1];
					}
				}
				else
				{
					dllPath = domain + FactoryCtrl.arrAssemblyFile[0];
				}
			}
			try
			{
				if (File.Exists(dllPath))
				{
					string doaminDll = string.Format("{0}\\PUB_TEST_FUNC_DLL.dll", domain);
					if (File.Exists(doaminDll))
					{
						File.Delete(doaminDll);
					}
					File.Copy(dllPath, doaminDll, true);
					result = true;
					Log.w("Factory:" + factory + " switch dll " + doaminDll);
				}
				else
				{
					Log.w(string.Format("file not exit {0}", dllPath));
				}
			}
			catch (Exception ex)
			{
				Log.w(ex.Message);
			}
			return result;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00012DFC File Offset: 0x00010FFC
		public static FlashResult SetFlashResultD(string deivce, bool reuslt)
		{
			Log.w(deivce, "start SetFlashResultD");
			FlashResult flashResult = new FlashResult();
			flashResult.Result = false;
			flashResult.Msg = "upload result failed";
			try
			{
				if (string.IsNullOrEmpty(deivce))
				{
					flashResult.Result = false;
					flashResult.Msg = "device is null";
					return flashResult;
				}
				Log.w(deivce, "upload flash result");
				TPUB_TEST_FUNC_DLL tfd = FactoryCtrl.GetFactoryObject(deivce);
				if (tfd == null)
				{
					flashResult.Result = false;
					flashResult.Msg = "error:couldn't get TPUB_TEST_FUNC_DLL";
				}
				else
				{
					string msg = "";
					bool fResult = tfd.SaveDataByCPUID(deivce, reuslt, out msg);
					msg = string.Format("SaveDataByCPUID result {0} status {1}", fResult.ToString(), msg);
					flashResult.Result = fResult;
					flashResult.Msg = msg;
					Log.w(deivce, msg);
					if (fResult)
					{
						Log.w(deivce, "upload result success");
					}
					else
					{
						Log.w(deivce, "upload result failed");
					}
				}
			}
			catch (Exception ex)
			{
				Log.w(string.Concat(new string[]
				{
					deivce,
					" ",
					ex.Message,
					"  ",
					ex.StackTrace
				}));
				FlashResult flashResult2 = flashResult;
				flashResult2.Msg = flashResult2.Msg + " " + ex.Message;
			}
			Log.w(deivce, flashResult.Msg);
			return flashResult;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00012F50 File Offset: 0x00011150
		public static CheckCPUIDResult GetSearchPathD(string deivce, string swPath)
		{
			CheckCPUIDResult resuktArg = new CheckCPUIDResult();
			Log.w(deivce, "GetSearchPath");
			bool result = false;
			string path = "";
			string msg = "";
			try
			{
				TPUB_TEST_FUNC_DLL tfd = FactoryCtrl.GetFactoryObject(deivce);
				if (tfd == null)
				{
					msg = "can not GetFactoryObject";
					Log.w(deivce + " CheckCPUID failed!");
				}
				else
				{
					result = tfd.CheckCPUID(deivce, out path, out msg);
					foreach (Device item in FlashingDevice.flashDeviceList)
					{
						if (item.Name == deivce)
						{
							item.CheckCPUID = result;
						}
					}
					Log.w(string.Format("{0} CheckCPUID result {1} status {2} imgPath {3}", new object[]
					{
						deivce,
						result.ToString(),
						msg,
						path
					}));
				}
				resuktArg.Msg = msg;
			}
			catch (Exception ex)
			{
				resuktArg.Msg = ex.Message;
			}
			resuktArg.Device = deivce;
			resuktArg.Result = result;
			resuktArg.Path = path;
			Log.w(deivce, string.Format("device {0} CheckCPUID result {1} status {2}", deivce, resuktArg.Result.ToString(), resuktArg.Msg));
			return resuktArg;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0001309C File Offset: 0x0001129C
		public static string GetSearchPath(string deivce, string swPath)
		{
			Log.w(deivce, "start GetSearchPath");
			string searchPath = "";
			try
			{
				object factoryInstance;
				Type factoryObject = FactoryCtrl.GetFactoryObject(deivce, out factoryInstance);
				if (factoryObject == null || factoryInstance == null)
				{
					return "";
				}
				string oMsg = "";
				ParameterModifier pm;
				pm..ctor(3);
				pm[0] = false;
				pm[1] = true;
				pm[2] = true;
				ParameterModifier[] pms = new ParameterModifier[]
				{
					pm
				};
				object[] args = new object[]
				{
					deivce,
					swPath,
					oMsg
				};
				bool bRet = (bool)factoryObject.InvokeMember("CheckCPUID", 256, null, factoryInstance, args, pms, null, null);
				oMsg = args[2].ToString();
				Log.w(deivce, string.Format("CheckCPUID result {0} status {1}", bRet.ToString(), oMsg));
				if (bRet)
				{
					searchPath = args[1].ToString();
				}
			}
			catch (Exception ex)
			{
				Log.w(deivce, ex.StackTrace);
				Log.w(deivce, ex.InnerException.StackTrace);
			}
			return searchPath;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x000131C4 File Offset: 0x000113C4
		public static TPUB_TEST_FUNC_DLL GetFactoryObject(string deivce)
		{
			TPUB_TEST_FUNC_DLL tfd = new TPUB_TEST_FUNC_DLL();
			string msg = string.Empty;
			bool result = tfd.InitCheck(out msg);
			Log.w(string.Format("{0} InitCheck result {1} status {2}", deivce, result.ToString(), msg));
			if (result)
			{
				return tfd;
			}
			FlashingDevice.UpdateDeviceStatus(deivce, default(float?), "can't init factory ev", "factory ev error", true);
			return null;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00013220 File Offset: 0x00011420
		public static Type GetFactoryObject(string deivce, out object factoryInstance)
		{
			Log.w(deivce, "start GetFactoryObject");
			Type factoryObject = null;
			factoryInstance = null;
			if (FactoryCtrl.firstCall)
			{
				FactoryCtrl.firstCall = false;
				for (int i = 0; i < FactoryCtrl.arrAssemblyFile.Length; i++)
				{
					try
					{
						Log.w(deivce, FactoryCtrl.arrAssemblyFile[i]);
						Assembly asm = Assembly.LoadFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FactoryCtrl.arrAssemblyFile[i]);
						factoryObject = asm.GetType("PUB_TEST_FUNC_DLL.TPUB_TEST_FUNC_DLL");
						factoryInstance = Activator.CreateInstance(factoryObject);
						string msg = "";
						ParameterModifier pm;
						pm..ctor(1);
						pm[0] = true;
						ParameterModifier[] pms = new ParameterModifier[]
						{
							pm
						};
						object[] args = new object[]
						{
							msg
						};
						bool bRet = (bool)factoryObject.InvokeMember("InitCheck", 256, null, factoryInstance, args, pms, null, null);
						msg = args[0].ToString();
						Log.w(deivce, string.Format("InitCheck result {0} status {1}", bRet.ToString(), msg));
						if (bRet)
						{
							break;
						}
					}
					catch (Exception ex)
					{
						factoryObject = null;
						factoryInstance = null;
						Log.w(deivce, ex.StackTrace);
						Log.w(deivce, ex.InnerException.StackTrace);
					}
				}
			}
			return factoryObject;
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0001336C File Offset: 0x0001156C
		public static Type GetFactoryObject2(out object factoryInstance)
		{
			Type factoryObject = null;
			factoryInstance = null;
			if (FactoryCtrl.firstCall)
			{
				FactoryCtrl.firstCall = false;
				try
				{
					for (int i = 0; i < FactoryCtrl.arrAssemblyFile.Length; i++)
					{
						Assembly asm = Assembly.LoadFile(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + FactoryCtrl.arrAssemblyFile[i]);
						factoryObject = asm.GetType("TestDll.Test");
						factoryInstance = Activator.CreateInstance(factoryObject);
						string aCPUID = "AAAAAA";
						bool aPass = true;
						string oMsg = "";
						ParameterModifier pm;
						pm..ctor(3);
						pm[0] = false;
						pm[1] = false;
						pm[2] = true;
						ParameterModifier[] pms = new ParameterModifier[]
						{
							pm
						};
						object[] args = new object[]
						{
							aCPUID,
							aPass,
							oMsg
						};
						bool bRet = (bool)factoryObject.InvokeMember("TestPa", 256, null, factoryInstance, args, pms, null, null);
						oMsg = args[2].ToString();
						if (bRet)
						{
							break;
						}
					}
				}
				catch (Exception ex)
				{
					Log.w(ex.Message);
				}
			}
			return factoryObject;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0001349C File Offset: 0x0001169C
		public static void Test()
		{
			object factoryInstance;
			Type factoryObject = FactoryCtrl.GetFactoryObject2(out factoryInstance);
			if (factoryObject != null)
			{
			}
		}

		// Token: 0x040001DA RID: 474
		private static string[] arrAssemblyFile = new string[]
		{
			"Source\\ThirdParty\\Foxconn\\PUB_TEST_FUNC_DLL.dll",
			"Source\\ThirdParty\\Inventec\\PUB_TEST_FUNC_DLL.dll"
		};

		// Token: 0x040001DB RID: 475
		private static string[] arrTypeName = new string[]
		{
			"PUB_TEST_FUNC_DLL.TPUB_TEST_FUNC_DLL",
			"PUB_TEST_FUNC_DLL.TPUB_TEST_FUNC_DLL"
		};

		// Token: 0x040001DC RID: 476
		private static bool firstCall = true;
	}
}

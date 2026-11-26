using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace orangesGame
{
	/// <summary>
	/// 平台原生桥（原 Android 实现：Adjust 埋点、渠道参数、震动、Google 支付、签名校验）。
	/// 项目广告与平台已统一为抖音小游戏（WebGL）：
	/// 非 Android 平台下这些方法全部空转，既避免 AndroidJavaClass 在 WebGL 运行时抛异常，
	/// 也把无用代码从包里剔除。以后若要重新出 Android 包，把 UNITY_ANDROID 分支写回来即可。
	/// </summary>
	public class SdkUtils : Singleton<SdkUtils>
	{
		private string _gaidTemp = string.Empty;

		public string[] GetAdParam()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return GetAdParamAndroid();
#else
			return new string[0];
#endif
		}

		public bool GetAdTest()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return GetAdTestAndroid();
#else
			return false;
#endif
		}

		public bool GetAdDebug()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return GetIsAdDebug();
#else
			return false;
#endif
		}

		public bool GetUmTest()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return GetUmTestAndroid();
#else
			return false;
#endif
		}

		public bool GetUmDebug()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return GetUmDebugAndroid();
#else
			return false;
#endif
		}

		public bool IsABTestKey(string key, string value)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return IsABTestAndroid(key, value);
#else
			return false;
#endif
		}

		public string GetABTestKey(string key)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			return GetABTestAndroid(key);
#else
			return string.Empty;
#endif
		}

		public void ShowVibrator(long[] arr)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			mainActivity.Call<bool>("showVibrator", new object[1]
			{
				arr
			});
#endif
		}

		public void Shake(int shakeFlag)
		{
			if (shakeFlag != 0)
			{
				return;
			}
#if UNITY_ANDROID && !UNITY_EDITOR
			// 原生 Android 包（以后若重新出包）走系统震动
			Handheld.Vibrate();
#elif UNITY_WEBGL && !UNITY_EDITOR
			// 抖音小游戏：UnityEngine.Handheld 在 WebGL 下不存在（编译报 CS0103），
			// 改用 TTSDK 震动接口。WebGL 只支持短/长两种，<1000ms 视为短震动，repeat 无效。
			try
			{
				TTSDK.TT.Vibrate(new long[] { 100L }, -1);
			}
			catch (System.Exception)
			{
				// 部分机型/低版本宿主不支持震动，忽略即可
			}
#endif
		}

		public void ShowVibratorNoDelay(long arr)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			mainActivity.Call<bool>("showVibratorNoDelay", new object[1]
			{
				arr
			});
#endif
		}

		public void CancelVibrator()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			mainActivity.Call<bool>("cancelVibrator", new object[0]);
#endif
		}

		public void ShareFB(string title, string context, string param)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.common.core.CommonCoreUtil");
			androidJavaClass.CallStatic("startShareFB", title, context, param);
#endif
			// 抖音小游戏的分享走 AdManager.ShareGame
		}

		public void ShareMessenger(string title, string context, string param)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.common.core.CommonCoreUtil");
			androidJavaClass.CallStatic("startShareMessenger", title, context, param);
#endif
		}

		public void GooglePay(string orderId, string itemType = "inapp")
		{
			// 抖音小游戏没有 Google Play / AppStore 内购，原 SDKBOX PurchaseHandler 已移除
			UnityEngine.Debug.Log("[SDK] 当前平台无内购能力，忽略订单：" + orderId);
		}

		public void QueryInventoryAsync()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			if (GameGlobalData.GoogleSkuIdDic.Count > 0)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.common.core.CommonCoreUtil");
				androidJavaClass.CallStatic("googleQueryInventoryAsync", "QueryInventoryAsync", GameGlobalData.GoogleSkuIdDic.Values.ToArray());
			}
#endif
		}

		public string[] GetAdParamAndroid()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<string[]>("getAdParam", new object[0]);
#else
			return new string[0];
#endif
		}

		public bool GetIsAdDebug()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<bool>("getIsAdDebug", new object[0]);
#else
			return false;
#endif
		}

		private bool GetAdTestAndroid()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<bool>("getIsAdTest", new object[0]);
#else
			return false;
#endif
		}

		private bool GetUmTestAndroid()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<bool>("getIsUmTest", new object[0]);
#else
			return false;
#endif
		}

		private bool GetUmDebugAndroid()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<bool>("getIsUmDebug", new object[0]);
#else
			return false;
#endif
		}

		private bool IsABTestAndroid(string key, string value)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<bool>("isABTestKey", new object[2]
			{
				key,
				value
			});
#else
			return false;
#endif
		}

		private string GetABTestAndroid(string key)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<string>("getABTestValue", new object[1]
			{
				key
			});
#else
			return string.Empty;
#endif
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		private AndroidJavaObject GetMainActivity()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			return androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}

		private AndroidJavaObject GetAndroidClass(string package, string className)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass(package + "." + className);
			return androidJavaClass.GetStatic<AndroidJavaObject>(className);
		}
#endif

		public void AdjustTrackEvent(string eventName)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			mainActivity.Call("adjustEvent", eventName);
#endif
		}

		public void AdjustTrackEventParam(string eventName, Dictionary<string, string> dic)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			AndroidJavaObject mainActivity = GetMainActivity();
			mainActivity.Call("adjustEventParam", eventName, list.ToArray(), list2.ToArray());
#endif
		}

		public bool IsCorrect()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			string text = "15:0A:F2:21:E3:68:3A:89:C3:E9:4B:87:8B:5F:41:1F:D6:66:5B:B0:";
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.common.core.CommonCoreUtil");
			return androidJavaClass.CallStatic<bool>("callSignatures", new object[1]
			{
				text
			});
#else
			// 非 Android 平台没有原生签名校验，视为通过
			return true;
#endif
		}

		public float GetMemory()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.common.core.CommonCoreUtil");
			return androidJavaClass.CallStatic<float>("getMemory", new object[0]);
#else
			return 0f;
#endif
		}

		public string GetGaID()
		{
			// 抖音小游戏没有 Google 广告 ID
			return string.Empty;
		}

		public void SetReloadData(string str)
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			mainActivity.Call<bool>("SetReloadData", new object[1]
			{
				str
			});
#endif
		}

		public string GetReloadData()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<string>("GetReloadData", new object[0]);
#else
			return string.Empty;
#endif
		}

		public bool HasOldApp()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			AndroidJavaObject mainActivity = GetMainActivity();
			return mainActivity.Call<bool>("HasOldApp", new object[0]);
#else
			return false;
#endif
		}
	}
}

using UnityEngine;

namespace Umeng
{
	public class DplusAgent
	{
		private static AndroidJavaObject _DplusAgent = new AndroidJavaClass("com.umeng.analytics.dplus.UMADplus");

		private static AndroidJavaObject Context = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

		private static AndroidJavaClass UnityUtil = new AndroidJavaClass("com.umeng.analytics.UnityUtil");

		public static void track(string eventName)
		{
			_DplusAgent.CallStatic("track", Context, eventName);
		}

		public static void track(string eventName, JSONObject jsonObject)
		{
			UnityUtil.CallStatic("track", Context, eventName, jsonObject.ToString());
		}

		public static void registerSuperProperty(JSONObject jsonObject)
		{
			UnityUtil.CallStatic("registerSuperPropertyAll", Context, jsonObject.ToString());
		}

		public static void unregisterSuperProperty(string propertyName)
		{
			_DplusAgent.CallStatic("unregisterSuperProperty", Context, propertyName);
		}

		public static JSONNode getSuperProperty(string propertyName)
		{
			string aJSON = UnityUtil.CallStatic<string>("getSuperProperty", new object[2]
			{
				Context,
				propertyName
			});
			return JSON.Parse(aJSON)["__umeng_internal_data_"];
		}

		public static JSONObject getSuperProperties()
		{
			string aJSON = _DplusAgent.CallStatic<string>("getSuperProperties", new object[1]
			{
				Context
			});
			return (JSONObject)JSON.Parse(aJSON);
		}

		public static void clearSuperProperties()
		{
			_DplusAgent.CallStatic("clearSuperProperties", Context);
		}

		public static void setFirstLaunchEvent(string[] trackID)
		{
			UnityUtil.CallStatic("setFirstLaunchEvent", Context, string.Join(";=umengUnity=;", trackID));
		}
	}
}

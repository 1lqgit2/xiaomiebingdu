using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class FireBaseSdk
	{
		public bool IsDebug;

		public FireBaseSdk(bool isDebug)
		{
			if (isDebug)
			{
				UnityEngine.Debug.Log("untiy firebase init");
			}
			SetDebugLog(isDebug);
		}

		public void SetDebugLog(bool islog)
		{
			IsDebug = islog;
		}

		public void OnEvent(string eventName, Dictionary<string, string> dic)
		{
			List<string> keys = new List<string>();
			List<string> values = new List<string>();
			ParseArray(dic, ref keys, ref values);
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			@static.Call("fireBaseOnEvent", eventName, keys.ToArray(), values.ToArray());
		}

		public static AndroidJavaObject ParseMap(Dictionary<string, string> dic)
		{
			if (dic == null)
			{
				return null;
			}
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			foreach (KeyValuePair<string, string> item in dic)
			{
				androidJavaObject.Call<string>("put", new object[2]
				{
					item.Key,
					item.Value
				});
			}
			return androidJavaObject;
		}

		public static void ParseArray(Dictionary<string, string> dic, ref List<string> keys, ref List<string> values)
		{
			foreach (KeyValuePair<string, string> item in dic)
			{
				keys.Add(item.Key);
				values.Add(item.Value);
			}
		}
	}
}

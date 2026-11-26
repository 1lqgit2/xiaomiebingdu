using System;
using System.Collections.Generic;
using UnityEngine;

namespace Umeng
{
	public class Analytics
	{
		private static class SingletonHolder
		{
			public static AndroidJavaClass instance_mobclick;

			public static AndroidJavaObject instance_context;

			static SingletonHolder()
			{
				instance_mobclick = new AndroidJavaClass("com.umeng.analytics.game.UMGameAgent");
				using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
				{
					instance_context = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				}
			}
		}

		private static bool hasInit;

		private static string _AppKey;

		private static string _ChannelId;

		private static AndroidJavaClass _UpdateAgent;

		public static string AppKey => _AppKey;

		public static string ChannelId => _ChannelId;

		protected static AndroidJavaClass Agent => SingletonHolder.instance_mobclick;

		protected static AndroidJavaClass UpdateAgent
		{
			get
			{
				if (_UpdateAgent == null)
				{
					_UpdateAgent = new AndroidJavaClass("com.umeng.update.UmengUpdateAgent");
				}
				return _UpdateAgent;
			}
		}

		protected static AndroidJavaObject Context => SingletonHolder.instance_context;

		public static void Start()
		{
			UMGameAgentInit();
			if (!hasInit)
			{
				onResume();
				CreateUmengManger();
				hasInit = true;
			}
		}

		public static void SetLogEnabled(bool value)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.commonsdk.UMConfigure");
			androidJavaClass.CallStatic("setLogEnabled", value);
		}

		public static void Event(string eventId)
		{
			Agent.CallStatic("onEvent", Context, eventId);
		}

		public static void Event(string eventId, string label)
		{
			Agent.CallStatic("onEvent", Context, eventId, label);
		}

		public static void Event(string eventId, Dictionary<string, string> attributes)
		{
			Agent.CallStatic("onEvent", Context, eventId, ToJavaHashMap(attributes));
		}

		public static void EventBegin(string eventId)
		{
			Agent.CallStatic("onEventBegin", Context, eventId);
		}

		public static void EventEnd(string eventId)
		{
			Agent.CallStatic("onEventEnd", Context, eventId);
		}

		public static void EventBegin(string eventId, string label)
		{
			Agent.CallStatic("onEventBegin", Context, eventId, label);
		}

		public static void EventEnd(string eventId, string label)
		{
			Agent.CallStatic("onEventEnd", Context, eventId, label);
		}

		public static void EventBeginWithPrimarykeyAndAttributes(string eventId, string primaryKey, Dictionary<string, string> attributes)
		{
			Agent.CallStatic("onKVEventBegin", Context, eventId, ToJavaHashMap(attributes), primaryKey);
		}

		public static void EventEndWithPrimarykey(string eventId, string primaryKey)
		{
			Agent.CallStatic("onKVEventEnd", Context, eventId, primaryKey);
		}

		public static void EventDuration(string eventId, int milliseconds)
		{
			Agent.CallStatic("onEventDuration", Context, eventId, (long)milliseconds);
		}

		public static void EventDuration(string eventId, string label, int milliseconds)
		{
			Agent.CallStatic("onEventDuration", Context, eventId, label, (long)milliseconds);
		}

		public static void EventDuration(string eventId, Dictionary<string, string> attributes, int milliseconds)
		{
			Agent.CallStatic("onEventDuration", Context, eventId, ToJavaHashMap(attributes), (long)milliseconds);
		}

		public static void PageBegin(string pageName)
		{
			Agent.CallStatic("onPageStart", pageName);
		}

		public static void PageEnd(string pageName)
		{
			Agent.CallStatic("onPageEnd", pageName);
		}

		public static void Event(string eventId, Dictionary<string, string> attributes, int value)
		{
			try
			{
				if (attributes == null)
				{
					attributes = new Dictionary<string, string>();
				}
				if (attributes.ContainsKey("__ct__"))
				{
					attributes["__ct__"] = value.ToString();
					Event(eventId, attributes);
				}
				else
				{
					attributes.Add("__ct__", value.ToString());
					Event(eventId, attributes);
					attributes.Remove("__ct__");
				}
			}
			catch (Exception)
			{
			}
		}

		public static string GetDeviceInfo()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
			return androidJavaClass.CallStatic<string>("getDeviceInfo", new object[1]
			{
				Context
			});
		}

		public static void SetLogEncryptEnabled(bool value)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.commonsdk.UMConfigure");
			androidJavaClass.CallStatic("setEncryptEnabled", value);
		}

		public static void SetLatency(int value)
		{
			Agent.CallStatic("setLatencyWindow", (long)value);
		}

		public static void SetContinueSessionMillis(long milliseconds)
		{
			Agent.CallStatic("setSessionContinueMillis", milliseconds);
		}

		[Obsolete("Flush")]
		public static void Flush()
		{
			Agent.CallStatic("flush", Context);
		}

		[Obsolete("SetEnableLocation已弃用")]
		public static void SetEnableLocation(bool reportLocation)
		{
			Agent.CallStatic("setAutoLocation", reportLocation);
		}

		public static void EnableActivityDurationTrack(bool isTraceActivity)
		{
			Agent.CallStatic("openActivityDurationTrack", isTraceActivity);
		}

		public static void SetCheckDevice(bool value)
		{
			Agent.CallStatic("setCheckDevice", value);
		}

		private static void CreateUmengManger()
		{
			GameObject gameObject = new GameObject();
			gameObject.AddComponent<UmengManager>();
			gameObject.name = "UmengManager";
		}

		public static void onResume()
		{
			Agent.CallStatic("onResume", Context);
		}

		public static void onPause()
		{
			Agent.CallStatic("onPause", Context);
		}

		public static void onKillProcess()
		{
			Agent.CallStatic("onKillProcess", Context);
		}

		private static AndroidJavaObject ToJavaHashMap(Dictionary<string, string> dic)
		{
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("java.util.HashMap");
			IntPtr methodID = AndroidJNIHelper.GetMethodID(androidJavaObject.GetRawClass(), "put", "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
			object[] array = new object[2];
			foreach (KeyValuePair<string, string> item in dic)
			{
				using (AndroidJavaObject androidJavaObject2 = new AndroidJavaObject("java.lang.String", item.Key))
				{
					using (AndroidJavaObject androidJavaObject3 = new AndroidJavaObject("java.lang.String", item.Value))
					{
						array[0] = androidJavaObject2;
						array[1] = androidJavaObject3;
						AndroidJNI.CallObjectMethod(androidJavaObject.GetRawObject(), methodID, AndroidJNIHelper.CreateJNIArgArray(array));
					}
				}
			}
			return androidJavaObject;
		}

		public static void UMGameAgentInit()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.umeng.analytics.UnityUtil");
			androidJavaClass.CallStatic("initUnity", Context, string.Empty, string.Empty);
		}

		public void Dispose()
		{
			Agent.Dispose();
			Context.Dispose();
		}
	}
}

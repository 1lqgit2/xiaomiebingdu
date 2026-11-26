using System.Collections.Generic;

namespace orangesGame
{
	public class AnalyzeSdk : Singleton<AnalyzeSdk>
	{
		public bool IsTestUm;

		public bool IsDebugUm;

		private UmSdk _umSdk;

		private FireBaseSdk _fireBaseSdk;

		private char _splitChar;

		private AnalyzeSdk()
		{
		}

		public void Init()
		{
			if (!IsTestUm)
			{
				_umSdk = new UmSdk(IsDebugUm);
				_fireBaseSdk = new FireBaseSdk(IsDebugUm);
				_splitChar = '@';
			}
		}

		public void SetUmDebug(bool isDebug)
		{
			IsDebugUm = isDebug;
		}

		public void FireBaseOnEvent(string eventName, Dictionary<string, string> dic)
		{
			if (!IsTestUm)
			{
				//_fireBaseSdk.OnEvent(eventName, dic);
			}
		}

		public void OnEvent(string eventID)
		{
			if (!IsTestUm)
			{
				//_umSdk.OnEvent(eventID);
			}
		}

		public void OnEventByLabel(string eventID, string label)
		{
			if (!IsTestUm)
			{
				//_umSdk.OnEvent(eventID, label);
			}
		}

		public void OnEventByArgs(string eventID, string[] args)
		{
			if (!IsTestUm)
			{
				//_umSdk.OnEvent(eventID, args, _splitChar);
			}
		}

		public void OnEventValue(string eventID, string[] args, int value)
		{
			if (!IsTestUm)
			{
				//_umSdk.OnEventValue(eventID, args, _splitChar, value);
			}
		}

		public void StartLevel(string levelID)
		{
			if (!IsTestUm)
			{
				//_umSdk.StartLevel(levelID);
			}
		}

		public void FinishLevel(string levelID)
		{
			if (!IsTestUm)
			{
				//_umSdk.FinishLevel(levelID);
			}
		}

		public void FailLevel(string levelID)
		{
			if (!IsTestUm)
			{
				//_umSdk.FailLevel(levelID);
			}
		}

		public string GetABTestKey(string key)
		{
            return "";
			if (!IsTestUm)
			{
				return Singleton<SdkUtils>.Instance.GetABTestKey(key);
			}
			return "1";
		}
	}
}

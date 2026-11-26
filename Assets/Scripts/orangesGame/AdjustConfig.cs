using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	internal class AdjustConfig : Singleton<AdjustConfig>
	{
		private string lv5 = "8bbsjs";

		private string lv25 = "2a8kr0";

		private string dailyTask2 = "dwbiy1";

		private string showVideo3 = "jx74lm";

		private string showVideo5 = "r63388";

		private string showVideo2 = "lrx8q9";

		private long CoolTime = 86400000L;

		private long AdjustFirstDayTime;

		private string Key = "AdjustConfig_";

		private Dictionary<string, int> data = new Dictionary<string, int>();

		public void InitKeys()
		{
			data[lv5] = PlayerPrefs.GetInt("AdjustConfig_" + lv5, 0);
			data[lv25] = PlayerPrefs.GetInt("AdjustConfig_" + lv25, 0);
			data[dailyTask2] = PlayerPrefs.GetInt("AdjustConfig_" + dailyTask2, 0);
			data[showVideo3] = PlayerPrefs.GetInt("AdjustConfig_" + showVideo3, 0);
			data[showVideo5] = PlayerPrefs.GetInt("AdjustConfig_" + showVideo5, 0);
			data[showVideo2] = PlayerPrefs.GetInt("AdjustConfig_" + showVideo2, 0);
		}

		public void SetKeys(string key)
		{
			data[key] = 1;
			PlayerPrefs.SetInt("AdjustConfig_" + key, 1);
		}

		public void InitFirstDayTime()
		{
			long num = AdjustFirstDayTime = long.Parse(PlayerPrefs.GetString("AdjustFirstDayTime", "0"));
			if (AdjustFirstDayTime == 0)
			{
				SetAdjustFirstDayTime(GameGlobalData.GetTimestamp());
			}
		}

		public void SetAdjustFirstDayTime(long time)
		{
			AdjustFirstDayTime = time;
			PlayerPrefs.SetString("AdjustFirstDayTime", AdjustFirstDayTime.ToString());
		}

		private void SendAndTrackLog(string name)
		{
			long num = GameGlobalData.GetTimestamp() - AdjustFirstDayTime;
			if (num > CoolTime)
			{
				return;
			}
			if (data.ContainsKey(name))
			{
				int num2 = data[name];
				if (num2 == 1)
				{
					return;
				}
			}
			Singleton<AdjustSdk>.Instance.TrackEvent(name);
			SetKeys(name);
		}

		public void FinishLvTrack()
		{
			if (GameGlobalData.CurLevel == 5)
			{
				SendAndTrackLog(lv5);
			}
			if (GameGlobalData.CurLevel == 25)
			{
				SendAndTrackLog(lv25);
			}
		}

		public void FinishVideoTrack()
		{
			if (GameGlobalData.ShowVideoTimes == 2)
			{
				SendAndTrackLog(showVideo2);
			}
			if (GameGlobalData.ShowVideoTimes == 3)
			{
				SendAndTrackLog(showVideo3);
			}
			if (GameGlobalData.ShowVideoTimes == 5)
			{
				SendAndTrackLog(showVideo5);
			}
		}

		public void FinishMission()
		{
			SendAndTrackLog(dailyTask2);
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class ShopPrefsData
	{
		private Dictionary<string, int> _timeLimitGiftState;

		private Dictionary<string, string> _timeLimitGiftTime;

		private Dictionary<string, int> _timeLimitGiftViewState;

		public int _noAdsState;

		private List<TimeGiftStruct> _timeGiftJson = new List<TimeGiftStruct>();

		public bool isNoAds => _noAdsState != 0;

		public ShopPrefsData()
		{
			_timeLimitGiftState = new Dictionary<string, int>();
			_timeLimitGiftTime = new Dictionary<string, string>();
			_timeLimitGiftViewState = new Dictionary<string, int>();
		}

		public void LoadNoAdsState()
		{
			_noAdsState = PlayerPrefs.GetInt("NoAdsState", _noAdsState);
		}

		public void SaveNoAdsState(bool isNoAdsState)
		{
			_noAdsState = (isNoAdsState ? 1 : 0);
			PlayerPrefs.SetInt("NoAdsState", _noAdsState);
		}

		public void LoadTimeLimitGiftState(TimeGiftStruct[] json)
		{
			int defaultValue = (Application.platform == RuntimePlatform.IPhonePlayer) ? 1 : 0;
			for (int i = 0; i < json.Length; i++)
			{
				string key = "TimeLimitGiftState" + json[i].ID;
				_timeLimitGiftState.Add(key, PlayerPrefs.GetInt(key, defaultValue));
				string key2 = "TimeLimitGiftViewState" + json[i].ID;
				_timeLimitGiftViewState.Add(key2, PlayerPrefs.GetInt(key2, defaultValue));
			}
		}

		public void SaveTimeLimitGiftState(int id, int value)
		{
			string text = "TimeLimitGiftState" + id;
			if (_timeLimitGiftState.ContainsKey(text))
			{
				_timeLimitGiftState[text] = value;
				PlayerPrefs.SetInt(text, value);
			}
			else
			{
				UnityEngine.Debug.LogError("TimeLimitGiftState key is null" + text);
			}
		}

		public void SaveTimeLimitGiftViewState(int id, int value)
		{
			string text = "TimeLimitGiftViewState" + id;
			if (_timeLimitGiftViewState.ContainsKey(text))
			{
				_timeLimitGiftViewState[text] = value;
				PlayerPrefs.SetInt(text, value);
			}
			else
			{
				UnityEngine.Debug.LogError("TimeLimitGiftViewState key is null" + text);
			}
		}

		public int GetTimeLimitGiftViewState(int id)
		{
			string key = "TimeLimitGiftViewState" + id;
			if (_timeLimitGiftViewState.ContainsKey(key))
			{
				return _timeLimitGiftViewState[key];
			}
			return 1;
		}

		public int GetTimeLimitGiftState(int id)
		{
			string key = "TimeLimitGiftState" + id;
			if (_timeLimitGiftState.ContainsKey(key))
			{
				return _timeLimitGiftState[key];
			}
			return 1;
		}

		public List<TimeGiftStruct> CheckGiftJson()
		{
			_timeGiftJson.Clear();
			for (int i = 0; i < Singleton<GameJson>.Instance.TimeGiftJson.Length; i++)
			{
				TimeGiftStruct item = Singleton<GameJson>.Instance.TimeGiftJson[i];
				if (GameGlobalData.ActualLevel > item.UnLockLevel && GameGlobalData.ShopData.GetTimeLimitGiftState(item.ID) != 1)
				{
					long timeLimitGiftTime = GameGlobalData.ShopData.GetTimeLimitGiftTime(item.ID);
					if (timeLimitGiftTime != 0 && !(GameUtils.GetTimeSpan(timeLimitGiftTime).TotalMilliseconds < 0.10000000149011612))
					{
						_timeGiftJson.Add(item);
					}
				}
			}
			return _timeGiftJson;
		}

		public void CheckGiftLevel()
		{
			TimeGiftStruct[] timeGiftJson = Singleton<GameJson>.Instance.TimeGiftJson;
			for (int i = 0; i < timeGiftJson.Length; i++)
			{
				TimeGiftStruct timeGiftStruct = timeGiftJson[i];
				string key = "TimeLimitGiftTime" + timeGiftStruct.ID;
				string @string = PlayerPrefs.GetString(key, "-1");
				if (@string == "-1" && GameGlobalData.ActualLevel > timeGiftStruct.UnLockLevel)
				{
					SaveTimeLimitGiftTime(value: GameUtils.GetTargetTimeTick(timeGiftStruct.Time).Ticks, id: timeGiftStruct.ID);
				}
			}
		}

		public void LoadTimeLimitGiftTime(TimeGiftStruct[] jsons, int curLevel)
		{
			for (int i = 0; i < jsons.Length; i++)
			{
				TimeGiftStruct timeGiftStruct = jsons[i];
				string key = "TimeLimitGiftTime" + timeGiftStruct.ID;
				string @string = PlayerPrefs.GetString(key, "-1");
				if (@string == "-1" && curLevel >= timeGiftStruct.UnLockLevel)
				{
					PlayerPrefs.SetString(key, GameUtils.GetTargetTimeTick(timeGiftStruct.Time).Ticks.ToString());
				}
				_timeLimitGiftTime.Add(key, PlayerPrefs.GetString(key, "-1"));
			}
		}

		public void SaveTimeLimitGiftTime(int id, long value)
		{
			string text = "TimeLimitGiftTime" + id;
			if (_timeLimitGiftTime.ContainsKey(text))
			{
				_timeLimitGiftTime[text] = value.ToString();
				PlayerPrefs.SetString(text, _timeLimitGiftTime[text]);
			}
			else
			{
				UnityEngine.Debug.LogError("TimeLimitGiftTime key is null" + text);
			}
		}

		public long GetTimeLimitGiftTime(int id)
		{
			string key = "TimeLimitGiftTime" + id;
			if (_timeLimitGiftTime.ContainsKey(key))
			{
				return long.Parse(_timeLimitGiftTime[key]);
			}
			return 0L;
		}

		public void TestDebug(TimeGiftStruct[] jsons)
		{
			for (int i = 0; i < jsons.Length; i++)
			{
				string key = "TimeLimitGiftState" + jsons[i].ID;
				string key2 = "TimeLimitGiftTime" + jsons[i].ID;
				UnityEngine.Debug.Log(PlayerPrefs.GetInt(key, 0) + "@@" + PlayerPrefs.GetString(key2, "-1"));
			}
		}
	}
}

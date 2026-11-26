using System;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class DailyTaskHandle : Singleton<DailyTaskHandle>
	{
		public const string KEY_TYPE = "TaskType_";

		public const string KEY_PROGRESS = "TaskProgress_";

		public const string KEY_REWARD = "TaskReward_";

		private List<int> _taskMiddle;

		private List<int> _taskTypes;

		private List<int> _taskRewardStates;

		private List<int> _taskProgress;

		private List<DailyTaskVO> _list;

		private Dictionary<DailyTaskType, DailyTaskVO> _dic;

		private DateTime _refreshTime;

		private const int REFRESH_TIME = 5;

		private DailyTaskHandle()
		{
			_taskMiddle = new List<int>(3);
			_taskTypes = new List<int>();
			_taskProgress = new List<int>();
			_taskRewardStates = new List<int>();
			_list = new List<DailyTaskVO>();
			_dic = new Dictionary<DailyTaskType, DailyTaskVO>();
		}

		public void Init()
		{
			float num = GameGlobalData.TaskTime - DateTime.Now.Ticks;
			if (num <= 0f)
			{
				GameGlobalData.SetTaskRefresh(0);
			}
			if (GameGlobalData.TaskRefresh == 0)
			{
				SetMiddleTaskType();
				RefreshAllTask();
				SaveAllTaskPrefs();
				SetTaskFirePowerCost();
				GetTime();
				GameGlobalData.SetTaskTime(_refreshTime.Ticks);
				GameGlobalData.SetTaskRefresh(1);
			}
			else
			{
				SaveAllTaskPrefs();
				LoadAllTaskPrefs();
				RefreshAllTaskByPrefs();
			}
			ListToDic();
		}

		public void AddProgress(DailyTaskType type, int value)
		{
			if (!_dic.ContainsKey(type) || _dic[type].isFinish)
			{
				return;
			}
			_dic[type].progress += value;
			if (_dic[type].isFinish)
			{
				string body = $"{LanguageConfig.TASK_TITLE[(int)(_dic[type].type - 1)]}   {LanguageConfig.TASK_COMPLETE}";
				ApplicationFacade.Instance.SendNotification("TIP_ANIM", body);
				GameAnalysisData.FinishMission((int)type);
				if (type == DailyTaskType.Revive_3)
				{
					Singleton<AdjustConfig>.Instance.FinishMission();
				}
				_dic[DailyTaskType.FinishTask_5].progress++;
				if (_dic[DailyTaskType.FinishTask_5].isFinish)
				{
					body = $"{LanguageConfig.TASK_TITLE[(int)(_dic[DailyTaskType.FinishTask_5].type - 1)]}   {LanguageConfig.TASK_COMPLETE}";
					ApplicationFacade.Instance.SendNotification("TIP_ANIM", body);
					GameAnalysisData.FinishMission(8);
				}
			}
		}

		public int GetIndex(DailyTaskType type)
		{
			if (_dic.ContainsKey(type))
			{
				return _dic[type].index;
			}
			return -1;
		}

		public bool IsReward(int index)
		{
			return _list[index].isFinish && _list[index].rewardState == 0;
		}

		public void SetRewardState(int index)
		{
			_list[index].rewardState = 1;
			SaveAllTaskPrefs();
		}

		public float GetRewardCoin(int index)
		{
			return GameUtils.UnitToFloat(GameGlobalData.TaskFirePowerCost) * _list[index].rate;
		}

		public bool IsReward()
		{
			for (int i = 0; i < _list.Count; i++)
			{
				if (_list[i].isFinish && _list[i].rewardState == 0)
				{
					return true;
				}
			}
			return false;
		}

		public List<DailyTaskVO> GetList()
		{
			return _list;
		}

		public int GetTime()
		{
			DateTime now = DateTime.Now;
			int hour = now.Hour;
			if (hour >= 0 && hour < 5)
			{
				_refreshTime = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
			}
			else
			{
				now = now.AddDays(1.0);
				_refreshTime = new DateTime(now.Year, now.Month, now.Day, 5, 0, 0);
			}
			return (int)(_refreshTime - DateTime.Now).TotalSeconds;
		}

		public string GetAllTaskProgress()
		{
			string text = string.Empty;
			for (int i = 0; i < _list.Count; i++)
			{
				if (!_list[i].isFinish && _list[i].rewardState == 0)
				{
					text += $"{LanguageConfig.TASK_TITLE[(int)(_list[i].type - 1)]} {_list[i].progress}/{_list[i].progressMax} \n";
				}
			}
			return text;
		}

		private void ListToDic()
		{
			_dic.Clear();
			for (int i = 0; i < _list.Count; i++)
			{
				_dic.Add(_list[i].type, _list[i]);
			}
		}

		private void RefreshAllTask()
		{
			_list.Clear();
			_list.Add(GetTaskVO(DailyTaskType.DailyLogin, 0));
			_list.Add(GetTaskVO(DailyTaskType.Revive_3, 1));
			for (int i = 0; i < _taskMiddle.Count; i++)
			{
				DailyTaskType type = (DailyTaskType)_taskMiddle[i];
				_list.Add(GetTaskVO(type, 2 + i));
			}
			_list.Add(GetTaskVO(DailyTaskType.FinishTask_5, 5));
		}

		private void RefreshAllTaskByPrefs()
		{
			_list.Clear();
			for (int i = 0; i < _taskTypes.Count; i++)
			{
				_list.Add(GetTaskVO((DailyTaskType)_taskTypes[i], i, _taskRewardStates[i], _taskProgress[i]));
			}
		}

		private DailyTaskVO GetTaskVO(DailyTaskType type, int index, int reward = 0, int progress = 0)
		{
			switch (type)
			{
			default:
				return new DailyTaskVO(type, progress, 1, 0.2f, index, reward);
			case DailyTaskType.Revive_3:
				return new DailyTaskVO(type, progress, 3, 0.4f, index, reward);
			case DailyTaskType.CoinReward_3:
				return new DailyTaskVO(type, progress, 3, 0.4f, index, reward);
			case DailyTaskType.UpGrade_5:
				return new DailyTaskVO(type, progress, 5, 0.3f, index, reward);
			case DailyTaskType.GetItem_20:
				return new DailyTaskVO(type, progress, 20, 0.3f, index, reward);
			case DailyTaskType.PassLevel_5:
				return new DailyTaskVO(type, progress, 5, 0.4f, index, reward);
			case DailyTaskType.Destroy_1000:
				return new DailyTaskVO(type, progress, 1000, 0.3f, index, reward);
			case DailyTaskType.FinishTask_5:
				return new DailyTaskVO(type, progress, 5, 1.5f, index, reward);
			}
		}

		private void SetMiddleTaskType()
		{
			_taskMiddle.Clear();
			List<int> list = new List<int>();
			list.Add(3);
			list.Add(4);
			list.Add(5);
			list.Add(6);
			list.Add(7);
			List<int> list2 = list;
			for (int i = 0; i < 3; i++)
			{
				int index = UnityEngine.Random.Range(0, list2.Count);
				_taskMiddle.Add(list2[index]);
				list2.RemoveAt(index);
			}
		}

		private void LoadAllTaskPrefs()
		{
			_taskProgress.Clear();
			_taskTypes.Clear();
			_taskRewardStates.Clear();
			for (int i = 0; i < 6; i++)
			{
				_taskTypes.Add(GetTaskPrefs("TaskType_", i));
				_taskProgress.Add(GetTaskPrefs("TaskProgress_", i));
				_taskRewardStates.Add(GetTaskPrefs("TaskReward_", i));
			}
		}

		private void SetTaskFirePowerCost()
		{
			int num = Mathf.Max(GameGlobalData.FirePowerLevel, 20);
			FirePowerStruct firePower = Singleton<GameJson>.Instance.FirePowerJson[num];
			GameGlobalData.SetTaskFirePowerCost(GameUtils.FloatToUnit(GameGlobalData.GetFireCost(firePower)));
		}

		private void SaveAllTaskPrefs()
		{
			for (int i = 0; i < _list.Count; i++)
			{
				SetTaskPrefs("TaskType_", i, (int)_list[i].type);
				SetTaskPrefs("TaskProgress_", i, _list[i].progress);
				SetTaskPrefs("TaskReward_", i, _list[i].rewardState);
			}
		}

		public void SetTaskPrefs(DailyTaskType type)
		{
			if (_dic.ContainsKey(type))
			{
				DailyTaskVO dailyTaskVO = _list[_dic[type].index];
				SetTaskPrefs("TaskType_", dailyTaskVO.index, (int)dailyTaskVO.type);
				SetTaskPrefs("TaskProgress_", dailyTaskVO.index, dailyTaskVO.progress);
				SetTaskPrefs("TaskReward_", dailyTaskVO.index, dailyTaskVO.rewardState);
			}
		}

		private void SetTaskPrefs(string key, int index, int value)
		{
			PlayerPrefs.SetInt(key + index, value);
		}

		private int GetTaskPrefs(string key, int index)
		{
			return PlayerPrefs.GetInt(key + index, 0);
		}
	}
}

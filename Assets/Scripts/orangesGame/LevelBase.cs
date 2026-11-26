using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LevelBase
	{
		protected List<BallVO> _dataList;

		protected Dictionary<string, List<BarrierBase>> _objPool;

		protected Timer _timer;

		protected int _levelProgress;

		public int LevelProgress => _levelProgress;

		public LevelBase()
		{
			_objPool = new Dictionary<string, List<BarrierBase>>();
		}

		public virtual void SetLevelData(int curLevel, LevelStruct level)
		{
			if (_dataList == null)
			{
				_dataList = new List<BallVO>();
			}
			else
			{
				_dataList.Clear();
			}
		}

		public virtual void SetTimer()
		{
		}

		public virtual void StopTimer()
		{
			if (_timer != null && !_timer.isCancelled)
			{
				_timer.Cancel();
			}
		}

		public virtual void PauseTimer()
		{
			if (_timer != null && !_timer.isPaused && !_timer.isCancelled)
			{
				_timer.Pause();
			}
		}

		public virtual void ResumeTimer()
		{
			if (_timer != null && _timer.isPaused && !_timer.isCancelled)
			{
				_timer.Resume();
			}
		}

		public virtual float GetCurProgress()
		{
			return 0f;
		}

		public virtual void Divide(Vector3 pos, BallVO ballVO)
		{
		}

		public virtual BarrierBase CreateOneBall(Vector3 pos, int venomID, float hitCount, int childFlag)
		{
			return null;
		}

		public void CleanAllBallList()
		{
			StopTimer();
			foreach (List<BarrierBase> value in _objPool.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i].gameObject.activeSelf)
					{
						value[i].Clean();
					}
				}
			}
		}

		public void Pause()
		{
			PauseTimer();
			foreach (string key in _objPool.Keys)
			{
				Pause(key);
			}
		}

		public void Pause(string key)
		{
			if (_objPool.ContainsKey(key))
			{
				List<BarrierBase> list = _objPool[key];
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Pause();
				}
			}
		}

		public void Destroy()
		{
			foreach (List<BarrierBase> value in _objPool.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					UnityEngine.Object.Destroy(value[i].gameObject);
				}
				value.Clear();
			}
		}

		public void DestroyWithOutType(List<int> typeList)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < typeList.Count; i++)
			{
				list.Add(GetVenomPath(typeList[i]));
			}
			foreach (KeyValuePair<string, List<BarrierBase>> item in _objPool)
			{
				if (!string.IsNullOrEmpty(item.Key) && !list.Contains(item.Key) && item.Value.Count > 0)
				{
					for (int j = 0; j < item.Value.Count; j++)
					{
						UnityEngine.Object.Destroy(item.Value[j].gameObject);
					}
					item.Value.Clear();
				}
			}
		}

		public void Stop()
		{
			StopTimer();
			foreach (List<BarrierBase> value in _objPool.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i].gameObject.activeSelf)
					{
						value[i].Continue();
					}
				}
			}
		}

		public void Continue()
		{
			ResumeTimer();
			foreach (List<BarrierBase> value in _objPool.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i].gameObject.activeSelf)
					{
						value[i].Continue();
					}
				}
			}
		}

		public void OnIce(bool isActive)
		{
		}

		public Transform GetBarrierTran()
		{
			foreach (List<BarrierBase> value in _objPool.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i].gameObject.activeSelf)
					{
						return value[i].transform;
					}
				}
			}
			return null;
		}

		public Transform[] GetBarrierTrans()
		{
			List<Transform> list = new List<Transform>();
			foreach (List<BarrierBase> value in _objPool.Values)
			{
				for (int i = 0; i < value.Count; i++)
				{
					if (value[i].gameObject.activeSelf)
					{
						list.Add(value[i].transform);
					}
				}
			}
			return list.ToArray();
		}

		public T GetBall<T>(string key) where T : BarrierBase
		{
			List<BarrierBase> list = null;
			if (_objPool.ContainsKey(key))
			{
				list = _objPool[key];
			}
			else
			{
				list = new List<BarrierBase>();
				_objPool.Add(key, list);
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].gameObject.activeSelf)
				{
					list[i].gameObject.SetActive(value: true);
					return list[i] as T;
				}
			}
			T val = Object.Instantiate(Resources.Load<T>(key));
			val.SetParent(Singleton<UIViewManager>.Instance.GetLayerTran(40));
			list.Add(val);
			return val;
		}

		private string GetVenomPath(int type)
		{
			switch (type)
			{
			case 0:
				return "GameObject/Venom/Venom0";
			case 1:
				return "GameObject/Venom/VenomHealer";
			case 2:
				return "GameObject/Venom/VenomSlower";
			case 3:
				return "GameObject/Venom/VenomSpawner";
			case 4:
				return "GameObject/Venom/VenomTracker";
			case 5:
				return "GameObject/Venom/VenomRegenerationer";
			case 6:
				return "GameObject/Venom/VenomSplasher";
			case 7:
				return "GameObject/Venom/VenomSprinter";
			case 8:
				return "GameObject/Venom/VenomBlaster";
			case 9:
				return "GameObject/Venom/VenomObstacle";
			case 10:
				return "GameObject/Venom/VenomTeamer";
			case 11:
				return "GameObject/Venom/VenomCopy";
			default:
				return string.Empty;
			}
		}

		protected CoinVO[][] GetCoinVO(int jsonCost, int divideCount)
		{
			CoinStruct[] coinJson = Singleton<GameJson>.Instance.CoinJson;
			int num = (int)Mathf.Pow(2f, divideCount - 1);
			CoinVO[][] array = new CoinVO[num][];
			int[] array2 = new int[num];
			if (divideCount == 1)
			{
				array2[0] = jsonCost;
			}
			else if (jsonCost < num)
			{
				for (int i = 0; i < jsonCost; i++)
				{
					array2[i] = 1;
				}
				for (int j = jsonCost; j < num; j++)
				{
					array2[j] = 0;
				}
			}
			else
			{
				array2[0] = Random.Range(1, jsonCost - num);
				jsonCost -= array2[0];
				int num2 = num - 1;
				for (int k = 1; k < num2; k++)
				{
					array2[k] = Random.Range(1, jsonCost - num2 + k);
					jsonCost -= array2[k];
				}
				array2[num2] = jsonCost;
			}
			for (int l = 0; l < array.Length; l++)
			{
				int[] coinCost = GetCoinCost(coinJson, array2[l]);
				List<CoinVO> list = new List<CoinVO>();
				for (int m = 0; m < coinCost.Length; m++)
				{
					CoinVO item = default(CoinVO);
					item.Cost = coinJson[m].Cost;
					item.DisappearSpeed = (float)coinJson[m].DisappearSpeed;
					item.RotateSpeed = (float)coinJson[m].RotateSpeed;
					item.Count = coinCost[m];
					item.ItemType = coinJson[m].ItemType;
					list.Add(item);
				}
				array[l] = list.ToArray();
			}
			return array;
		}

		protected int[] GetCoinCost(CoinStruct[] coinJson, int jsonCost)
		{
			int[] array = new int[coinJson.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			int num = jsonCost;
			for (int num2 = coinJson.Length - 1; num2 >= 0; num2--)
			{
				int num3 = num % coinJson[num2].Cost;
				if (num3 == 0)
				{
					array[num2] = num / coinJson[num2].Cost;
					return array;
				}
				array[num2] = num / coinJson[num2].Cost;
				num = num3;
			}
			return array;
		}
	}
}

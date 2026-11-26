using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LevelCoomon : LevelBase
	{
		private float _curTime;

		private float _timeMax = 1.5f;

		private float _curWaveTime;

		private int _listLength;

		private int _showBound;

		private int[] _rounds;

		private int _roundsIndex;

		private int lvType;

		private const int LvNomal = 0;

		private const int LvBoss = 2;

		private bool showTips;

		private LevelDataHandle handle;

		private int _startIndex;

		private int _endIndex;

		private int _curCount;

		private List<Timer> _timerList = new List<Timer>();

		private List<int> l = new List<int>();

		public override void SetLevelData(int curLevel, LevelStruct level)
		{
			_timerList.Clear();
			base.SetLevelData(curLevel, level);
			BarrierCombineStruct[] barrierCombineJson = Singleton<GameJson>.Instance.BarrierCombineJson;
			int num = Random.Range(0, level.CombineIDs.Length);
			int num2 = level.CombineIDs[num];
			lvType = level.Type;
			if (barrierCombineJson.Length >= num2)
			{
				BarrierCombineStruct json = barrierCombineJson[num2 - 1];
				FireSpeedStruct[] fireSpeedJson = Singleton<GameJson>.Instance.FireSpeedJson;
				FirePowerStruct[] firePowerJson = Singleton<GameJson>.Instance.FirePowerJson;
				handle = new LevelDataHandle(fireSpeedJson, firePowerJson, json);
				handle.Init(GameGlobalData.FireSpeedLevel, GameGlobalData.FirePowerLevel);
				handle.SetRounds();
				json = handle.RefreshBarrierCombineJson(curLevel);
				GameGlobalData.BallRedValue = handle.RedAve;
				_dataList = CreateBattleData(json, level);
				_listLength = _dataList.Count;
				_levelProgress = 0;
				_roundsIndex = 0;
				_curTime = _timeMax;
				showTips = false;
				Singleton<CoinDataHandle>.Instance.Init();
			}
			else
			{
				UnityEngine.Debug.LogError("combineJson is null:" + num2);
				_dataList = null;
			}
		}

		public override void SetTimer()
		{
			_timer = Timer.Register(Time.deltaTime, null, UpdateSpawn, isLooped: true);
		}

		public override float GetCurProgress()
		{
			return (float)_levelProgress * 1f / (float)_listLength;
		}

		public override void Divide(Vector3 pos, BallVO ballVO)
		{
			int divideID = ballVO.DivideID;
			BallVO vo = ballVO;
			vo.BallID = divideID;
			vo.InitPos = new Vector3(pos.x, pos.y, 0f);
			VenomStruct venomStruct = Singleton<GameJson>.Instance.VenomJson[divideID - 1];
			vo.Scale = (float)venomStruct.Scale;
			vo.DivideID = venomStruct.DivideID;
			vo.ShowValue = venomStruct.ShowValue;
			vo.RatateSpeed = (float)venomStruct.RotateSpeed;
			vo.BombScale = (float)venomStruct.BombScale;
			vo.Shake = (float)venomStruct.Shake;
			vo.DivideCount = venomStruct.DivideCount;
			vo.MoveSpeed = (float)venomStruct.MoveSpeed;
			vo.HitEffect = (float)venomStruct.HitEffect;
			vo.isLaserHit = ballVO.isLaserHit;
			float num = ballVO.HitCount / 2f;
			float num2 = Mathf.Floor(num);
			float num3 = num - num2;
			if (num < 1f)
			{
				num = 1f;
				num3 = 0f;
			}
			int divideValue = vo.DivideValue;
			for (int i = 0; i < 2; i++)
			{
				vo.Dir = -2 * i + 1;
				vo.HitCount = num + num3 * (float)vo.Dir;
				GameGlobalData.HitCountHp += vo.HitCount;
				vo.DivideValue = i + divideValue * 2;
				BarrierBase venomState = GetVenomState(vo.Type);
				venomState.Divide(vo);
			}
		}

		public override BarrierBase CreateOneBall(Vector3 pos, int venomID, float hitCount, int childFlag)
		{
			VenomStruct[] venomJson = Singleton<GameJson>.Instance.VenomJson;
			BallVO ballVO = default(BallVO);
			ballVO.BallID = venomID;
			ballVO.HitCount = hitCount;
			ballVO.InitPos = new Vector3(pos.x, pos.y, 0f);
			if (venomJson.Length >= ballVO.BallID)
			{
				VenomStruct venomStruct = venomJson[ballVO.BallID - 1];
				ballVO.Type = venomStruct.Type;
				ballVO.Scale = (float)venomStruct.Scale;
				ballVO.DivideID = venomStruct.DivideID;
				ballVO.ShowValue = venomStruct.ShowValue;
				ballVO.Shake = (float)venomStruct.Shake;
				ballVO.BombScale = (float)venomStruct.BombScale;
				ballVO.RatateSpeed = (float)venomStruct.RotateSpeed;
				ballVO.DivideCount = venomStruct.DivideCount;
				ballVO.MoveSpeed = (float)venomStruct.MoveSpeed;
				ballVO.HitEffect = (float)venomStruct.HitEffect;
				ballVO.DivideValue = 0;
				ballVO.ChildFlag = childFlag;
				ballVO.CoinDatas = GetCoinVO(1, ballVO.DivideCount);
				ballVO.isLaserHit = false;
				GameGlobalData.SetNeedHitCount(ballVO.DivideCount);
			}
			else
			{
				UnityEngine.Debug.LogError("BarrierConfigStruct data is null:" + ballVO.BallID);
			}
			BarrierBase venomState = GetVenomState(ballVO.Type);
			venomState.Spawn(ballVO);
			return venomState;
		}

		private void UpdateSpawn(float time)
		{
			_curTime += time;
			if (GameGlobalData.ShowValue > _showBound)
			{
				return;
			}
			int num = _rounds.Length;
			bool flag = false;
			bool flag2 = false;
			if (showTips)
			{
				return;
			}
			if (num > 1)
			{
				if (lvType == 2)
				{
					if (num == _roundsIndex)
					{
						flag = true;
						flag2 = true;
					}
				}
				else if (lvType == 0 && num == _roundsIndex)
				{
					flag = false;
					flag2 = true;
				}
			}
			if (flag2)
			{
				if (!showTips)
				{
					showTips = true;
					ApplicationFacade.Instance.SendNotification("BATTLE_BOSSTIPS", flag);
					if (!showTips)
					{
					}
				}
			}
			else
			{
				Invading();
			}
		}

		public void Invading()
		{
			if (showTips)
			{
				showTips = false;
			}
			if (!(_curTime >= _timeMax))
			{
				return;
			}
			_curTime = 0f;
			if (_listLength <= _levelProgress)
			{
				return;
			}
			int num = _rounds[_roundsIndex];
			int levelProgress = _levelProgress;
			_roundsIndex++;
			int num2 = 3;
			int num3 = num / num2;
			int num4 = num % num2;
			if (num3 == 0)
			{
				_levelProgress += num;
				for (int i = levelProgress; i < _levelProgress; i++)
				{
					BallVO ballVO = _dataList[i];
					BarrierBase venomState = GetVenomState(ballVO.Type);
					venomState.Spawn(ballVO);
				}
				if (_listLength <= _levelProgress)
				{
					_timer.Cancel();
				}
				return;
			}
			if (num4 > 0)
			{
				num3++;
			}
			for (int j = 0; j < num3; j++)
			{
				if (j == num3 - 1)
				{
					if (num4 == 0)
					{
						_levelProgress += num2;
					}
					else
					{
						_levelProgress += num4;
					}
				}
				else
				{
					_levelProgress += num2;
				}
				l.Add(levelProgress);
				l.Add(_levelProgress);
				levelProgress = _levelProgress;
				Timer item = Timer.Register(1f * (float)j, delegate
				{
					int num5 = l[0];
					int num6 = l[1];
					l.RemoveAt(0);
					l.RemoveAt(0);
					for (int k = num5; k < num6; k++)
					{
						if (k < _dataList.Count)
						{
							BallVO ballVO2 = _dataList[k];
							BarrierBase venomState2 = GetVenomState(ballVO2.Type);
							venomState2.Spawn(ballVO2);
						}
					}
					if (_listLength <= _levelProgress)
					{
						_timer.Cancel();
					}
				});
				_timerList.Add(item);
			}
		}

		private BarrierBase GetVenomState(int type)
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				return GetBall<VenomCommon>("GameObject/Venom/Venom0");
			}
			switch (type)
			{
			default:
				return GetBall<VenomCommon>("GameObject/Venom/Venom0");
			case 1:
				return GetBall<VenomHealer>("GameObject/Venom/VenomHealer");
			case 2:
				return GetBall<VenomSlower>("GameObject/Venom/VenomSlower");
			case 3:
				return GetBall<VenomSpawner>("GameObject/Venom/VenomSpawner");
			case 4:
				return GetBall<VenomTracker>("GameObject/Venom/VenomTracker");
			case 5:
				return GetBall<VenomRegenerationer>("GameObject/Venom/VenomRegenerationer");
			case 6:
				return GetBall<VenomSplasher>("GameObject/Venom/VenomSplasher");
			case 7:
				return GetBall<VenomSprinter>("GameObject/Venom/VenomSprinter");
			case 8:
				return GetBall<VenomBlaster>("GameObject/Venom/VenomBlaster");
			case 9:
				return GetBall<VenomObstacle>("GameObject/Venom/VenomObstacle");
			case 10:
				return GetBall<VenomTeamer>("GameObject/Venom/VenomTeamer");
			case 11:
				return GetBall<VenomCopy>("GameObject/Venom/VenomCopy");
			}
		}

		private Vector3 GetInitPos(LevelStruct level, int num, int divideCount)
		{
			int num2 = Random.Range(0, level.InitPos.Length);
			int num3 = (Random.Range(0, 100) >= 50) ? 1 : (-1);
			if (divideCount <= 2)
			{
				return new Vector3((float)level.InitPos[num2] * (float)num3, (num3 != 1) ? 9.3f : 10f, 0f);
			}
			if (divideCount == 5)
			{
				return new Vector3(0f, (num3 != 1) ? 9.3f : 10f, 0f);
			}
			return new Vector3((float)level.InitPos[num2] * (float)num3, (num3 != 1) ? 10f : 10.2f, 0f);
		}

		private List<BallVO> CreateBattleData(BarrierCombineStruct combine, LevelStruct level)
		{
			int num = combine.BallID.Length;
			if (combine.HitCont.Length % num != 0 || combine.Dir.Length % num != 0)
			{
				UnityEngine.Debug.LogError("combine length is error :");
				return null;
			}
			VenomStruct[] venomJson = Singleton<GameJson>.Instance.VenomJson;
			CoinLevelStruct coinCount = Singleton<GameJson>.Instance.CoinLevelJson[GameGlobalData.CoinCountLevel];
			float coinValueTxt = GameGlobalData.GetCoinValueTxt(coinCount);
			_showBound = level.ShowBound;
			_rounds = combine.Rounds;
			List<BallVO> list = new List<BallVO>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < num; i++)
			{
				BallVO item = default(BallVO);
				item.BallID = combine.BallID[i];
				item.HitCount = GameUtils.UnitToFloat(combine.HitCont[i]);
				item.isLaserHit = false;
				GameGlobalData.HitCountHp += item.HitCount;
				if (venomJson.Length >= item.BallID)
				{
					VenomStruct venomStruct = venomJson[item.BallID - 1];
					item.Type = venomStruct.Type;
					item.Scale = (float)venomStruct.Scale;
					item.DivideID = venomStruct.DivideID;
					item.ShowValue = venomStruct.ShowValue;
					item.Shake = (float)venomStruct.Shake;
					item.BombScale = (float)venomStruct.BombScale;
					item.RatateSpeed = (float)venomStruct.RotateSpeed;
					item.DivideCount = venomStruct.DivideCount;
					item.MoveSpeed = (float)venomStruct.MoveSpeed;
					item.HitEffect = (float)venomStruct.HitEffect;
					item.DivideValue = 0;
					item.ChildFlag = 0;
					int num2 = Mathf.RoundToInt((float)combine.Coin[i] * coinValueTxt);
					if (num2 < 0)
					{
						num2 = 1;
					}
					item.CoinDatas = GetCoinVO(num2, item.DivideCount);
					item.InitPos = GetInitPos(level, i, item.DivideCount);
					if (item.DivideCount > 2)
					{
						list2.Add(i);
					}
					GameGlobalData.SetNeedHitCount(item.DivideCount);
					list.Add(item);
					continue;
				}
				UnityEngine.Debug.LogError("BarrierConfigStruct data is null:" + item.BallID);
				return null;
			}
			return GenerateItem(list2, list);
		}

		private List<BallVO> GenerateItem(List<int> voForItems, List<BallVO> list)
		{
			voForItems = GetDisorderBytes(voForItems);
			int num = Singleton<ItemDropManager>.Instance.DropItemCount();
			for (int i = 0; i < voForItems.Count; i++)
			{
				int num2 = voForItems[0];
				voForItems.RemoveAt(0);
				if (num <= 0)
				{
					break;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (voForItems.Count > 0 && num2 == j)
					{
						BallVO value = list[j];
						value.HasItem = 1;
						list[j] = value;
						num--;
					}
				}
			}
			return list;
		}

		public static List<int> GetDisorderBytes(List<int> arr)
		{
			List<int> list = new List<int>();
			int count = arr.Count;
			while (list.Count < count)
			{
				int index = Random.Range(0, arr.Count - 1);
				if (!list.Contains(arr[index]))
				{
					list.Add(arr[index]);
					arr.Remove(arr[index]);
				}
			}
			return list;
		}

		private float lowerHp(float oldHp)
		{
			int failCount = GameGlobalData.FailCount;
			int num = 2;
			float num2 = 0.05f;
			float b = 0.2f;
			if (failCount < num)
			{
				return oldHp;
			}
			int num3 = Mathf.Max(failCount - num, 0);
			float a = (float)num3 * num2;
			float num4 = Mathf.Min(a, b);
			float f = num4 * oldHp;
			return oldHp - Mathf.Floor(f);
		}

		public override void StopTimer()
		{
			base.StopTimer();
			for (int i = 0; i < _timerList.Count; i++)
			{
				if (_timerList[i] != null && !_timerList[i].isCancelled)
				{
					_timerList[i].Cancel();
				}
			}
		}

		public override void PauseTimer()
		{
			base.PauseTimer();
			for (int i = 0; i < _timerList.Count; i++)
			{
				if (_timerList[i] != null && !_timerList[i].isPaused && !_timerList[i].isCancelled)
				{
					_timerList[i].Pause();
				}
			}
		}

		public override void ResumeTimer()
		{
			base.ResumeTimer();
			for (int i = 0; i < _timerList.Count; i++)
			{
				if (_timerList[i] != null && _timerList[i].isPaused && !_timerList[i].isCancelled)
				{
					_timerList[i].Resume();
				}
			}
		}
	}
}

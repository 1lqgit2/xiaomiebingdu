using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class LevelDataHandle
	{
		private FireSpeedStruct[] _speedJson;

		private FirePowerStruct[] _powerJson;

		private BarrierCombineStruct _barrierCombineJson;

		private BossLevelStruct[] _bossLevelJson;

		private List<List<RoundVO>> _roundList = new List<List<RoundVO>>();

		private float _ave;

		private List<RoundHpRangeVO> _roundHpRangeList = new List<RoundHpRangeVO>();

		private List<float> _roundHps = new List<float>();

		private float[] _hpRate = new float[3]
		{
			0.2f,
			0.3f,
			0.5f
		};

		private float[] _thridRoundRate = new float[5]
		{
			0.2f,
			0.4f,
			0.6f,
			0.8f,
			1f
		};

		private float _totalHp;

		private int _coin;

		private int[] _venomIds;

		private float[] _venomWeights;

		private float _dps;

		private int _curLevel;

		public float RedAve => _ave;

		public float Dps => _dps;

		public LevelDataHandle(FireSpeedStruct[] speedJson, FirePowerStruct[] powerJson, BarrierCombineStruct json)
		{
			_speedJson = speedJson;
			_powerJson = powerJson;
			_barrierCombineJson = json;
		}

		public void Init(int speedLevel, int powerLevel)
		{
			Init(speedLevel, powerLevel, 0);
		}

		public void Init(int speedLevel, int powerLevel, int curLevel)
		{
			_totalHp = GameUtils.UnitToFloat(_barrierCombineJson.TotalHp);
			_coin = _barrierCombineJson.CoinNum;
			_venomIds = _barrierCombineJson.VenomIDs;
			int[] venomWeights = _barrierCombineJson.VenomWeights;
			_venomWeights = new float[venomWeights.Length];
			int num = 0;
			for (int i = 0; i < venomWeights.Length; i++)
			{
				num += venomWeights[i];
			}
			_venomWeights[0] = (float)venomWeights[0] / (float)num * 100f;
			for (int j = 1; j < venomWeights.Length; j++)
			{
				_venomWeights[j] = _venomWeights[j - 1] + (float)venomWeights[j] / (float)num * 100f;
			}
			SetDps(speedLevel, powerLevel);
			SetAve();
			SetRoundHp();
			SetRoundHpRange();
			_curLevel = curLevel;
		}

		public void SetRounds()
		{
			SetOneRound(_roundHps[0]);
			SetSecondRound(_roundHps[1]);
			SetThridRound(_roundHps[2]);
		}

		public BarrierCombineStruct RefreshBarrierCombineJson(int level)
		{
			List<int> list = new List<int>();
			List<string> list2 = new List<string>();
			List<int> list3 = new List<int>();
			List<int> list4 = new List<int>();
			List<int> list5 = new List<int>();
			for (int i = 0; i < _roundList.Count; i++)
			{
				if (_roundList[i].Count != 0)
				{
					list5.Add(_roundList[i].Count);
				}
				for (int j = 0; j < _roundList[i].Count; j++)
				{
					List<int> list6 = list;
					RoundVO roundVO = _roundList[i][j];
					list6.Add(roundVO.id);
					List<string> list7 = list2;
					RoundVO roundVO2 = _roundList[i][j];
					list7.Add(GameUtils.FloatToUnit(roundVO2.hitCount));
					list3.Add(Random.Range(0, 2));
					List<int> list8 = list4;
					RoundVO roundVO3 = _roundList[i][j];
					list8.Add((int)roundVO3.coin);
				}
			}
			if (_barrierCombineJson.BossID != 0)
			{
				list.Add(_barrierCombineJson.BossID);
				float bossHitCount = GetBossHitCount(level, _barrierCombineJson.BossHPMul);
				string item = GameUtils.FloatToUnit(bossHitCount);
				list2.Add(item);
				list3.Add(Random.Range(0, 2));
				float coin = GetCoin(bossHitCount);
				list4.Add((int)coin);
				List<int> list9;
				int index;
				(list9 = list5)[index = list5.Count - 1] = list9[index] + 1;
			}
			_barrierCombineJson.BallID = list.ToArray();
			_barrierCombineJson.HitCont = list2.ToArray();
			_barrierCombineJson.Dir = list3.ToArray();
			_barrierCombineJson.Coin = list4.ToArray();
			_barrierCombineJson.Rounds = list5.ToArray();
			return _barrierCombineJson;
		}

		public string[] GetHitCount()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < _roundList.Count; i++)
			{
				for (int j = 0; j < _roundList[i].Count; j++)
				{
					List<string> list2 = list;
					RoundVO roundVO = _roundList[i][j];
					list2.Add(GameUtils.FloatToUnit(roundVO.hitCount));
				}
			}
			return list.ToArray();
		}

		public string[] GetHps()
		{
			List<string> list = new List<string>();
			for (int i = 0; i < _roundList.Count; i++)
			{
				for (int j = 0; j < _roundList[i].Count; j++)
				{
					List<string> list2 = list;
					RoundVO roundVO = _roundList[i][j];
					list2.Add(GameUtils.FloatToUnit(roundVO.hp));
				}
			}
			return list.ToArray();
		}

		public float GetTotalHitCount()
		{
			VenomStruct[] venomJson = Singleton<GameJson>.Instance.VenomJson;
			float temp = 0f;
			for (int i = 0; i < _roundList.Count; i++)
			{
				for (int j = 0; j < _roundList[i].Count; j++)
				{
					float num = temp;
					RoundVO roundVO = _roundList[i][j];
					temp = num + roundVO.hitCount;
					VenomStruct[] array = venomJson;
					RoundVO roundVO2 = _roundList[i][j];
					int divideCount = array[roundVO2.id - 1].DivideCount;
					RoundVO roundVO3 = _roundList[i][j];
					float hitCount = roundVO3.hitCount;
					if (divideCount != 1)
					{
						SetDivideCount(divideCount, hitCount, ref temp);
					}
				}
			}
			return temp;
		}

		private void SetDivideCount(int level, float count, ref float temp)
		{
			if (level > 1)
			{
				float num = count / 2f;
				float num2 = Mathf.Floor(num);
				float num3 = num - num2;
				if (num < 1f)
				{
					num = 1f;
					num3 = 0f;
				}
				for (int i = 0; i < 2; i++)
				{
					float num4 = num + num3 * (float)(-2 * i + 1);
					temp += num4;
					SetDivideCount(level - 1, num4, ref temp);
				}
			}
		}

		public int[] GetRounds()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < _roundList.Count; i++)
			{
				if (_roundList[i].Count != 0)
				{
					list.Add(_roundList[i].Count);
				}
			}
			return list.ToArray();
		}

		public float GetHitCount(int round)
		{
			RoundHpRangeVO roundHpRangeVO = _roundHpRangeList[round];
			return GetHitCount(roundHpRangeVO.minHp, roundHpRangeVO.maxHp);
		}

		private void SetDps(int speedLevel, int powerLevel)
		{
			int count = _speedJson[speedLevel].Count;
			float num = GameUtils.UnitToFloat(_powerJson[powerLevel].Power);
			if (count <= 50)
			{
				_dps = (float)count * num;
			}
			else if (count <= 100)
			{
				_dps = num * ((float)(count - 50) * 0.5f + 50f);
			}
			else
			{
				_dps = num * 75f;
			}
		}

		private void SetAve()
		{
			_ave = _dps * 2.7f;
		}

		private void SetRoundHp()
		{
			for (int i = 0; i < _hpRate.Length; i++)
			{
				_roundHps.Add(Mathf.Ceil(_totalHp * _hpRate[i]));
			}
		}

		private void SetRoundHpRange()
		{
			_roundHpRangeList.Clear();
			int num = 3;
			float[] array = new float[3]
			{
				0.01f,
				0.1f,
				0.2f
			};
			float[] array2 = new float[3]
			{
				1.88f,
				2.22f,
				3f
			};
			for (int i = 0; i < num; i++)
			{
				RoundHpRangeVO item = default(RoundHpRangeVO);
				item.minHp = Mathf.Ceil(_ave * array[i]);
				item.maxHp = Mathf.Ceil(_ave * array2[i]);
				_roundHpRangeList.Add(item);
			}
		}

		private void SetOneRound(float hp)
		{
			List<RoundVO> list = new List<RoundVO>();
			RoundHpRangeVO roundHpRangeVO = _roundHpRangeList[0];
			RoundVO item = default(RoundVO);
			int venomWeightIndex = GetVenomWeightIndex();
			item.id = _venomIds[venomWeightIndex];
			item.hitCount = roundHpRangeVO.minHp * 1f;
			item.hp = GetSingleHp(item.id, item.hitCount);
			item.coin = GetCoin(item.hp);
			list.Add(item);
			for (float num = hp - item.hp; num > 0f; num -= item.hp)
			{
				item = default(RoundVO);
				venomWeightIndex = GetVenomWeightIndex();
				item.id = _venomIds[venomWeightIndex];
				item.hitCount = GetHitCount(roundHpRangeVO.minHp, roundHpRangeVO.maxHp);
				float singleHp = GetSingleHp(item.id, item.hitCount);
				if (singleHp > num)
				{
					item.hp = num;
					item.hitCount = SingleHpToHitCount(item.id, item.hp);
				}
				else
				{
					item.hp = singleHp;
				}
				item.coin = GetCoin(item.hp);
				list.Add(item);
			}
			_roundList.Add(list);
		}

		private void SetSecondRound(float hp)
		{
			List<RoundVO> list = new List<RoundVO>();
			RoundHpRangeVO roundHpRangeVO = _roundHpRangeList[1];
			RoundVO item = default(RoundVO);
			int venomWeightIndex = GetVenomWeightIndex();
			item.id = _venomIds[venomWeightIndex];
			item.hitCount = roundHpRangeVO.minHp * 1f;
			item.hp = GetSingleHp(item.id, item.hitCount);
			item.coin = GetCoin(item.hp);
			list.Add(item);
			for (float num = hp - item.hp; num > 0f; num -= item.hp)
			{
				item = default(RoundVO);
				venomWeightIndex = GetVenomWeightIndex();
				item.id = _venomIds[venomWeightIndex];
				item.hitCount = GetHitCount(roundHpRangeVO.minHp, roundHpRangeVO.maxHp);
				float singleHp = GetSingleHp(item.id, item.hitCount);
				if (singleHp > num)
				{
					item.hp = num;
					item.hitCount = SingleHpToHitCount(item.id, item.hp);
				}
				else
				{
					item.hp = singleHp;
				}
				item.coin = GetCoin(item.hp);
				list.Add(item);
			}
			_roundList.Add(list);
		}

		private void SetThridRound(float hp)
		{
			List<RoundVO> list = new List<RoundVO>();
			RoundHpRangeVO roundHpRangeVO = _roundHpRangeList[2];
			float num = hp;
			for (int i = 0; i < _thridRoundRate.Length; i++)
			{
				if (num > 0f)
				{
					RoundVO item = default(RoundVO);
					int venomWeightIndex = GetVenomWeightIndex();
					item.id = _venomIds[venomWeightIndex];
					item.hitCount = Mathf.Ceil(roundHpRangeVO.minHp * _thridRoundRate[i]);
					float singleHp = GetSingleHp(item.id, item.hitCount);
					if (singleHp > num)
					{
						item.hp = num;
						item.hitCount = SingleHpToHitCount(item.id, item.hp);
					}
					else
					{
						item.hp = singleHp;
					}
					item.coin = GetCoin(item.hp);
					list.Add(item);
					num -= item.hp;
				}
			}
			while (num > 0f)
			{
				RoundVO item2 = default(RoundVO);
				int venomWeightIndex2 = GetVenomWeightIndex();
				item2.id = _venomIds[venomWeightIndex2];
				item2.hitCount = GetHitCount(roundHpRangeVO.minHp, roundHpRangeVO.maxHp);
				float singleHp2 = GetSingleHp(item2.id, item2.hitCount);
				if (singleHp2 > num)
				{
					item2.hp = num;
					item2.hitCount = SingleHpToHitCount(item2.id, item2.hp);
				}
				else
				{
					item2.hp = singleHp2;
				}
				item2.coin = GetCoin(item2.hp);
				list.Add(item2);
				num -= item2.hp;
			}
			_roundList.Add(list);
		}

		private float GetCoin(float hp)
		{
			return Mathf.Ceil(hp * (float)_coin / _totalHp);
		}

		private float GetSingleHp(int id, float hitCount)
		{
			int num = id % 5;
			if (id >= 26 && id <= 30)
			{
				float num2 = 0f;
				for (int i = 0; i < num; i++)
				{
					num2 += hitCount * Mathf.Pow(1.5f, i);
				}
				return num2;
			}
			return Mathf.Ceil(hitCount * (float)num);
		}

		private float SingleHpToHitCount(int id, float hp)
		{
			int num = id % 5;
			return Mathf.Ceil(hp / (float)num);
		}

		private float GetHitCount(float min, float max)
		{
			int num = Random.Range(0, 100);
			float num2 = (float)num / 100f;
			float f = num2 * (max - min) + min;
			return Mathf.Ceil(f);
		}

		private int GetVenomWeightIndex()
		{
			int num = Random.Range(0, 100);
			for (int i = 1; i < _venomWeights.Length; i++)
			{
				if ((float)num <= _venomWeights[i])
				{
					return i;
				}
			}
			return 0;
		}

		private float GetBossHitCount(int level)
		{
			return Mathf.Ceil(Mathf.Min(9f + (float)(level + 1) / 2f, 36f) * 0.27f * _dps);
		}

		private float GetBossHitCount(int level, double hpMul)
		{
			return Mathf.Ceil((float)hpMul * 0.27f * _dps);
		}

		private int GetBossID()
		{
			for (int i = 0; i < _bossLevelJson.Length; i++)
			{
				if (_curLevel == _bossLevelJson[i].LevelID)
				{
					return _bossLevelJson[i].BossID;
				}
			}
			return 10;
		}
	}
}

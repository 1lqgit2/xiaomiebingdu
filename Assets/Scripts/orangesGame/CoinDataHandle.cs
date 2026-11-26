using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class CoinDataHandle : Singleton<CoinDataHandle>
	{
		private List<int> _rateList = new List<int>();

		private float _powerLevel;

		private int _level;

		private int _failCount;

		private float _coinLevelValue;

		private int _testTotalCoin;

		private float _coin_1 => _powerLevel * 100f / (float)_level;

		private float _coin_2 => _coin_1;

		private float _coin_3 => (float)_level * 100f / _powerLevel;

		private float _coin_4 => (float)_level * 100f / (_powerLevel + 50f * (float)_failCount);

		public void Init()
		{
			FirePowerStruct firePowerStruct = Singleton<GameJson>.Instance.FirePowerJson[GameGlobalData.FirePowerLevel];
			CoinLevelStruct coinCount = Singleton<GameJson>.Instance.CoinLevelJson[GameGlobalData.CoinCountLevel];
			_powerLevel = GameGlobalData.FirePowerLevel + 1;
			_level = GameGlobalData.ActualLevel;
			_failCount = GameGlobalData.FailCount;
			_coinLevelValue = GameGlobalData.GetCoinValueTxt(coinCount) * Mathf.Max(1f, Mathf.Pow(GameGlobalData.ActualLevel + 100, 0.43f) / 10f);
			float num = _coin_1 + _coin_2 + _coin_3 + _coin_4;
			_rateList.Clear();
			_rateList.Add(Mathf.RoundToInt(_coin_1 / num * 100f));
			_rateList.Add(_rateList[0] + Mathf.RoundToInt(_coin_2 / num * 100f));
			_rateList.Add(_rateList[1] + Mathf.RoundToInt(_coin_3 / num * 100f));
			_rateList.Add(_rateList[2] + Mathf.RoundToInt(_coin_4 / num * 100f));
		}

		public int GetCoinIndex()
		{
			int num = Random.Range(0, 100);
			for (int i = 0; i < _rateList.Count; i++)
			{
				if (num <= _rateList[i])
				{
					return i;
				}
			}
			return 0;
		}

		public void DebugLogTotalCoin()
		{
			UnityEngine.Debug.Log("total:" + _testTotalCoin);
			_testTotalCoin = 0;
		}

		public float GetCoinLevelValue()
		{
			return _coinLevelValue;
		}
	}
}

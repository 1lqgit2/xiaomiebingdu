using UnityEngine;

namespace orangesGame
{
	internal class DiamondManager : Singleton<DiamondManager>
	{
		private bool IsUseDiamondRetry;

		private int UseDiamondRetryTimes;

		private int UseDiamondRetryNum = 10;

		private float ExchangeRate = 0.1f;

		private int ShowVideoDiamondNum = 10;

		private long CoolTime = 1800000L;

		private long GotShowVideoRewardTime;

		private int ShareDiamonNum = 10;

		private int VipFreeDiamonNum = 100;

		public int GetUseDiamondRetryTimes()
		{
			return UseDiamondRetryTimes;
		}

		public long GetCoolTime()
		{
			return CoolTime;
		}

		private void AddUseDiamondRetryTimes()
		{
			UseDiamondRetryTimes++;
		}

		public void InitGotVideoReward(long ticks)
		{
			GotShowVideoRewardTime = ticks;
		}

		public long RemainGetShowVideoRewardTime()
		{
			if (GotShowVideoRewardTime == 0)
			{
				return 0L;
			}
			long num = GotShowVideoRewardTime + CoolTime;
			long num2 = num - GameGlobalData.GetTimestamp();
			if (num2 < 0)
			{
				return 0L;
			}
			return num2;
		}

		public bool IsCanGotShowVideoReward()
		{
			if (GotShowVideoRewardTime == 0)
			{
				return true;
			}
			if (GameGlobalData.GetTimestamp() > GotShowVideoRewardTime + CoolTime)
			{
				return true;
			}
			return false;
		}

		public void DoGotShowVideoReward()
		{
			if (IsCanGotShowVideoReward())
			{
				SetGotShowVideoReward();
				ShowVideoDiamond();
				GameAnalysisData.GotDiamondByVideo();
			}
		}

		public void SetGotShowVideoReward()
		{
			GotShowVideoRewardTime = GameGlobalData.GetTimestamp();
			PlayerPrefs.SetString("GotShowVideoRewardTime", GotShowVideoRewardTime.ToString());
		}

		public void ShowVideoDiamond()
		{
			GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + (float)ShowVideoDiamondNum);
		}

		public int ShareDiamond()
		{
			GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + (float)ShareDiamonNum);
			return ShareDiamonNum;
		}

		public void VipFreeDiamond()
		{
			GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + (float)VipFreeDiamonNum);
		}

		public void SetUseDiamondRetry()
		{
			long oldDiamond = (long)GameGlobalData.CurDiamond;
			int cost = CalCostDiamondRetry();
			GameGlobalData.CurDiamond -= CalCostDiamondRetry();
			GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond);
			IsUseDiamondRetry = true;
			AddUseDiamondRetryTimes();
			GameAnalysisData.DiamondRelive(GetUseDiamondRetryTimes(), oldDiamond, cost);
		}

		public void Clean()
		{
			IsUseDiamondRetry = false;
			UseDiamondRetryTimes = 0;
		}

		public int CalCostDiamondRetry()
		{
			return UseDiamondRetryNum * (UseDiamondRetryTimes + 1);
		}

		public float CalDiamondExchangeGold(int diamondNum)
		{
			if (diamondNum <= 0)
			{
				return 0f;
			}
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			float fireCost = GameGlobalData.GetFireCost(firePowerData);
			return Mathf.Ceil((float)diamondNum * fireCost * ExchangeRate);
		}

		public bool DiamondExchangeGold(int diamondNum)
		{
			if (!IsEnough(diamondNum))
			{
				return false;
			}
			float num = CalDiamondExchangeGold(diamondNum);
			long oldDiamondNum = (long)GameGlobalData.CurDiamond;
			long oldGold = (long)GameGlobalData.CurCoin;
			GameGlobalData.CurDiamond -= diamondNum;
			GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond);
			GameGlobalData.AddCurCoin(num);
			GameAnalysisData.DiamondExchange(diamondNum, oldDiamondNum, (long)num, oldGold);
			return true;
		}

		public bool IsEnough(int diamondNum)
		{
			return GameGlobalData.CurDiamond >= (float)diamondNum;
		}

		public bool ComsumeDiamond(int index)
		{
			if (IsEnough(index))
			{
				GameGlobalData.CurDiamond -= index;
				GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond);
				return true;
			}
			return false;
		}
	}
}

using UnityEngine;

namespace orangesGame
{
	internal class DiamondStorageManager : Singleton<DiamondStorageManager>
	{
		private int TotalDiamond;

		private int LimitBuy = 450;

		private int MaxBuy = 600;

		public void InitToTalDiamond(int totalDiamond)
		{
			TotalDiamond = totalDiamond;
		}

		public void SetTotalDiamond(int totalDiamond)
		{
			InitToTalDiamond(totalDiamond);
			PlayerPrefs.SetInt("TotalStorageDiamond", TotalDiamond);
		}

		public int RandomDiamond()
		{
			return Random.Range(5, 9);
		}

		public bool AddStorageDiamond(int num)
		{
			TotalDiamond += num;
			if (TotalDiamond >= MaxBuy)
			{
				TotalDiamond = MaxBuy;
			}
			SetTotalDiamond(TotalDiamond);
			if (TotalDiamond >= LimitBuy)
			{
				return true;
			}
			return false;
		}

		public void ShowVideoAndClean(int AddDiamond)
		{
			GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + (float)AddDiamond);
			GameAnalysisData.ShowVideoDiamondStorage(AddDiamond);
		}

		public void BuyAndClean()
		{
			if (TotalDiamond >= LimitBuy)
			{
				GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond + (float)TotalDiamond);
				GameAnalysisData.BuyDiamondStorage(TotalDiamond);
				SetTotalDiamond(0);
			}
		}

		public bool IsCanBuy()
		{
			return TotalDiamond >= LimitBuy;
		}

		public float GetProcess()
		{
			if (TotalDiamond >= MaxBuy)
			{
				return 1f;
			}
			return (float)TotalDiamond / (float)MaxBuy;
		}

		public int GetTotalDiamond()
		{
			return TotalDiamond;
		}

		public float GetBuyCost()
		{
			return 2.99f;
		}
	}
}

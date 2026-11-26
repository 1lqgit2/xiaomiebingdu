using UnityEngine;

namespace orangesGame
{
	internal class TutorialManager : Singleton<TutorialManager>
	{
		public static bool Start;

		private int FireSpeedCost = 5;

		private int FirePowerCost = 5;

		private bool FireSpeedGuide;

		private bool FirePowerGuide;

		public void InitGuide()
		{
			int @int = PlayerPrefs.GetInt("FireSpeedGuide", 0);
			FireSpeedGuide = (@int == 1);
			int int2 = PlayerPrefs.GetInt("FirePowerGuide", 0);
			FirePowerGuide = (int2 == 1);
		}

		public void CleanGuideStatus()
		{
			SetGuideFireSpeed(temp: false);
			SetGuideFirePower(temp: false);
		}

		public void SetGuideFireSpeed(bool temp)
		{
			FireSpeedGuide = temp;
			PlayerPrefs.SetInt("FireSpeedGuide", FireSpeedGuide ? 1 : 0);
			if (Start)
			{
				Start = false;
			}
		}

		public void SetGuideFirePower(bool temp)
		{
			FirePowerGuide = temp;
			PlayerPrefs.SetInt("FirePowerGuide", FirePowerGuide ? 1 : 0);
			if (Start)
			{
				Start = false;
			}
		}

		public int IsGuideToUpgrade()
		{
			if (GameGlobalData.IsTutorialNew())
			{
				return 0;
			}
			if (!Start)
			{
				return 0;
			}
			FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
			float fireSpeedCost = GameGlobalData.GetFireSpeedCost(fireSpeedData);
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			float fireCost = GameGlobalData.GetFireCost(firePowerData);
			if (GameGlobalData.CurCoin >= fireCost * (float)FirePowerCost && !FirePowerGuide)
			{
				return 1;
			}
			if (GameGlobalData.CurCoin >= fireSpeedCost * (float)FireSpeedCost && !FireSpeedGuide)
			{
				return 2;
			}
			return 0;
		}

		public bool HasGuideTips()
		{
			if (GameGlobalData.IsTutorialNew())
			{
				return false;
			}
			if (!Start)
			{
				return false;
			}
			FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
			float fireSpeedCost = GameGlobalData.GetFireSpeedCost(fireSpeedData);
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			float fireCost = GameGlobalData.GetFireCost(firePowerData);
			if (FirePowerGuide && FireSpeedGuide)
			{
				return false;
			}
			if (!FirePowerGuide && GameGlobalData.CurCoin >= fireCost * (float)FirePowerCost)
			{
				return true;
			}
			if (!FireSpeedGuide && GameGlobalData.CurCoin >= fireSpeedCost * (float)FireSpeedCost)
			{
				return true;
			}
			return false;
		}

		public void InitStart()
		{
			if (!GameGlobalData.IsTutorialNew())
			{
				Start = true;
			}
		}
	}
}

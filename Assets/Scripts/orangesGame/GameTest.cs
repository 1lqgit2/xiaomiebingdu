using System;
using UnityEngine;

namespace orangesGame
{
	public class GameTest : Singleton<GameTest>
	{
		public void Init()
		{
			long num = -1L;
			ulong num2 = ulong.MaxValue;
			UnityEngine.Debug.Log(Convert.ToString(5, 2));
			UnityEngine.Debug.Log(ulong.MaxValue + "@@" + uint.MaxValue + "@@" + (uint)num2);
			UnityEngine.Debug.Log((ulong)num);
			InitCoin();
			if (GameGlobalData.GameTest == GameTestState.RecordUnlimitCoin)
			{
				WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
				for (int i = 1; i < weaponJson.Length; i++)
				{
					weaponJson[i].UnLockLevel = weaponJson[0].UnLockLevel;
				}
				Singleton<GameJson>.Instance.WeaponJson = weaponJson;
				GameGlobalData.CurLevel = 6;
				GameGlobalData.OpenWeaponCount = 5;
				GameGlobalData.WeaponID = 0;
				return;
			}
			GameGlobalData.CurLevel = 99;
			GameGlobalData.OpenWeaponCount = 5;
			GameGlobalData.WeaponID = 0;
			Singleton<GameJson>.Instance.InitTest();
			TestWeaponStruct[] testWeaponJson = Singleton<GameJson>.Instance.TestWeaponJson;
			GameGlobalData.FireSpeedLevel = 120;
			GameGlobalData.FirePowerLevel = 50;
			GameGlobalData.WeaponLevels[0] = 31;
			GameGlobalData.WeaponLevels[1] = 13;
			for (int j = 2; j < GameGlobalData.WeaponLevels.Length; j++)
			{
				if (j % 2 == 0)
				{
					GameGlobalData.WeaponLevels[j] = 31;
				}
				else
				{
					GameGlobalData.WeaponLevels[j] = 998;
				}
			}
			WeaponStruct[] weaponJson2 = Singleton<GameJson>.Instance.WeaponJson;
			weaponJson2[0].FireDistance = 3.0;
			weaponJson2[2].FireDistance = 2.0;
			weaponJson2[3].FireDistance = 3.0;
			weaponJson2[3].BulletSpeed = 20.0;
			Singleton<GameJson>.Instance.WeaponJson = weaponJson2;
			GameGlobalData.ShakeFlag = 0;
			LevelStruct[] levelJson = Singleton<GameJson>.Instance.LevelJson;
			int num3 = 0;
			for (int k = GameGlobalData.CurLevel; k < levelJson.Length; k++)
			{
				levelJson[k].BgID = num3 % 6 + 1;
				num3++;
				levelJson[k].ItemsDropNum[0] = 10;
				levelJson[k].ItemsDropNum[1] = 20;
				levelJson[k].ItemsDropWeight[0] = 100;
				levelJson[k].ItemsDropWeight[1] = 100;
				levelJson[k].ItemsDropWeight[2] = 100;
				levelJson[k].ItemsDropWeight[3] = 100;
				levelJson[k].ItemsDropWeight[4] = 0;
				levelJson[k].ItemsDropWeight[5] = 0;
				levelJson[k].ItemsDropWeight[6] = 20;
				levelJson[k].ItemsDropWeight[7] = 100;
			}
			Singleton<GameJson>.Instance.LevelJson = levelJson;
			BarrierCombineStruct[] barrierCombineJson = Singleton<GameJson>.Instance.BarrierCombineJson;
			for (int l = 0; l < barrierCombineJson.Length; l++)
			{
				int[] venomIDs = barrierCombineJson[l].VenomIDs;
				for (int m = 0; m < venomIDs.Length; m++)
				{
					int num4 = venomIDs[m] % 5;
					if (num4 < 3)
					{
						venomIDs[m] += 2;
					}
				}
				barrierCombineJson[l].VenomIDs = venomIDs;
			}
			Singleton<GameJson>.Instance.BarrierCombineJson = barrierCombineJson;
		}

		public void InitCoin()
		{
			GameGlobalData.CurCoin = GameUtils.UnitToFloat("10Y");
		}
	}
}

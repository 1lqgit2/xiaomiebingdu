using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class PropertyProxy : Proxy, IProxy
	{
		public bool isInit;

		private const int FREE_RATE = 0;

		private const int FREE_UPGRADE = 1;

		private int _maxWeaponID;

		private int _tempLevel_0;

		private int _tempLevel_1;

		private int _freeUpGradeRate;

		private int _maxLevelUseRate;

		private int _preTempWeaponID = -1;

		private List<int> _freeUpGradeList = new List<int>();

		public PropertyProxy(string name)
			: base(name)
		{
			_freeUpGradeRate = (int)Singleton<GameJson>.Instance.CommonJson[1].Value;
		}

		public void SetMaxWeaponState()
		{
			_maxLevelUseRate = (int)Singleton<GameJson>.Instance.CommonJson[0].Value;
			MaxWeaponState maxWeaponState = (MaxWeaponState)GameGlobalData.MaxWeaponState;
			if (maxWeaponState == MaxWeaponState.Free)
			{
				maxWeaponState = MaxWeaponState.UnUsed;
			}
			bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetMaxWeaponState");
			if (maxWeaponState == MaxWeaponState.HasBeenAdUse)
			{
				maxWeaponState = MaxWeaponState.UnUsed;
				GameGlobalData.SetWeaponID(GameGlobalData.PreWeaponID);
				Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
				GameGlobalData.SetTempWeaponID(-1);
			}
			if (maxWeaponState == MaxWeaponState.UnUsed)
			{
				int num = Random.Range(0, 100);
				maxWeaponState = ((num <= _maxLevelUseRate) ? MaxWeaponState.Free : MaxWeaponState.UnUsed);
			}
			else
			{
				if (maxWeaponState == MaxWeaponState.HasBeenFree)
				{
					GameGlobalData.SetWeaponMaxLevelFree(GameGlobalData.TempWeaponID);
					GameGlobalData.SetWeaponID(GameGlobalData.PreWeaponID);
					Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
				}
				maxWeaponState = (((maxWeaponState == MaxWeaponState.HasBeenFree || maxWeaponState == MaxWeaponState.AdUse) && flag) ? MaxWeaponState.AdUse : MaxWeaponState.UnUsed);
				int num2 = Random.Range(0, 100);
				maxWeaponState = ((num2 <= _maxLevelUseRate) ? MaxWeaponState.Free : MaxWeaponState.UnUsed);
			}
			int tempWeaponID = GameGlobalData.TempWeaponID;
			if (maxWeaponState != 0)
			{
				tempWeaponID = GetMaxWeaponID();
				if (tempWeaponID < 0)
				{
					maxWeaponState = MaxWeaponState.UnUsed;
				}
				else if (!IsFreeUse(tempWeaponID))
				{
					if (flag)
					{
						maxWeaponState = MaxWeaponState.AdUse;
					}
					else
					{
						maxWeaponState = MaxWeaponState.UnUsed;
						tempWeaponID = -1;
					}
				}
				else
				{
					maxWeaponState = MaxWeaponState.Free;
				}
			}
			else
			{
				tempWeaponID = -1;
			}
			if (tempWeaponID >= 0)
			{
				_preTempWeaponID = tempWeaponID;
			}
			GameGlobalData.SetTempWeaponID(tempWeaponID);
			GameGlobalData.SetMaxWeaponState((int)maxWeaponState);
		}

		public void SetFreeUpGrade()
		{
			if (Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "SetFreeUpGrade") && GameGlobalData.FreeUpGradeIndex == -1)
			{
				if (IsCanFreeUpGrade())
				{
					int num = (GameGlobalData.FailCount < 3) ? 30 : 100;
					if (Random.Range(0, 100) <= num)
					{
						GameGlobalData.FreeUpGradeIndex = _freeUpGradeList[Random.Range(0, _freeUpGradeList.Count)];
					}
				}
			}
			else
			{
				GameGlobalData.FreeUpGradeIndex = -1;
			}
		}

		private bool IsCanFreeUpGrade()
		{
			if (GameGlobalData.WeaponID < 0)
			{
				return false;
			}
			WeaponStruct weaponStruct = Singleton<GameJson>.Instance.WeaponJson[GameGlobalData.WeaponID];
			int weaponLevel = GameGlobalData.GetWeaponLevel(GameGlobalData.WeaponID * 2, 0);
			_freeUpGradeList.Clear();
			int num = 0;
			if (GameGlobalData.FireSpeedLevel >= Singleton<GameJson>.Instance.FireSpeedJson.Length - 1)
			{
				num++;
			}
			else
			{
				_freeUpGradeList.Add(0);
			}
			if (GameGlobalData.FirePowerLevel >= Singleton<GameJson>.Instance.FirePowerJson.Length - 1)
			{
				num++;
			}
			else
			{
				_freeUpGradeList.Add(1);
			}
			if (weaponLevel >= weaponStruct.LevelMax[0] - 1)
			{
				num++;
			}
			else
			{
				_freeUpGradeList.Add(2);
			}
			if (GameGlobalData.CoinCountLevel >= Singleton<GameJson>.Instance.CoinLevelJson.Length - 1)
			{
				num++;
			}
			else
			{
				_freeUpGradeList.Add(4);
			}
			if (num >= 4)
			{
				return false;
			}
			float curCoin = GameGlobalData.CurCoin;
			float num2 = GameUtils.UnitToFloat(GameGlobalData.GetWeaponProp1Cost(GameGlobalData.WeaponID, weaponLevel));
			FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
			FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
			CoinLevelStruct coinLevelData = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinCountLevel);
			if (curCoin >= GameGlobalData.GetFireSpeedCost(fireSpeedData) || curCoin >= GameGlobalData.GetFireCost(firePowerData) || curCoin >= num2 || curCoin >= GameGlobalData.GetCoinValueCost(coinLevelData))
			{
				return false;
			}
			return true;
		}

		public bool IsBossLevelUse()
		{
			LevelStruct levelStruct = Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel];
			if (levelStruct.Type != 2)
			{
				return true;
			}
			if (GameGlobalData.FailCount <= 3)
			{
				if (GameGlobalData.TempWeaponID >= 0 && GameGlobalData.PreWeaponID >= 0)
				{
					GameGlobalData.SetWeaponID(GameGlobalData.PreWeaponID);
					Singleton<PlayerManager>.Instance.ChangeLeaderWeapon();
				}
				GameGlobalData.SetTempWeaponID(-1);
				GameGlobalData.SetMaxWeaponState(0);
				return false;
			}
			return true;
		}

		public void SetOpenWeaopon()
		{
			WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
			int num = 0;
			for (int i = 0; i < weaponJson.Length; i++)
			{
				if (GameGlobalData.CurLevel >= weaponJson[i].UnLockLevel)
				{
					num++;
				}
			}
			if (GameGlobalData.OpenWeaponCount != num)
			{
				GameGlobalData.SetOpenWeaponCount(num);
				SendNotification("PROPERTY_NEW_WEAPON_TIP");
			}
		}

		private int GetMaxWeaponID()
		{
			WeaponStruct[] weaponJson = Singleton<GameJson>.Instance.WeaponJson;
			int[] weaponLevels = GameGlobalData.WeaponLevels;
			int openWeaponCount = GameGlobalData.OpenWeaponCount;
			List<int> list = new List<int>();
			for (int i = 0; i < openWeaponCount; i++)
			{
				if (GameGlobalData.WeaponID == i)
				{
					continue;
				}
				for (int j = 0; j < 2; j++)
				{
					int num = j + i;
					if (weaponLevels[num] < weaponJson[i].LevelMax[j])
					{
						list.Add(i);
						break;
					}
				}
			}
			for (int k = openWeaponCount; k < weaponJson.Length; k++)
			{
				list.Add(k);
			}
			for (int num2 = list.Count - 1; num2 >= 0; num2--)
			{
				if (list[num2] == _preTempWeaponID)
				{
					list.Remove(_preTempWeaponID);
					break;
				}
			}
			if (list.Count == 0)
			{
				return -1;
			}
			return list[Random.Range(0, list.Count)];
		}

		private bool IsFreeUse(int weaponID)
		{
			return GameGlobalData.WeaponMaxLevelFree[weaponID] == 0;
		}
	}
}

using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace orangesGame
{
	public class MainProxy : Proxy, IProxy
	{
		public Vector2 upgradeCoinEffPos;

		private const int STEPS = 1;

		private IEffectAction _upgradeEff;

		private int _weaponIndex;

		[CompilerGenerated]
		private static Action<int> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Action<int> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Action<int> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Action<int> _003C_003Ef__mg_0024cache3;

		public MainProxy(string NAME)
			: base(NAME)
		{
			_upgradeEff = new UpGradeEffect();
		}

		public void MainUpGradeByFree(object type)
		{
			UpGradeVO mainUpGradeVO = GetMainUpGradeVO(type);
			SetFreeUpGrade();
			MainUpGrade(mainUpGradeVO, isADS: true);
		}

		public void MainUpGradeByCoin(object type)
		{
			UpGradeVO mainUpGradeVO = GetMainUpGradeVO(type);
			GameGlobalData.AddCurCoin(0f - mainUpGradeVO.cost);
			MainUpGrade(mainUpGradeVO, isADS: false);
			GameAnalysisData.CostGold((long)(0f - mainUpGradeVO.cost), ((int)type != 0) ? "mainfire" : "mainspeed");
		}

		public void WeaponUpGradeByFree(object type)
		{
			UpGradeVO vo = WeaponUpgrade(type);
			SetFreeUpGrade();
			WeaponUpGarde(vo, isADS: true);
		}

		public void WeaponUpGradeByCoin(object type)
		{
			UpGradeVO vo = WeaponUpgrade(type);
			GameGlobalData.AddCurCoin(0f - vo.cost);
			WeaponUpGarde(vo, isADS: false);
			GameAnalysisData.CostGold((long)(0f - vo.cost), ((int)type != 0) ? "weaponfire" : "weaponspeed");
		}

		public void CoinUpGradeByCoin(object type)
		{
			UpGradeVO vo = CoinUpGrade(type);
			GameGlobalData.AddCurCoin(0f - vo.cost);
			CoinUpGrade(vo, isADS: false);
			GameAnalysisData.CostGold((long)(0f - vo.cost), ((int)type != 0) ? "offlineup" : "goldup");
		}

		public void CoinUpGradeByFree(object type)
		{
			UpGradeVO vo = CoinUpGrade(type);
			SetFreeUpGrade();
			CoinUpGrade(vo, isADS: true);
		}

		public void SetFreeUpGrade()
		{
			GameGlobalData.FreeUpGradeIndex = -1;
			GameGlobalData.FreeUpGradeCount = 2;
		}

		private void MainUpGrade(UpGradeVO vo, bool isADS)
		{
			Singleton<AudioManager>.Instance.PlaySound("levelGrade");
			ShowEffect();
			vo.onUpGrade(1);
			SendNotification("TASK_UPGRADE");
			SendMainDataAnalysis(isADS, vo);
		}

		private void CoinUpGrade(UpGradeVO vo, bool isADS)
		{
			Singleton<AudioManager>.Instance.PlaySound("levelGrade");
			ShowEffect(upgradeCoinEffPos, 2f);
			vo.onUpGrade(1);
			SendNotification("TASK_UPGRADE");
			SendCoinDataAnalysis(isADS, vo);
		}

		private void WeaponUpGarde(UpGradeVO vo, bool isADS)
		{
			Singleton<AudioManager>.Instance.PlaySound("levelGrade");
			Player leaderObj = Singleton<PlayerManager>.Instance.GetLeaderObj();
			ShowEffect();
			GameGlobalData.SetWeaponLevel(1, GameGlobalData.WeaponID, vo.weaponIndex);
			SendNotification("TASK_UPGRADE");
			SendWeaponDataAnalysis(isADS, vo);
		}

		private void ShowEffect()
		{
			Transform leader = Singleton<PlayerManager>.Instance.GetLeader();
			Vector3 localPosition = leader.localPosition;
			ShowEffect(new Vector3(0f, localPosition.y, 0f), 3.5f);
		}

		private void ShowEffect(Vector3 pos, float scale)
		{
			_upgradeEff.Spawn(pos, Vector3.one * scale);
		}

		private UpGradeVO GetMainUpGradeVO(object type)
		{
			UpGradeVO result = default(UpGradeVO);
			result.weaponIndex = (int)type;
			switch (result.weaponIndex)
			{
			case 0:
			{
				FireSpeedStruct fireSpeedData = GameGlobalData.GetFireSpeedData();
				result.onUpGrade = GameGlobalData.SetFireSpeed;
				result.cost = GameUtils.UnitToFloat(GameUtils.FloatToUnit(GameGlobalData.GetFireSpeedCost(fireSpeedData)));
				break;
			}
			case 1:
			{
				FirePowerStruct firePowerData = GameGlobalData.GetFirePowerData();
				result.onUpGrade = GameGlobalData.SetFirePower;
				result.cost = GameUtils.UnitToFloat(GameUtils.FloatToUnit(GameGlobalData.GetFireCost(firePowerData)));
				break;
			}
			}
			return result;
		}

		private UpGradeVO CoinUpGrade(object type)
		{
			UpGradeVO result = default(UpGradeVO);
			result.weaponIndex = (int)type;
			switch (result.weaponIndex)
			{
			case 0:
			{
				CoinLevelStruct coinLevelData2 = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinCountLevel);
				result.onUpGrade = GameGlobalData.SetCoinCountLevel;
				result.cost = GameGlobalData.GetCoinValueCost(coinLevelData2);
				break;
			}
			case 1:
			{
				CoinLevelStruct coinLevelData = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinOffLineLevel);
				result.onUpGrade = GameGlobalData.SetCoinOffLineLevel;
				result.cost = GameGlobalData.GetOfflineValueCost(coinLevelData);
				break;
			}
			}
			return result;
		}

		private UpGradeVO WeaponUpgrade(object type)
		{
			UpGradeVO result = default(UpGradeVO);
			int num = (int)type;
			int weaponLevel = GameGlobalData.GetWeaponLevel(GameGlobalData.WeaponID * 2, num);
			WeaponLevelStruct[] weaponLevelJson = Singleton<GameJson>.Instance.WeaponLevelJson;
			switch (GameGlobalData.WeaponID)
			{
			case 0:
				result.cost = ((num != 0) ? GameGlobalData.GetWeaponFireCostTxt(0, weaponLevelJson[weaponLevel]) : GameGlobalData.GetWeaponScaleCostTxt(0, weaponLevelJson[weaponLevel]));
				break;
			case 1:
				result.cost = ((num != 0) ? GameGlobalData.GetWeaponFireCostTxt(1, weaponLevelJson[weaponLevel]) : GameGlobalData.GetWeaponScaleCostTxt(1, weaponLevelJson[weaponLevel]));
				break;
			case 2:
				result.cost = ((num != 0) ? GameGlobalData.GetWeaponFireCostTxt(2, weaponLevelJson[weaponLevel]) : GameGlobalData.GetWeaponScaleCostTxt(2, weaponLevelJson[weaponLevel]));
				break;
			case 3:
				result.cost = ((num != 0) ? GameGlobalData.GetWeaponFireCostTxt(3, weaponLevelJson[weaponLevel]) : GameGlobalData.GetWeaponScaleCostTxt(3, weaponLevelJson[weaponLevel]));
				break;
			case 4:
				result.cost = ((num != 0) ? GameGlobalData.GetWeaponFireCostTxt(4, weaponLevelJson[weaponLevel]) : GameGlobalData.GetWeaponScaleCostTxt(4, weaponLevelJson[weaponLevel]));
				break;
			}
			result.weaponIndex = num;
			return result;
		}

		private void SendMainDataAnalysis(bool isVedio, UpGradeVO vo)
		{
			string item = string.Empty;
			string format = string.Empty;
			string item2 = string.Empty;
			string path = string.Empty;
			int num = 0;
			Property weaponIndex = (Property)vo.weaponIndex;
			switch (weaponIndex)
			{
			case Property.FireSpeed:
				item = "type@射速";
				format = "cost@{0}";
				item2 = "type@射速升级";
				path = "levelup_main_1";
				num = GameGlobalData.FireSpeedLevel - 1;
				break;
			case Property.FirePower:
				item = "type@火力";
				format = "cost@{0}";
				item2 = "type@火力升级";
				path = "levelup_main_2";
				num = GameGlobalData.FirePowerLevel - 1;
				break;
			}
			List<string> list = UmConfig.SetCommonParam();
			list.Add(item);
			list.Add(string.Format(format, vo.cost));
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Grownup", list.ToArray());
			if (isVedio)
			{
				List<string> list2 = UmConfig.SetCommonParam();
				list2.Add(item2);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list2.ToArray());
			}
			if (isVedio)
			{
				GameAnalysisData.SendShowVedio(path);
			}
			GameAnalysisData.SendUpgrade(weaponIndex.ToString(), isVedio, 0);
		}

		private void SendWeaponDataAnalysis(bool isVedio, UpGradeVO vo)
		{
			string item = string.Empty;
			string format = string.Empty;
			string item2 = string.Empty;
			Property property = Property.FireSpeed;
			string path = string.Empty;
			switch (vo.weaponIndex)
			{
			case 0:
				item = "type@射速";
				format = "cost@{0}";
				item2 = "type@副武器射速升级";
				property = Property.FireSpeed;
				path = "weaponlevelup1";
				break;
			case 1:
				item = "type@火力";
				format = "cost@{0}";
				item2 = "type@副武器火力升级";
				property = Property.FirePower;
				path = "weaponlevelup2";
				break;
			}
			List<string> list = UmConfig.SetCommonParam();
			list.Add(item);
			list.Add(string.Format(format, vo.cost));
			list.Add($"id@{GameGlobalData.WeaponID}");
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GrownupWeapon", list.ToArray());
			if (isVedio)
			{
				List<string> list2 = UmConfig.SetCommonParam();
				list2.Add(item2);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list2.ToArray());
			}
			if (isVedio)
			{
				GameAnalysisData.SendShowVedio(path);
			}
			GameAnalysisData.SendUpgrade(property.ToString(), isVedio, GameGlobalData.WeaponID + 1);
		}

		private void SendCoinDataAnalysis(bool isVedio, UpGradeVO vo)
		{
			string item = string.Empty;
			string item2 = string.Empty;
			string format = string.Empty;
			Property property = Property.Gold;
			string path = string.Empty;
			switch (vo.weaponIndex)
			{
			case 0:
				item = "type@金币";
				format = "cost@{0}";
				item2 = "type@金币系统升级";
				property = Property.Gold;
				path = "goldup1";
				break;
			case 1:
				item = "type@离线";
				format = "cost@{0}";
				item2 = "type@离线升级";
				property = Property.OffLine;
				path = "goldup2";
				break;
			}
			List<string> list = UmConfig.SetCommonParam();
			list.Add(item);
			list.Add(string.Format(format, vo.cost));
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("Grownup", list.ToArray());
			if (isVedio)
			{
				List<string> list2 = UmConfig.SetCommonParam();
				list2.Add(item2);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoReward", list2.ToArray());
			}
			if (isVedio)
			{
				GameAnalysisData.SendShowVedio(path);
			}
			GameAnalysisData.SendUpgrade(property.ToString(), isVedio, 0);
		}
	}
}

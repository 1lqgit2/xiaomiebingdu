using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public static class GameAnalysisData
	{
		public static float TotalGameTime;

		public static float StartGameTime;

		public static bool UseRetry;

		public static void AddTotalGameTime(string path)
		{
			TotalGameTime += Time.unscaledTime - StartGameTime;
			StartGameTime = Time.unscaledTime;
		}

		public static int GetTotalGameTime()
		{
			float totalGameTime = TotalGameTime;
			totalGameTime = ((!(totalGameTime > 600f)) ? totalGameTime : 600f);
			return (int)totalGameTime;
		}

		private static Dictionary<string, string> GetBase(bool needWeapon)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();
			if (needWeapon)
			{
				data = GetWeaponLv(data);
			}
			data = GetMainWeaponLv(data);
			data = GetGoldAndOfflineLv(data);
			data["LEVEL"] = (GameGlobalData.CurLevel + 1).ToString();
			data["CUR_GOLD"] = GameGlobalData.LongCoin.ToString();
			data["FailCount"] = GameGlobalData.FailCount.ToString();
			data["Diamond"] = ((long)GameGlobalData.CurDiamond).ToString();
			return data;
		}

		private static Dictionary<string, string> GetWeaponLv(Dictionary<string, string> data)
		{
			int[] weaponLevels = GameGlobalData.WeaponLevels;
			int num;
			for (num = 0; num < weaponLevels.Length; num++)
			{
				int num2 = num + 1;
				data["SECOND_WEAPON_" + num2 + "_LEVEL"] = weaponLevels[num].ToString();
				data["SECOND_WEAPON_" + num2 + "_FIRE"] = weaponLevels[num + 1].ToString();
				num++;
			}
			return data;
		}

		private static Dictionary<string, string> GetMainWeaponLv(Dictionary<string, string> data)
		{
			data["MAIN_WEAPON_LEVEL"] = GameGlobalData.FireSpeedLevel.ToString();
			data["MAIN_WEAPON_FIRE"] = GameGlobalData.FirePowerLevel.ToString();
			return data;
		}

		private static Dictionary<string, string> GetGoldAndOfflineLv(Dictionary<string, string> data)
		{
			data["CoinCountLevel"] = GameGlobalData.CoinCountLevel.ToString();
			data["CoinOffLineLevel"] = GameGlobalData.CoinOffLineLevel.ToString();
			return data;
		}

		public static void SendLoginStart(int newGuide)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["NewGuide"] = newGuide.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("SessionStart", dictionary);
		}

		public static void SendLoginSuc(int newGuide, int time)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["NewGuide"] = newGuide.ToString();
			@base["Time"] = time.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("SessionStartSuc", @base);
		}

		public static void SendStartBattle(int newGuide, int canUpdateMain, int canUpdateCoin, int canUpdateWeapon)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["NewGuide"] = newGuide.ToString();
			@base["CanUpateMain"] = canUpdateMain.ToString();
			@base["CanUpateCoin"] = canUpdateCoin.ToString();
			@base["CanUpateWeapon"] = canUpdateWeapon.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("LevelStart", @base);
			CheckCoinLv();
			CheckSpeedLv();
		}

		public static void SendSoundState(int state)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["Sound"] = state.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("SoundState", dictionary);
		}

		public static void SendShakeState(int state)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["Shake"] = state.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShakeState", dictionary);
		}

		public static void SendEndBattle(int suc, float process)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: false);
			@base["SUCCESS"] = suc.ToString();
			@base["SCORE"] = GameGlobalData.HitCountHp.ToString();
			@base["DPS"] = Singleton<PlayerManager>.Instance.GetOutputFire().ToString();
			@base["VALUE"] = GetTotalGameTime().ToString();
			if (process == 1f)
			{
				@base["PROCESS"] = "1.0";
			}
			else if (process == 0f)
			{
				@base["PROCESS"] = "0.0";
			}
			else
			{
				@base["PROCESS"] = process.ToString();
			}
			@base["UseRetry"] = ((!UseRetry) ? "0" : "1");
			@base["MaxWeapon"] = ((GameGlobalData.TempWeaponID != -1) ? "1" : "0");
			@base["GOTCOIN"] = GameGlobalData.CurGetCoin.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("LevelEnd", @base);
			SendWeaponLv();
			SendCoinRate();
		}

		private static void SendWeaponLv()
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("LevelEndForWeaponLv", @base);
		}

		public static void SendRetry(float process)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			if (process == 1f)
			{
				@base["PROCESS"] = "1.0";
			}
			else if (process == 0f)
			{
				@base["PROCESS"] = "0.0";
			}
			else
			{
				@base["PROCESS"] = process.ToString();
			}
			@base["SCORE"] = GameGlobalData.HitCountHp.ToString();
			@base["DPS"] = Singleton<PlayerManager>.Instance.GetOutputFire().ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("Retry", @base);
		}

		public static void SendShowVedio(string path)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["Path"] = path;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShowVedio", @base);
		}

		public static void SendUpgrade(string path, bool ads, int weaponId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["Path"] = path;
			@base["ADS"] = ((!ads) ? "0" : "1");
			@base["WEAPONID"] = weaponId.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("Upgrade", @base);
		}

		public static void SendGetOffline(bool ads, int getGold)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ADS"] = ((!ads) ? "0" : "1");
			@base["GETGOLD"] = getGold.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("GetOffline", @base);
		}

		public static void SendMaxWeapon(bool ads, int weaponId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ADS"] = ((!ads) ? "0" : "1");
			@base["WEAPONID"] = weaponId.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("MaxWeapon", @base);
		}

		public static void GetGold(long coin, string path)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["GetCoin"] = coin.ToString();
			@base["GetPath"] = path;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("GetGold", @base);
		}

		public static void CostGold(long coin, string path)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["CostCoin"] = coin.ToString();
			@base["CosPath"] = path;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("CostGold", @base);
		}

		public static void SendPerformanceData(float fps, float totalMemory)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["TotalMemory"] = totalMemory.ToString();
			dictionary["fps"] = fps.ToString();
			dictionary["level"] = (GameGlobalData.CurLevel + 1).ToString();
			dictionary["time"] = Time.realtimeSinceStartup + "s";
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("PerformanceData", dictionary);
		}

		private static void SendCoinRate()
		{
			if (GameGlobalData.IsGameManagerTest)
			{
				Dictionary<string, string> @base = GetBase(needWeapon: true);
				@base["CoinRate"] = GameGlobalData.GotCoinInGame();
				@base["gaid"] = GameGlobalData.GetGaid;
				Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("CoinCountRate", @base);
			}
		}

		public static void UserEvaluation(int status, int starLevel, string content)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["Status"] = status.ToString();
			@base["StarLevel"] = starLevel.ToString();
			@base["Content"] = content;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("UserEvaluationNew", @base);
		}

		public static void ShareClickEvent()
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShareClickEvent", @base);
		}

		public static void ShareSuccessEvent()
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShareSuccessEvent", @base);
		}

		public static void FinishMission(int id)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["MissionId"] = id.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("NewFinishMission", @base);
		}

		public static void GetMissionReward(int id, int multipleTimes)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["MissionId"] = id.ToString();
			@base["MultipleTimes"] = multipleTimes.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("NewGetMission", @base);
		}

		public static void ADSIsLoad(string path, bool isSuc)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["Path"] = path;
			@base["IsSuc"] = ((!isSuc) ? "0" : "1");
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ADSIsLoad", @base);
		}

		public static void DiamondRelive(int times, long oldDiamond, int cost)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ReliveTimes"] = times.ToString();
			@base["OldDiamond"] = oldDiamond.ToString();
			@base["Cost"] = cost.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("DiamondRelive", @base);
		}

		public static void Share(string approach)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["Approach"] = approach;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShareApproach", @base);
		}

		public static void ShareCallBack(bool isSuccess)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["IsSuccess"] = isSuccess.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShareCallBack", @base);
		}

		public static void ShowInterstitial(string path)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["Path"] = path;
			@base["ShowVideoTimes"] = GameGlobalData.ShowVideoTimes.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShowInner", @base);
		}

		public static void DiamondExchange(long diamondNum, long oldDiamondNum, long gold, long oldGold)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["DiamondNum"] = diamondNum.ToString();
			@base["OldDiamondNum"] = oldDiamondNum.ToString();
			@base["Gold"] = gold.ToString();
			@base["OldGold"] = oldGold.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("DiamondExchange", @base);
		}

		public static void GotDiamondByVideo()
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("GotDiamondByVideo", @base);
		}

		public static void ShareAndGotDiamond(int diamondNum, long oldDiamond)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["DiamondNum"] = diamondNum.ToString();
			@base["OldDiamond"] = oldDiamond.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShareAndGotDiamond", @base);
		}

		public static void BuyDiamondStorage(int diamondNum)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["DiamondNum"] = diamondNum.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyDiamondStorage", @base);
		}

		public static void ShowVideoDiamondStorage(int diamondNum)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["DiamondNum"] = diamondNum.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShowVideoDiamondStorage", @base);
		}

		public static void BuyFromShopStart(string shopId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ShopId"] = shopId;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyFromShopStart", @base);
		}

		public static void BuyFromShopEnd(string shopId, string orderId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ShopId"] = shopId;
			@base["OrderId"] = orderId;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyFromShopEnd", @base);
		}

		public static void BuyFromShop(string shopId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ShopId"] = shopId;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyFromShop", @base);
		}

		public static void BuyFailFromShop(string shopId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ShopId"] = shopId;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyFailFromShop", @base);
		}

		public static void BuyFromSubscribeStart()
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyFromSubscribeStart", @base);
		}

		public static void BuyFromSubscribeEnd(string orderId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["orderId"] = orderId;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyFromSubscribeEnd", @base);
		}

		public static void BuyItem(int needDiamond, int num, int itemId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["NeedDiamond"] = needDiamond.ToString();
			@base["Num"] = num.ToString();
			@base["ItemId"] = itemId.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("BuyItem", @base);
		}

		public static void UseItem()
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			int[] useItems = Singleton<BattleItemManager>.Instance.GetUseItems();
			if (useItems[0] != 0 || useItems[1] != 0)
			{
				for (int i = 0; i < useItems.Length; i++)
				{
					@base["item" + i] = useItems[i].ToString();
				}
				Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("UseItem", @base);
			}
		}

		public static void ShowVideoBuyDiamond(int diamondId, int showVedioTimes, int got)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["diamondId"] = diamondId.ToString();
			@base["showVedioTimes"] = showVedioTimes.ToString();
			@base["got"] = got.ToString();
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ShowVideoBuyDiamond", @base);
		}

		public static void DrawItem(string itemType, string AddNums)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["itemType"] = itemType;
			@base["AddNums"] = AddNums;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("DrawItem", @base);
		}

		public static void GooglePaySuccessNullOrderId(string shopId)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["ShopId"] = shopId;
			Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("PaySucNullOrderId", @base);
		}

		private static void CheckCoinLv()
		{
			CoinLevelStruct coinLevelData = GameGlobalData.GetCoinLevelData(GameGlobalData.CoinCountLevel);
			float num = GameUtils.UnitToFloat(coinLevelData.CoinCountCost);
			if (!(GameGlobalData.LongCoin < (double)(num * 2f)))
			{
				long num2 = (long)(GameGlobalData.LongCoin / (double)num);
				Dictionary<string, string> @base = GetBase(needWeapon: true);
				@base["rate"] = num2.ToString();
				Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("CheckCoinLv", @base);
			}
		}

		private static void CheckSpeedLv()
		{
			float fireSpeedCost = GameGlobalData.GetFireSpeedCost(GameGlobalData.GetFireSpeedData());
			if (!(GameGlobalData.LongCoin < (double)(fireSpeedCost * 6f)))
			{
				long num = (long)(GameGlobalData.LongCoin / (double)fireSpeedCost);
				Dictionary<string, string> @base = GetBase(needWeapon: true);
				@base["rate"] = num.ToString();
				Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("CheckSpeedLv", @base);
			}
		}

		public static void ClickAndShowView(string viewName)
		{
			Dictionary<string, string> @base = GetBase(needWeapon: true);
			@base["viewName"] = viewName;
			//Singleton<AnalyzeSdk>.Instance.FireBaseOnEvent("ClickAndShowView", @base);
		}
	}
}

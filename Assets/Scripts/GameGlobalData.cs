using orangesGame;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameGlobalData
{
	public const string AD_APPKEY = "1000990";

	public const string AD_SECRET = "5S2Rx2yQKM5yzkOgHGJmuw==";

	public const string AD_CHANNEL = "releaseAndoird";

	public const string AD_BANNER = "";

	public const string AD_INTER = "35eeefa8-c011-4112-bc9b-4a04fe3c413c";

	public const string AD_VEDIO = "0a2cad70-9e1c-450f-bcdc-f3798b324b95";

	public const string AD_VEDIO_2 = "0a2cad70-9e1c-450f-bcdc-f3798b324b95";

	public const string AD_UMENKEY = "5c80cb16203657f48e0003b9";

	public static string gamePackName = "com.ihippogame.bioblast";

	public static bool IsGameManagerTest = false;

	public static bool WithOutSDK = false;

	public const float GAME_TIMESCALE = 1f;

	public const float GAME_TIMESCALE_SLOW = 0.1f;

	public static readonly Vector3 Player_InitPos = new Vector3(0f, -2.5f, 0f);

	public const float SCENE_HEIGHT = 19.2f;

	public const float SCENE_WIDE = 10.8f;

	public const float SCENE_HEIGHT_HALF = 9.6f;

	public const float SCENE_WIDE_HALF = 5.4f;

	public const int BALL_OFFSET_Z = 2;

	public const float BALL_INIT_Y = 10.2f;

	public static readonly Vector2 BarrierInitPos = new Vector2(0f, 10.2f);

	public const float BALL_INIT_Y2 = 10f;

	public const float BALL_INIT_Y3 = 9.3f;

	public const float FIRE_SPEED_MAX1 = 0.05f;

	public const float FIRE_SPEED_MAX2 = 0.05f;

	public static int BarrierBackAngleMin = 150;

	public static int BarrierBackAngleMax = 180;

	public static int BarrierInitAngleMin = 170;

	public static int BarrierInitAngleMax = 190;

	public static int BarrierDivideAngleMin = 30;

	public static int BarrierDivideAngleMax = 80;

	public static int BarrierGreenBallAngleMin = -40;

	public static int BarrierGreenBallAngleMax = 40;

	public const int NO = 0;

	public const int YES = 1;

	public const float ITEM_AUTOKILL_TIME = 10f;

	public const float ITEM_CD = 6f;

	public const float BOSS_TIPS_CD = 4f;

	public const int DEAD_LINE_HOUR = 5;

	public static int Addfirespeed = 0;

	public static int Bullettimes = 1;

	public static float BGM_VOLUME = 1f;

	public static float SOUND_VOLUME = 1f;

	public static int CurLevel = 0;

	public static float CurCoin = 0f;

	public static float CurDiamond = 0f;

	public static int CurHeart = 1;

	public static float CurGetCoin = 0f;

	public static int FailCount = 0;

	public static int HitCount = 0;

	public static int ShowValue = 0;

	public static int PlayerType = 0;

	public static int TutorialState = 0;

	public static int OffLineTime = 86400;

	public static int ShowOfflineAdsTime = 1200;

	public static int OffLineCoin = 0;

	public static int DropItemsInLevel = 0;

	public static long OfflineTimeRecord = 0L;

	public static string GetDropItemsInGame = string.Empty;

	public static int ShakeFlag = 0;

	public static int SlotProgress = 0;

	public const int SLOT_MAXPROGRESS = 5;

	public static GameTestState GameTest = GameTestState.NoTest;

	public static int isCanUpadteMain = 0;

	public static int isCanUpadteCoin = 0;

	public static int isCanUpadteWeapon = 0;

	public static int IsEvaluate = 0;

	public static int ActualLevelReject = -1;

	public static int TaskLock = 0;

	public static string TaskFirePowerCost = "0";

	public static int TaskRefresh = 0;

	public static long TaskTime = 0L;

	public static long RefreshTime = 0L;

	public static ShopPrefsData ShopData;

	public static NpcPrefsData NpcData;

	public static BattleItemPrefsData BattleItemData;

	public static Vector2 MainViewPropBtnPos;

	public const string REVIVE_STATE_KEY = "retry_limited";

	public static ReviveState ReviveType = ReviveState.One;

	public static int ReviveCountMax = 1;

	public static int ReviveCount = 0;

	public static bool ShareInCd = true;

	public static float ShareCdTime = 6f;

	private static long _shareDataTime = -1L;

	public static int FreeUpGradeIndex = -1;

	public static int FreeUpGradeCount = 0;

	public static float WeakenVenomSpeedFactor = 1f;

	public static float EnhanceVenomSpeedFactor = 1f;

	public static float ExpandVenomScaleFactor = 1f;

	public static string PlayerBulletResPath = "GameObject/Bullet/bullet_main1";

	public static BulletType PlayerBulletType = BulletType.SmallBullet;

	public static float EnhanceBulletDamageFactor = 1f;

	public static bool IsPlayerBulletEnhanced = false;

	public static bool IsPlayerCoinBullet = false;

	public static List<int> CoinInCurrentLevel = new List<int>();

	public static List<CoinManager.Type> ItemsInCurrentLevel = new List<CoinManager.Type>();

	public static int WeaponID = -1;

	public static int PreWeaponID = -1;

	public static int TempWeaponID = -1;

	public static int MaxWeaponState = 0;

	public static int OpenWeaponCount = 0;

	public static int[] WeaponLevels = new int[10];

	public static int[] WeaponMaxLevelFree = new int[5]
	{
		1,
		1,
		1,
		1,
		1
	};

	public static float MoveDistance = 0f;

	public static int NeedHitCount = 0;

	public static int GameEndTime = 0;

	public static LevelType LevelType = LevelType.Common;

	public static bool IsGamePause = false;

	public static bool IsGameAuto = false;

	public static bool IsMusice = true;

	public static bool IsDie = false;

	public static bool IsHurt = false;

	public static GameState GameState = GameState.Main;

	public static bool IsLayerShake = false;

	public static Vector3 ClickStartPos = Vector3.zero;

	public static Vector2 GameItemHidePos = Vector2.one * 5000f;

	public static Vector2 CoinRecoverPos;

	public static Vector2 CoinRecoverTopLeftPos;

	public static Vector2 DiamondRecoverTopLeftPos;

	public static Vector3 PropsShadowScale = new Vector3(7.65f, 1f, 1f);

	public static bool IsFirstStart = true;

	public static int ItemType_Weapon_H_Record = 20;

	public static Vector2 TouchPos;

	public static int ChipEffectCount = 0;

	public static long curTime;

	public static float HitCountHp;

	public static string[] VenomDieSounds = new string[5]
	{
		"bd1",
		"bd2",
		"bd3",
		"bd4",
		"bd5"
	};

	public static float BallRedValue;

	public static Color[] BallColors = new Color[9]
	{
		new Color(43f / 51f, 1f, 0.117647059f),
		new Color(12f / 85f, 1f, 0.117647059f),
		new Color(0.117647059f, 1f, 40f / 51f),
		new Color(0.117647059f, 67f / 85f, 1f),
		new Color(71f / 255f, 7f / 15f, 1f),
		new Color(8f / 15f, 64f / 255f, 1f),
		new Color(196f / 255f, 0.235294119f, 1f),
		new Color(1f, 0.235294119f, 62f / 85f),
		new Color(1f, 0.235294119f, 25f / 51f)
	};

	public static int GradeState = 0;

	public const int GRADE_LEVEL = 3;

	public static string GARDE_URL = "https://play.google.com/store/apps/details?id=com.fisher.virusblast";

	public static string APPSTORE_URL = "https://itunes.apple.com/us/app/shooting-jet/id1457556007?l=zh&ls=1&mt=8";

	public static int UpGradeFireSpeedCount = 0;

	public static int UpGradeFireSpeedMax = 1;

	public static int UpGradeFirePowerCount = 0;

	public static int UpGradeFirePowerMax = 1;

	public static int UpGradeOfflineCount = 0;

	public static int UpGradeOfflineMax = 1;

	public static long ADSTimestamp = 0L;

	public static int remainADSTime = 0;

	public static int ShowVideoTimes = 0;

	public static bool startGame = false;

	public const string PLAYER_RUN = "Run";

	public const string PLAYER_STAND = "Stand";

	public const string PLAYER_DIE = "Die";

	private static bool isInitADS = false;

	public static int FireSpeedLevel = 0;

	public static int FirePowerLevel = 0;

	public static int CoinCountLevel = 0;

	public static int CoinOffLineLevel = 0;

	public const string ITEM_TYPE_INAPP = "inapp";

	public const string ITEM_TYPE_SUBS = "subs";

	public const string DIAMOND_GIFT_1 = "diamond_1";

	public const string DIAMOND_GIFT_2 = "diamond_2";

	public const string DIAMOND_GIFT_3 = "diamond_3";

	public const string DIAMOND_GIFT_4 = "diamond_4";

	public const string DIAMOND_GIFT_5 = "diamond_5";

	public const string DIAMOND_GIFT_6 = "diamond_6";

	public const string TIME_GIFT_1 = "time_1";

	public const string TIME_GIFT_2 = "time_2";

	public const string TIME_GIFT_3 = "time_3";

	public const string TIME_GIFT_4 = "time_4";

	public const string TIME_GIFT_5 = "time_5";

	public const string ITEM_GIFT_1 = "item_1";

	public const string ITEM_GIFT_2 = "item_2";

	public const string ITEM_GIFT_3 = "item_3";

	public const string HOLD_UP_VESSEL = "hold_up_vessel";

	public const string NOADS = "no_ads";

	public const string VIP_SUBS = "diamond_membership";

	public static Dictionary<string, string> GoogleSkuIdDic = new Dictionary<string, string>
	{
		{
			"diamond_1",
			"diamond_1"
		},
		{
			"diamond_2",
			"diamond_2"
		},
		{
			"diamond_3",
			"diamond_3"
		},
		{
			"diamond_4",
			"diamond_4"
		},
		{
			"diamond_5",
			"diamond_5"
		},
		{
			"diamond_6",
			"diamond_6"
		},
		{
			"no_ads",
			"no_ads"
		},
		{
			"diamond_membership",
			"diamond_membership"
		},
		{
			"time_1",
			"time_1"
		},
		{
			"time_2",
			"time_2"
		},
		{
			"time_3",
			"time_3"
		},
		{
			"time_4",
			"time_4"
		},
		{
			"time_5",
			"time_5"
		},
		{
			"item_1",
			"item_1"
		},
		{
			"item_2",
			"item_2"
		},
		{
			"item_3",
			"item_3"
		},
		{
			"hold_up_vessel",
			"hold_up_vessel"
		}
	};

	public static int ActualLevel => CurLevel + 1;

	public static double LongCoin => CurCoin;

	public static long ShareDataTime
	{
		get
		{
			_shareDataTime = ((_shareDataTime != -1) ? _shareDataTime : long.Parse(PlayerPrefs.GetString("ShareDateTime", _shareDataTime.ToString())));
			return _shareDataTime;
		}
		set
		{
			_shareDataTime = value;
			PlayerPrefs.SetString("ShareDateTime", _shareDataTime.ToString());
		}
	}

	public static string GetGaid
	{
		get
		{
			if (WithOutSDK)
			{
				return string.Empty;
			}
			return Singleton<SdkUtils>.Instance.GetGaID();
		}
	}

	public static int ActualFireSpeedLevel => FireSpeedLevel + 1;

	public static int ActualFirePowerLevel => FirePowerLevel + 1;

	public static void UpdateOfflineTimeRecord(bool clean)
	{
		if (clean)
		{
			ResetOfflineTimeRecord();
		}
		else if (OfflineTimeRecord == 0)
		{
			ResetOfflineTimeRecord();
		}
	}

	private static void ResetOfflineTimeRecord()
	{
		OfflineTimeRecord = GetTimestamp();
		PlayerPrefs.SetString("OfflineTimeRecord", OfflineTimeRecord.ToString());
	}

	public static long GetOfflineTimeRecord()
	{
		if (OfflineTimeRecord == 0)
		{
			ResetOfflineTimeRecord();
		}
		return OfflineTimeRecord;
	}

	private static int GetOutOfflineGold(float value)
	{
		long outOfflineTime = GetOutOfflineTime();
		float num = Mathf.Ceil((float)outOfflineTime / 10f);
		return Mathf.RoundToInt(num * value);
	}

	public static int MaxGetOutOfflineGold(int num, float value)
	{
		int outOfflineGold = GetOutOfflineGold(value);
		int num2 = Mathf.FloorToInt(OffLineTime / 10);
		int num3 = Mathf.FloorToInt((float)num2 * value);
		int num4 = outOfflineGold + num;
		if (num4 >= num3)
		{
			return num3;
		}
		return num4;
	}

	public static long GetOutOfflineTime()
	{
		return (GetTimestamp() - GetOfflineTimeRecord()) / 1000;
	}

	public static string GotItemInGame()
	{
		if (ItemsInCurrentLevel == null || ItemsInCurrentLevel.Count == 0)
		{
			return string.Empty;
		}
		string text = string.Empty;
		foreach (CoinManager.Type item in ItemsInCurrentLevel)
		{
			text = text + (int)item + ",";
		}
		return text;
	}

	public static string GotCoinInGame()
	{
		if (CoinInCurrentLevel == null || CoinInCurrentLevel.Count == 0)
		{
			return string.Empty;
		}
		string text = string.Empty;
		foreach (int item in CoinInCurrentLevel)
		{
			text = text + item + ",";
		}
		return text;
	}

	public static void AddCurGetCoin(int value)
	{
		CurGetCoin += value;
		ApplicationFacade.Instance.SendNotification("SET_TEMPCOIN");
	}

	public static void SetWeaponID(int id, bool isSave = true)
	{
		WeaponID = id;
		if (isSave)
		{
			PlayerPrefs.SetInt("WeaponID", WeaponID);
		}
	}

	public static int GetWeaponLevel(int weapID, int index)
	{
		return WeaponLevels[weapID + index];
	}

	public static void SetWeaponLevel(int change, int weaponID, int index)
	{
		int num = weaponID * 2 + index;
		WeaponLevels[num] += change;
		PlayerPrefs.SetInt("WeaponLevel_" + num, WeaponLevels[num]);
	}

	public static string GetWeaponProp1Cost(int weapID, int level)
	{
		WeaponLevelStruct weaponStruct = Singleton<GameJson>.Instance.WeaponLevelJson[level];
		return GameUtils.FloatToUnit(GetWeaponScaleCostTxt(weapID, weaponStruct));
	}

	public static float GetCurHitCountPro()
	{
		return (float)HitCount / (float)NeedHitCount;
	}

	public static bool IsGraded()
	{
		return GradeState == 1;
	}

	public static void SetGradeState(int value)
	{
		GradeState = value;
		PlayerPrefs.SetInt("GradeState", value);
	}

	public static void AddUpGradeFireSpeedCount(int value)
	{
		UpGradeFireSpeedCount += value;
		SetUpGradeFireSpeedCount(UpGradeFireSpeedCount);
	}

	public static void SetUpGradeFireSpeedCount(int value)
	{
		UpGradeFireSpeedCount = value;
		PlayerPrefs.SetInt("UpGradeFireSpeedCount", UpGradeFireSpeedCount);
	}

	public static void AddUpGradeFirePowerCount(int value)
	{
		UpGradeFirePowerCount += value;
		SetUpGradeFirePowerCount(UpGradeFirePowerCount);
	}

	public static void SetUpGradeFirePowerCount(int value)
	{
		UpGradeFirePowerCount = value;
		PlayerPrefs.SetInt("UpGradeFirePowerCount", UpGradeFirePowerCount);
	}

	public static void AddUpGradeOfflineCount(int value)
	{
		UpGradeOfflineCount += value;
		SetUpGradeOfflineCount(UpGradeOfflineCount);
	}

	public static void SetUpGradeOfflineCount(int value)
	{
		UpGradeOfflineCount = value;
		PlayerPrefs.SetInt("UpGradeOfflineCount", UpGradeOfflineCount);
	}

	public static void SetShowVideoTimes(int times)
	{
		ShowVideoTimes = times;
		PlayerPrefs.SetInt("ShowVideoTimes", ShowVideoTimes);
	}

	public static int getWeaponHFailCount()
	{
		if (CurLevel == 0)
		{
			return 2;
		}
		if (CurLevel == 1)
		{
			return 3;
		}
		if ((CurLevel >= 2) & (CurLevel < 10))
		{
			return 5;
		}
		if ((CurLevel >= 10) & (CurLevel < 20))
		{
			return 7;
		}
		if ((CurLevel >= 20) & (CurLevel < 30))
		{
			return 9;
		}
		return 12;
	}

	public static bool CanShowADSScreen()
	{
		if (Singleton<VipManager>.Instance.VipStatus)
		{
			return false;
		}
		if (ShopData.isNoAds)
		{
			return false;
		}
		if (CurLevel < 20)
		{
			return false;
		}
		if (ADSTimestamp == 0)
		{
			return false;
		}
		long num = GetTimestamp() - ADSTimestamp;
		if (num > 0)
		{
			return true;
		}
		return false;
	}

	public static bool CanShowADSScreenNew()
	{
		if (Singleton<VipManager>.Instance.VipStatus)
		{
			return false;
		}
		if (ShopData.isNoAds)
		{
			return false;
		}
		if (CurLevel < 20)
		{
			return false;
		}
		if (!isInitADS)
		{
			return false;
		}
		if (remainADSTime <= 0)
		{
			return true;
		}
		return false;
	}

	public static void UpdateADSTimestamp(bool isVedio)
	{
		int num = 240000;
		int num2 = 360000;
		if (ShowVideoTimes >= 15)
		{
			num *= 2;
			num2 *= 2;
		}
		if (isVedio)
		{
			ShowVideoTimes++;
			SetShowVideoTimes(ShowVideoTimes);
			if (CurLevel >= 20)
			{
				ADSTimestamp = GetTimestamp() + num2;
			}
		}
		else if (CurLevel >= 20)
		{
			ADSTimestamp = GetTimestamp() + num;
		}
	}

	public static void UpdateADSTime(bool isVedio)
	{
		int num = 240000;
		int num2 = 360000;
		if (ShowVideoTimes >= 15)
		{
			num *= 2;
			num2 *= 2;
		}
		if (isVedio)
		{
			ShowVideoTimes++;
			SetShowVideoTimes(ShowVideoTimes);
			if (CurLevel >= 20)
			{
				remainADSTime = num2;
			}
		}
		else if (CurLevel >= 20)
		{
			remainADSTime = num;
		}
	}

	public static void ReduceADSTime(int time)
	{
		if (isInitADS)
		{
			remainADSTime -= time * 1000;
			if (remainADSTime <= 0)
			{
				remainADSTime = 0;
			}
		}
	}

	public static void InitADSTimestamp()
	{
		if (CurLevel >= 20 && ADSTimestamp <= 0)
		{
			int num = 240000;
			if (ShowVideoTimes >= 15)
			{
				num *= 2;
			}
			ADSTimestamp = GetTimestamp() + num;
		}
	}

	public static void SetRemainADSTime()
	{
		int num = 240000;
		if (ShowVideoTimes >= 15)
		{
			num *= 2;
		}
		remainADSTime = num;
	}

	public static void InitADSTimestampNew()
	{
		if (CurLevel >= 20 && !isInitADS)
		{
			isInitADS = true;
			SetRemainADSTime();
		}
	}

	public static long GetTimestamp()
	{
		return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
	}

	public static long GetTimestampByDateTime(DateTime d)
	{
		return (d.ToUniversalTime().Ticks - 621355968000000000L) / 10000;
	}

	public static float MsToHour(long ms)
	{
		return (float)ms / 3600000f;
	}

	public static void SetCoinCountLevel(int value)
	{
		CoinCountLevel += value;
		PlayerPrefs.SetInt("CoinCountLevel", CoinCountLevel);
	}

	public static void SetCoinOffLineLevel(int value)
	{
		CoinOffLineLevel += value;
		PlayerPrefs.SetInt("CoinOffLineLevel", CoinOffLineLevel);
	}

	public static void SetFireSpeed(int change)
	{
		FireSpeedLevel += change;
		PlayerPrefs.SetInt("FireSpeedLevel", FireSpeedLevel);
	}

	public static void SetFirePower(int change)
	{
		FirePowerLevel += change;
		PlayerPrefs.SetInt("FirePowerLevel", FirePowerLevel);
	}

	public static FireSpeedStruct GetFireSpeedData()
	{
		if (FireSpeedLevel >= Singleton<GameJson>.Instance.FireSpeedJson.Length)
		{
			FireSpeedLevel = Singleton<GameJson>.Instance.FireSpeedJson.Length - 1;
		}
		return Singleton<GameJson>.Instance.FireSpeedJson[FireSpeedLevel];
	}

	public static FirePowerStruct GetFirePowerData()
	{
		if (FirePowerLevel >= Singleton<GameJson>.Instance.FirePowerJson.Length)
		{
			FirePowerLevel = Singleton<GameJson>.Instance.FirePowerJson.Length - 1;
		}
		return Singleton<GameJson>.Instance.FirePowerJson[FirePowerLevel];
	}

	public static WeaponStruct GetWeaponData()
	{
		if (WeaponID >= Singleton<GameJson>.Instance.WeaponJson.Length)
		{
			UnityEngine.Debug.LogError("GetWeaponData is erro: " + WeaponID);
			return Singleton<GameJson>.Instance.WeaponJson[0];
		}
		if (WeaponID < 0)
		{
			return Singleton<GameJson>.Instance.WeaponJson[0];
		}
		return Singleton<GameJson>.Instance.WeaponJson[WeaponID];
	}

	public static CoinLevelStruct GetCoinLevelData(int level)
	{
		if (level >= Singleton<GameJson>.Instance.CoinLevelJson.Length)
		{
			level = Singleton<GameJson>.Instance.CoinLevelJson.Length - 1;
		}
		return Singleton<GameJson>.Instance.CoinLevelJson[level];
	}

	public static string GetFireSpeedTxt(FireSpeedStruct fireSpeed)
	{
		return ((int)((double)fireSpeed.Count * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].Count)).ToString();
	}

	public static string GetFireTxt(FirePowerStruct firePower)
	{
		float value = (float)((double)GameUtils.UnitToFloat(firePower.Power) * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].Power);
		return GameUtils.FloatToUnit(value);
	}

	public static float GetFireSpeedCost(FireSpeedStruct fireSpeed)
	{
		return GameUtils.UnitToFloat(fireSpeed.Cost) * (float)Singleton<GameJson>.Instance.CoinCostRateStruct[0].CostSpeed;
	}

	public static float GetFireCost(FirePowerStruct firePower)
	{
		return GameUtils.UnitToFloat(firePower.Cost) * (float)Singleton<GameJson>.Instance.CoinCostRateStruct[0].CostFire;
	}

	public static string GetWeaponScaleTxt(int weaponID, WeaponLevelStruct weaponStruct)
	{
		double num = 0.0;
		switch (weaponID)
		{
		case 0:
			num = weaponStruct.FireScale_1 * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FireScale_1;
			break;
		case 1:
			num = weaponStruct.FireDistance_2 * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FireDistance_2;
			break;
		case 2:
			num = weaponStruct.FireWide_3 * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FireWide_3;
			break;
		case 3:
			num = weaponStruct.FireScale_4 * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FireScale_4;
			break;
		case 4:
			num = weaponStruct.FireNetScale_5 * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FireNetScale_5;
			break;
		}
		return num.ToString();
	}

	public static string GetWeaponFireTxt(int weaponID, WeaponLevelStruct weaponStruct)
	{
		double num = 0.0;
		switch (weaponID)
		{
		case 0:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePower_1) * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FirePower_1;
			break;
		case 1:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePower_2) * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FirePower_2;
			break;
		case 2:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePower_3) * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FirePower_3;
			break;
		case 3:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePower_4) * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FirePower_4;
			break;
		case 4:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePower_5) * Singleton<GameJson>.Instance.WeaponParamRateStruct[0].FirePower_5;
			break;
		}
		return GameUtils.FloatToUnit((float)num);
	}

	public static float GetWeaponScaleCostTxt(int weaponID, WeaponLevelStruct weaponStruct)
	{
		double num = 0.0;
		switch (weaponID)
		{
		case 0:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FireScaleCost_1) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 1:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FireDistanceCost_2) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 2:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FireWideCost_3) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 3:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FireScaleCost_4) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 4:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FireScaleCost_5) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		}
		return (float)num;
	}

	public static float GetWeaponFireCostTxt(int weaponID, WeaponLevelStruct weaponStruct)
	{
		double num = 0.0;
		switch (weaponID)
		{
		case 0:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePowerCost_1) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 1:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePowerCost_2) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 2:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePowerCost_3) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 3:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePowerCost_4) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		case 4:
			num = (double)GameUtils.UnitToFloat(weaponStruct.FirePowerCost_5) * Singleton<GameJson>.Instance.CoinCostRateStruct[0].WeaponCost;
			break;
		}
		return (float)num;
	}

	public static float GetCoinValueCost(CoinLevelStruct coinCount)
	{
		return GameUtils.UnitToFloat(coinCount.CoinCountCost) * (float)Singleton<GameJson>.Instance.CoinCostRateStruct[0].CoinCountCost;
	}

	public static float GetOfflineValueCost(CoinLevelStruct coinCount)
	{
		return GameUtils.UnitToFloat(coinCount.OffLineCost) * (float)Singleton<GameJson>.Instance.CoinCostRateStruct[0].OffLineCost;
	}

	public static float GetCoinValueTxt(CoinLevelStruct coinCount)
	{
		return (float)(coinCount.CoinCount * Singleton<GameJson>.Instance.CoinCostRateStruct[0].CoinCount);
	}

	public static void InitPlayerPrefs()
	{
		IsMusice = (PlayerPrefs.GetInt("IsMusice", 0) == 0);
		CurLevel = PlayerPrefs.GetInt("CurLevel", CurLevel);
		CurCoin = PlayerPrefs.GetFloat("CurCoin", CurCoin);
		CurDiamond = PlayerPrefs.GetFloat("CurDiamond", CurDiamond);
		CurHeart = PlayerPrefs.GetInt("CurHeart", CurHeart);
		PlayerType = PlayerPrefs.GetInt("PlayerType", PlayerType);
		FailCount = PlayerPrefs.GetInt("FailCount", FailCount);
		UpGradeFireSpeedCount = PlayerPrefs.GetInt("UpGradeFireSpeedCount", UpGradeFireSpeedCount);
		UpGradeFirePowerCount = PlayerPrefs.GetInt("UpGradeFirePowerCount", UpGradeFirePowerCount);
		UpGradeOfflineCount = PlayerPrefs.GetInt("UpGradeOfflineCount", UpGradeOfflineCount);
		GradeState = PlayerPrefs.GetInt("GradeState", GradeState);
		OpenWeaponCount = PlayerPrefs.GetInt("OpenWeaponCount", OpenWeaponCount);
		ShakeFlag = PlayerPrefs.GetInt("ShakeFlag", ShakeFlag);
		TutorialState = PlayerPrefs.GetInt("TutorialState", TutorialState);
		ActualLevelReject = PlayerPrefs.GetInt("ActualLevelReject", ActualLevelReject);
		IsEvaluate = PlayerPrefs.GetInt("IsEvaluate", IsEvaluate);
		TaskLock = PlayerPrefs.GetInt("TaskLock", TaskLock);
		TaskFirePowerCost = PlayerPrefs.GetString("TaskFirePowerCost", TaskFirePowerCost);
		TaskRefresh = PlayerPrefs.GetInt("TaskRefresh", TaskRefresh);
		TaskTime = long.Parse(PlayerPrefs.GetString("TaskTime", "0"));
		RefreshTime = long.Parse(PlayerPrefs.GetString("RefreshTime", "0"));
		ShowVideoTimes = PlayerPrefs.GetInt("ShowVideoTimes", ShowVideoTimes);
		Singleton<ShareManager>.Instance.InitIsShareToday(PlayerPrefs.GetInt("IsShareToday", 0));
		Singleton<ShareManager>.Instance.InitClickShareTips(PlayerPrefs.GetInt("ClickShareTips", 0));
		Singleton<DiamondManager>.Instance.InitGotVideoReward(long.Parse(PlayerPrefs.GetString("GotShowVideoRewardTime", "0")));
		Singleton<LuckyDrawManager>.Instance.InitLuckyBoxTime(PlayerPrefs.GetInt("IsLuckyBox", 0));
		Singleton<DiamondStorageManager>.Instance.InitToTalDiamond(PlayerPrefs.GetInt("TotalStorageDiamond", 0));
		Singleton<AdjustConfig>.Instance.InitKeys();
		Singleton<AdjustConfig>.Instance.InitFirstDayTime();
		Singleton<ShopManager>.Instance.InitShowAdsTimes();
		FireSpeedLevel = PlayerPrefs.GetInt("FireSpeedLevel", FireSpeedLevel);
		FirePowerLevel = PlayerPrefs.GetInt("FirePowerLevel", FirePowerLevel);
		WeaponID = PlayerPrefs.GetInt("WeaponID", WeaponID);
		PreWeaponID = WeaponID;
		for (int i = 0; i < WeaponLevels.Length; i++)
		{
			WeaponLevels[i] = PlayerPrefs.GetInt("WeaponLevel_" + i, WeaponLevels[i]);
		}
		for (int j = 0; j < WeaponMaxLevelFree.Length; j++)
		{
			WeaponMaxLevelFree[j] = PlayerPrefs.GetInt("WeaponMaxLevelFree_" + j, WeaponMaxLevelFree[j]);
		}
		CoinCountLevel = PlayerPrefs.GetInt("CoinCountLevel", CoinCountLevel);
		CoinOffLineLevel = PlayerPrefs.GetInt("CoinOffLineLevel", CoinOffLineLevel);
		OfflineTimeRecord = long.Parse(PlayerPrefs.GetString("OfflineTimeRecord", "0"));
		SlotProgress = PlayerPrefs.GetInt("SlotProgress", SlotProgress);
		ShopData = new ShopPrefsData();
		ShopData.LoadNoAdsState();
		ShopData.LoadTimeLimitGiftState(Singleton<GameJson>.Instance.TimeGiftJson);
		ShopData.LoadTimeLimitGiftTime(Singleton<GameJson>.Instance.TimeGiftJson, ActualLevel);
		BattleItemData = new BattleItemPrefsData();
		BattleItemData.InitLoadData();
		Singleton<VipManager>.Instance.Init();
		NpcData = new NpcPrefsData();
		NpcData.LoadNpcLevelID();
		Singleton<TutorialManager>.Instance.InitGuide();
		Singleton<WeaponMaxManager>.Instance.InitMaxOpenToday();
		Singleton<ReloadDataManager>.Instance.InitReload();
		Singleton<DialogueManager>.Instance.InitShowFirshNpc();
	}

	public static void SetTaskTime(long time)
	{
		TaskTime = time;
		PlayerPrefs.SetString("TaskTime", TaskTime.ToString());
	}

	public static void SetRefreshTime(long time)
	{
		RefreshTime = time;
		PlayerPrefs.SetString("RefreshTime", RefreshTime.ToString());
	}

	public static void SetTaskRefresh(int value)
	{
		TaskRefresh = value;
		PlayerPrefs.SetInt("TaskRefresh", TaskRefresh);
	}

	public static void SetTaskFirePowerCost(string cost)
	{
		TaskFirePowerCost = cost;
		PlayerPrefs.SetString("TaskFirePowerCost", TaskFirePowerCost);
	}

	public static void SetTaskLock(int value)
	{
		TaskLock = value;
		PlayerPrefs.SetInt("TaskLock", TaskLock);
	}

	public static void SetSlotProgress(int value)
	{
		if (value > 5)
		{
			value = 5;
		}
		SlotProgress = value;
		SlotProgress = PlayerPrefs.GetInt("SlotProgress", SlotProgress);
	}

	public static bool IsTutorialNew()
	{
		return TutorialState != 3;
	}

	public static void SetTutorialStat(TutorialType type)
	{
		TutorialState = (int)type;
		PlayerPrefs.SetInt("TutorialState", TutorialState);
	}

	public static void SetShake()
	{
		ShakeFlag = ((ShakeFlag == 0) ? 1 : 0);
		PlayerPrefs.SetInt("ShakeFlag", ShakeFlag);
	}

	public static void SetActualLevelReject()
	{
		PlayerPrefs.SetInt("ActualLevelReject", ActualLevelReject);
	}

	public static void SetIsEvaluate()
	{
		PlayerPrefs.SetInt("IsEvaluate", IsEvaluate);
	}

	public static void SetWeaponMaxLevelFree(int weaponID)
	{
		if (weaponID < WeaponMaxLevelFree.Length)
		{
			WeaponMaxLevelFree[weaponID] = 1;
			PlayerPrefs.SetInt("WeaponMaxLevelFree_" + weaponID, WeaponMaxLevelFree[weaponID]);
		}
	}

	public static void SetOpenWeaponCount(int count)
	{
		OpenWeaponCount = count;
		PlayerPrefs.SetInt("OpenWeaponCount", OpenWeaponCount);
	}

	public static void SetTempWeaponID(int id)
	{
		TempWeaponID = id;
	}

	public static void SetMaxWeaponState(int state)
	{
		MaxWeaponState = state;
	}

	public static bool IsTutorial()
	{
		return PlayerType == 0;
	}

	public static void SetTutorial()
	{
		if (PlayerType == 0)
		{
			PlayerType = 1;
			PlayerPrefs.SetInt("PlayerType", PlayerType);
		}
	}

	public static void AddFailCount(int value)
	{
		FailCount += value;
		SetFailCount(FailCount);
	}

	public static void SetFailCount(int value)
	{
		FailCount = value;
		PlayerPrefs.SetInt("FailCount", FailCount);
	}

	public static void AddCurLevel(int value)
	{
		if (CurLevel < Singleton<GameJson>.Instance.LevelJson.Length - 1)
		{
			CurLevel += value;
		}
		PlayerPrefs.SetInt("CurLevel", CurLevel);
	}

	public static void SetCurLevel(int value)
	{
		PlayerPrefs.SetInt("CurLevel", CurLevel);
	}

	public static void SetIsMusice(bool isMusice)
	{
		IsMusice = isMusice;
		PlayerPrefs.SetInt("IsMusice", (!isMusice) ? 1 : 0);
	}

	public static void SetNeedHitCount(int divideCount)
	{
		NeedHitCount += (int)Mathf.Pow(2f, divideCount - 1);
	}

	public static void SetCleanVenomNeedHitCount(int divideCount)
	{
		NeedHitCount -= (int)Mathf.Pow(2f, divideCount - 1);
	}

	public static void AddCurCoin(float change)
	{
		CurCoin += change;
		SetCurCoinPlayerPres(CurCoin);
	}

	public static void SetCurCoin(float value)
	{
		CurCoin = value;
		SetCurCoinPlayerPres(CurCoin);
	}

	private static void SetCurCoinPlayerPres(float CurCoin)
	{
		PlayerPrefs.SetFloat("CurCoin", CurCoin);
		ApplicationFacade.Instance.SendNotification("SET_COIN");
		ApplicationFacade.Instance.SendNotification("PROPERTY_GOLD_REFRESH_GUIDE");
	}

	public static void SetCurDiamond(float value)
	{
		CurDiamond = value;
		PlayerPrefs.SetFloat("CurDiamond", CurDiamond);
		ApplicationFacade.Instance.SendNotification("SET_DIAMOND");
	}

	public static void SetCurHeartCount(int value)
	{
		CurHeart = value;
		PlayerPrefs.SetInt("CurHeart", CurHeart);
		ApplicationFacade.Instance.SendNotification("SET_HEART");
	}
}

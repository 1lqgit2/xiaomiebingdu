using LitJson;
using UnityEngine;

namespace orangesGame
{
	internal class ReloadDataManager : Singleton<ReloadDataManager>
	{
		private int reloadData;

		public void InitReload()
		{
			reloadData = PlayerPrefs.GetInt("reloadData", 0);
		}

		public void SetReloadData(int temp)
		{
			reloadData = temp;
			PlayerPrefs.SetInt("reloadData", reloadData);
		}

		private void RecoveryData(string data)
		{
			if (!string.IsNullOrEmpty(data))
			{
				AData aData = JsonMapper.ToObject<AData>(data);
				GameGlobalData.CurLevel = aData.lv;
				GameGlobalData.CurCoin = GameUtils.UnitToFloat(aData.coin);
				GameGlobalData.CurDiamond = GameUtils.UnitToFloat(aData.diamond);
				GameGlobalData.FirePowerLevel = aData.mainFireLv;
				GameGlobalData.FireSpeedLevel = aData.mainSpeedLv;
				GameGlobalData.WeaponLevels[0] = aData.weapon1_1;
				GameGlobalData.WeaponLevels[1] = aData.weapon1_2;
				GameGlobalData.WeaponLevels[2] = aData.weapon2_1;
				GameGlobalData.WeaponLevels[3] = aData.weapon2_2;
				GameGlobalData.WeaponLevels[4] = aData.weapon3_1;
				GameGlobalData.WeaponLevels[5] = aData.weapon3_2;
				GameGlobalData.WeaponLevels[6] = aData.weapon4_1;
				GameGlobalData.WeaponLevels[7] = aData.weapon4_2;
				GameGlobalData.WeaponLevels[8] = aData.weapon5_1;
				GameGlobalData.WeaponLevels[9] = aData.weapon5_2;
				GameGlobalData.CoinCountLevel = aData.coinLv;
				GameGlobalData.CoinOffLineLevel = aData.offlinecoinlv;
				GameGlobalData.WeaponID = aData.weaponID;
				GameGlobalData.OpenWeaponCount = aData.openWeaponCount;
				GameGlobalData.SetCurLevel(GameGlobalData.CurLevel);
				GameGlobalData.SetCurCoin(GameGlobalData.CurCoin);
				GameGlobalData.SetCurDiamond(GameGlobalData.CurDiamond);
				PlayerPrefs.SetInt("FirePowerLevel", GameGlobalData.FirePowerLevel);
				PlayerPrefs.SetInt("FireSpeedLevel", GameGlobalData.FireSpeedLevel);
				PlayerPrefs.SetInt("CoinCountLevel", GameGlobalData.CoinCountLevel);
				PlayerPrefs.SetInt("CoinOffLineLevel", GameGlobalData.CoinOffLineLevel);
				GameGlobalData.SetWeaponID(GameGlobalData.WeaponID);
				GameGlobalData.SetOpenWeaponCount(GameGlobalData.OpenWeaponCount);
				for (int i = 0; i < GameGlobalData.WeaponLevels.Length; i++)
				{
					PlayerPrefs.SetInt("WeaponLevel_" + i, GameGlobalData.WeaponLevels[i]);
				}
			}
		}

		public void GetData()
		{
			if (1 == 0)
			{
			}
		}

		private void GetRemoteData(string serverData)
		{
			RecoveryData(serverData);
		}

		private bool GetReloadData()
		{
			string text = Singleton<SdkUtils>.Instance.GetReloadData();
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			RecoveryData(text);
			return true;
		}

		public void SetData()
		{
			AData aData = new AData();
			aData.coin = GameUtils.FloatToUnit(GameGlobalData.CurCoin);
			aData.diamond = GameUtils.FloatToUnit(GameGlobalData.CurDiamond);
			aData.lv = GameGlobalData.CurLevel;
			aData.mainFireLv = GameGlobalData.FirePowerLevel;
			aData.mainSpeedLv = GameGlobalData.FireSpeedLevel;
			aData.weapon1_1 = GameGlobalData.WeaponLevels[0];
			aData.weapon1_2 = GameGlobalData.WeaponLevels[1];
			aData.weapon2_1 = GameGlobalData.WeaponLevels[2];
			aData.weapon2_2 = GameGlobalData.WeaponLevels[3];
			aData.weapon3_1 = GameGlobalData.WeaponLevels[4];
			aData.weapon3_2 = GameGlobalData.WeaponLevels[5];
			aData.weapon4_1 = GameGlobalData.WeaponLevels[6];
			aData.weapon4_2 = GameGlobalData.WeaponLevels[7];
			aData.weapon5_1 = GameGlobalData.WeaponLevels[8];
			aData.weapon5_2 = GameGlobalData.WeaponLevels[9];
			aData.coinLv = GameGlobalData.CoinCountLevel;
			aData.offlinecoinlv = GameGlobalData.CoinOffLineLevel;
			aData.weaponID = GameGlobalData.WeaponID;
			aData.openWeaponCount = GameGlobalData.OpenWeaponCount;
			string text = JsonMapper.ToJson(aData);
			string gaID = Singleton<SdkUtils>.Instance.GetGaID();
			if (string.IsNullOrEmpty(gaID))
			{
			}
		}

		private void UploadDataSuc(string str)
		{
		}
	}
}

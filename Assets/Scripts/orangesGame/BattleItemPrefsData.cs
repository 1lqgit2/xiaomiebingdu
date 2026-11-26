using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class BattleItemPrefsData
	{
		public int BattleItemAddfirespeed;

		public int BattleItemBullettimes = 1;

		public float BattleItemWeakenVenomSpeedFactor = 1f;

		public int BattleEnhanceBulletDamageFactor = 1;

		public bool BattleIsPlayerBulletEnhanced;

		public int BattleItemLimitTimes = 2;

		public Dictionary<int, int> OwnBattleItemDic = new Dictionary<int, int>();

		private bool _isGiveProps;

		public bool IsGiveProps
		{
			get
			{
				return _isGiveProps;
			}
			set
			{
				PlayerPrefs.SetInt("IsGiveProps", value ? 1 : 0);
				_isGiveProps = value;
			}
		}

		public void InitLoadData()
		{
			OwnBattleItemDic.Clear();
			BattleItemStruct[] battleItemJson = Singleton<GameJson>.Instance.BattleItemJson;
			for (int i = 0; i < battleItemJson.Length; i++)
			{
				int iD = battleItemJson[i].ID;
				int @int = PlayerPrefs.GetInt("BattleItem" + iD, 0);
				OwnBattleItemDic.Add(iD, @int);
			}
			int int2 = PlayerPrefs.GetInt("IsGiveProps", 0);
			_isGiveProps = (int2 == 1);
		}

		public void ConsumeItemNums(int itemId)
		{
			SaveOwnBattleItem(itemId, -1);
		}

		public void SaveOwnBattleItem(int itemId, int number)
		{
			if (OwnBattleItemDic.ContainsKey(itemId))
			{
				int num = OwnBattleItemDic[itemId];
				OwnBattleItemDic[itemId] = num + number;
				OwnBattleItemDic[itemId] = ((OwnBattleItemDic[itemId] >= 0) ? OwnBattleItemDic[itemId] : 0);
				PlayerPrefs.SetInt("BattleItem" + itemId, OwnBattleItemDic[itemId]);
			}
			else
			{
				UnityEngine.Debug.LogError("[BattleItemData] SaveOwnBattleItem  Key is unlawful id: " + itemId);
			}
		}

		public int GetOwnBattleItem(int itemId)
		{
			if (OwnBattleItemDic.ContainsKey(itemId))
			{
				return OwnBattleItemDic[itemId];
			}
			return 0;
		}
	}
}

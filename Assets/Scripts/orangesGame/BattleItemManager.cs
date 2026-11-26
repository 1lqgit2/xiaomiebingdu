using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class BattleItemManager : Singleton<BattleItemManager>
	{
		public enum BattleItemIdType
		{
			EnhanceBulletDamage = 1,
			FastFireRate,
			WeakenVenom,
			InvincibleShield,
			RelivePower
		}

		private readonly Dictionary<int, BattleItemBase> _battleItemsDict = new Dictionary<int, BattleItemBase>();

		private readonly Dictionary<int, BattleItemStruct> _battleItemsDataDict = new Dictionary<int, BattleItemStruct>();

		private BattleItemStruct[] _battleItemStructs;

		private int _givePropNums = 2;

		private int[] useItems = new int[2];

		private BattleItemManager()
		{
			_battleItemStructs = Singleton<GameJson>.Instance.BattleItemJson;
			_battleItemsDict.Clear();
			_battleItemsDataDict.Clear();
			for (int i = 0; i < _battleItemStructs.Length; i++)
			{
				BattleItemBase battleItemBase = InitBattleItem(_battleItemStructs[i]);
				if (battleItemBase != null)
				{
					_battleItemsDict.Add(_battleItemStructs[i].ID, battleItemBase);
					_battleItemsDataDict.Add(_battleItemStructs[i].ID, _battleItemStructs[i]);
				}
			}
		}

		private BattleItemBase InitBattleItem(BattleItemStruct data)
		{
			switch (data.ID)
			{
			case 1:
				return new BattleItemEnhanceBulletDamage(data.EnName, data.Price, data.Time, data.ID);
			case 2:
				return new BattleItemFastFireRate(data.EnName, data.Price, data.Time, data.ID);
			case 3:
				return new BattleItemWeakenVenom(data.EnName, data.Price, data.Time, data.ID);
			case 4:
				return new BattleItemInvincibleShield(data.EnName, data.Price, data.Time, data.ID);
			case 5:
				return new BattleItemRelivePower(data.EnName, data.Price, data.Time, data.ID);
			default:
				return null;
			}
		}

		public bool ConsumeItem(int itemId)
		{
			if (_battleItemsDict.ContainsKey(itemId))
			{
				if (CheckItemNums(itemId) && GameGlobalData.GameState != GameState.Gaming)
				{
					return false;
				}
				if (_battleItemsDict[itemId].CurrentBattleItemState != 0)
				{
					return false;
				}
				_battleItemsDict[itemId].Employ();
				GameGlobalData.BattleItemData.ConsumeItemNums(itemId);
				AddUseItem(itemId);
				return true;
			}
			UnityEngine.Debug.LogError("[BattleItemManager] ConsumeItem ID is Unlawful id:" + itemId);
			return false;
		}

		public bool CheckItemNums(int itemId)
		{
			return GameGlobalData.BattleItemData.GetOwnBattleItem(itemId) > 0;
		}

		public bool CheckItemStateReady(int itemId)
		{
			if (_battleItemsDict.ContainsKey(itemId) && _battleItemsDict[itemId].CurrentBattleItemState == BattleItemBase.BattleItemState.Ready)
			{
				return true;
			}
			return false;
		}

		public void SetBattleItemNumber(int itemId, int number, bool isBuy = true)
		{
			if (!isBuy)
			{
				ApplicationFacade.Instance.SendNotification("MAIN_PROPSHOP_TIP", true);
			}
			GameGlobalData.BattleItemData.SaveOwnBattleItem(itemId, number);
		}

		public int GetOwnBattleItem(int itemId)
		{
			return GameGlobalData.BattleItemData.GetOwnBattleItem(itemId);
		}

		public BattleItemStruct GetBattleItemStructByItemId(int itemId)
		{
			return _battleItemsDataDict[itemId];
		}

		public BattleItemStruct GetBattleItemStructByIndex(int index)
		{
			return _battleItemStructs[index];
		}

		public void GivePropsFree()
		{
			SetBattleItemNumber(1, _givePropNums, isBuy: false);
			SetBattleItemNumber(2, _givePropNums, isBuy: false);
			SetBattleItemNumber(3, _givePropNums, isBuy: false);
		}

		private void AddUseItem(int itemId)
		{
			if (useItems[0] == 0)
			{
				useItems[0] = itemId;
			}
			else if (useItems[1] == 0)
			{
				useItems[1] = itemId;
			}
		}

		public int[] GetUseItems()
		{
			return useItems;
		}

		public void CleanUseItem()
		{
			useItems[0] = 0;
			useItems[1] = 0;
		}
	}
}

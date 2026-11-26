using System;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class CoinManager : Singleton<CoinManager>
	{
		public enum Type
		{
			ITEM_INDEX,
			WeakenVenom,
			FastFireRate,
			EnhanceBulletDamage,
			CoinBullet,
			SlowPlayer,
			EnhanceVenom,
			ExpandVenom,
			ItemWingman,
			COUNT,
			NONE
		}

		private GameItemManager _gameItemMgr;

		private BallManger _ballMgr;

		private CoinManager()
		{
			_gameItemMgr = Singleton<GameItemManager>.Instance;
			_ballMgr = Singleton<BallManger>.Instance;
		}

		public List<int> ForwardItem()
		{
			List<int> list = new List<int>();
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);
			list.Add(8);
			return list;
		}

		public void SpawnFloatCoin(float count, int coinIndex, Vector2 pos)
		{
			float num = UnityEngine.Random.Range(0, 360);
			if (GameGlobalData.IsPlayerCoinBullet)
			{
				count *= 3f;
			}
			if (count != 0f)
			{
				int init = (int)(count / (float)coinIndex);
				for (int i = 0; i < coinIndex; i++)
				{
					FloatGold floatGold = _gameItemMgr.Spawn<FloatGold>("GameObject/Item/FloatCoin", "GameObject/Item/FloatCoin");
					floatGold.SetInit(init);
					floatGold.Moving(pos, num + (float)(90 * i), 0.35f);
				}
			}
		}

		public void SpawnCoinBulletCoin(int money, Vector2 pos)
		{
			if (money != 0)
			{
				FloatGold floatGold = _gameItemMgr.Spawn<FloatGold>("GameObject/Item/FloatCoin", "GameObject/Item/FloatCoin");
				floatGold.SetInit(money);
				Vector3 zero = Vector3.zero;
				zero.x = (pos.x - GameGlobalData.CoinRecoverPos.x) / 4f + GameGlobalData.CoinRecoverPos.x;
				zero.y = GameGlobalData.CoinRecoverPos.y - 2f;
				Vector3[] path = new Vector3[2]
				{
					(Vector2)zero + UnityEngine.Random.insideUnitCircle * 0.5f,
					GameGlobalData.CoinRecoverPos
				};
				floatGold.Moving(pos, 0.5f, path);
			}
		}

		public void Sqawn(Vector2 pos, Type type, Vector2 dir, bool isPlayer = false)
		{
			ItemBase itemBase;
			switch (type)
			{
			case Type.WeakenVenom:
				itemBase = _gameItemMgr.Spawn<ItemWeakenVenom>("GameObject/Item/ItemWeakenVenom", "GameObject/Item/ItemWeakenVenom");
				break;
			case Type.FastFireRate:
				itemBase = _gameItemMgr.Spawn<ItemFastFireRate>("GameObject/Item/ItemFastFireRate", "GameObject/Item/ItemFastFireRate");
				break;
			case Type.EnhanceBulletDamage:
				itemBase = _gameItemMgr.Spawn<ItemEnhanceBulletDamage>("GameObject/Item/ItemEnhancedBulletDamage", "GameObject/Item/ItemEnhancedBulletDamage");
				break;
			case Type.CoinBullet:
				itemBase = _gameItemMgr.Spawn<ItemCoinBullet>("GameObject/Item/ItemCoinBullet", "GameObject/Item/ItemCoinBullet");
				break;
			case Type.SlowPlayer:
				itemBase = _gameItemMgr.Spawn<ItemSlowPlayer>("GameObject/Item/ItemSlowPlayer", "GameObject/Item/ItemSlowPlayer");
				break;
			case Type.EnhanceVenom:
				itemBase = _gameItemMgr.Spawn<ItemEnhanceVenom>("GameObject/Item/ItemEnhanceVenom", "GameObject/Item/ItemEnhanceVenom");
				break;
			case Type.ExpandVenom:
				itemBase = _gameItemMgr.Spawn<ItemExpandVenom>("GameObject/Item/ItemExpandVenom", "GameObject/Item/ItemExpandVenom");
				break;
			case Type.ItemWingman:
				itemBase = _gameItemMgr.Spawn<ItemWingman>("GameObject/Item/ItemWingman", "GameObject/Item/ItemWingman");
				break;
			default:
				UnityEngine.Debug.LogError("ItemBase Type is nil");
				itemBase = (ItemBase)new GameItem();
				break;
			}
			itemBase.Init(pos, type, dir, isPlayer);
		}

		public void FloatByCircle<T>(string path, int count, Vector2 startPos, Vector2 endPos, float moveDuration, Action onComplete, Transform tran) where T : FloatCoin, new()
		{
			float num = 360f / (float)count;
			for (int i = 0; i < count; i++)
			{
				T val = _gameItemMgr.Spawn<T>(path, path, tran);
				val.SetInit();
				float distance = UnityEngine.Random.Range(0.5f, 0.75f);
				float preMoveCircleRadius = UnityEngine.Random.Range(2.5f, 4.3f);
				val.Moving(startPos, num * (float)i, distance, endPos, preMoveCircleRadius, moveDuration);
			}
			Timer.Register(1f, onComplete);
		}
	}
}

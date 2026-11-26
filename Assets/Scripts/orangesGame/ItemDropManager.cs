using System.Collections.Generic;

namespace orangesGame
{
	internal class ItemDropManager : Singleton<ItemDropManager>
	{
		private int DropCount = 2;

		private int DropCountMax = 5;

		public int DropItemCount()
		{
			LevelStruct levelStruct = Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel];
			int[] itemsDropNum = levelStruct.ItemsDropNum;
			int num = GameGlobalData.FailCount - 2;
			num = ((num >= 0) ? num : 0);
			num = ((num <= DropCountMax) ? num : DropCountMax);
			int num2 = itemsDropNum[0];
			return num2 + num;
		}

		public int[] DropItemWeight()
		{
			LevelStruct levelStruct = Singleton<GameJson>.Instance.LevelJson[GameGlobalData.CurLevel];
			int[] itemsDropWeight = levelStruct.ItemsDropWeight;
			if (GameGlobalData.FailCount < DropCount)
			{
				return itemsDropWeight;
			}
			List<int> list = Singleton<CoinManager>.Instance.ForwardItem();
			for (int i = 0; i < itemsDropWeight.Length; i++)
			{
				int item = i + 1;
				if (!list.Contains(item))
				{
					itemsDropWeight[i] = 0;
				}
			}
			return itemsDropWeight;
		}
	}
}

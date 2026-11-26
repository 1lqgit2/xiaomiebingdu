using UnityEngine;

namespace orangesGame
{
	internal class ShopManager : Singleton<ShopManager>
	{
		private int[] adsToBuyTimes = new int[3]
		{
			4,
			8,
			24
		};

		private int[] adsToDiamonds = new int[3]
		{
			25,
			50,
			150
		};

		private int[] showAdsTimes = new int[3];

		private int tempIndex = -1;

		public void InitShowAdsTimes()
		{
			for (int i = 0; i < showAdsTimes.Length; i++)
			{
				showAdsTimes[i] = PlayerPrefs.GetInt("Shop_ShowAdsTimes_" + i, 0);
			}
		}

		public void SetRemainAdsTimes(int index, int addCount)
		{
			showAdsTimes[index] += addCount;
			PlayerPrefs.SetInt("Shop_ShowAdsTimes_" + index, showAdsTimes[index]);
		}

		public int GetAdsToBuyTimes(int index)
		{
			return adsToBuyTimes[index];
		}

		public int GetShowAdsTimes(int index)
		{
			return showAdsTimes[index];
		}

		public int GetAdsDiamondNum(int index)
		{
			return adsToDiamonds[index];
		}

		public void SetTempIndex(int index)
		{
			tempIndex = index;
		}

		public int GetTempIndex()
		{
			return (tempIndex != -1) ? tempIndex : 0;
		}

		public int ShowVedioAndGetReward()
		{
			if (tempIndex == -1)
			{
				return -1;
			}
			if (GetShowAdsTimes(tempIndex) >= GetAdsToBuyTimes(tempIndex))
			{
				return -1;
			}
			SetRemainAdsTimes(tempIndex, 1);
			int num = GetShowAdsTimes(tempIndex);
			int result = 0;
			bool flag = false;
			if (num == GetAdsToBuyTimes(tempIndex))
			{
				result = GetAdsDiamondNum(tempIndex);
				flag = true;
				SetRemainAdsTimes(tempIndex, -GetAdsToBuyTimes(tempIndex));
			}
			GameAnalysisData.ShowVideoBuyDiamond(tempIndex + 1, num, flag ? 1 : 0);
			tempIndex = -1;
			return result;
		}
	}
}

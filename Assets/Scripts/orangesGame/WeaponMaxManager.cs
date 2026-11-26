using UnityEngine;

namespace orangesGame
{
	public class WeaponMaxManager : Singleton<WeaponMaxManager>
	{
		private int MaxOpenToday;

		private int FAIL_RATE = 30;

		private int WIN_RATE = 10;

		public void InitMaxOpenToday()
		{
			MaxOpenToday = PlayerPrefs.GetInt("MaxOpenToday", 0);
		}

		public void SetMaxOpenToday(int temp)
		{
			MaxOpenToday = temp;
			PlayerPrefs.SetInt("MaxOpenToday", MaxOpenToday);
		}

		public void ShowMaxWeapon(bool isSuc)
		{
			if (GameGlobalData.ActualLevel < 6)
			{
				return;
			}
			if (!isSuc)
			{
				if (MaxOpenToday == 0)
				{
					Singleton<GameJson>.Instance.CommonJson[0].Value = 100.0;
				}
				else
				{
					Singleton<GameJson>.Instance.CommonJson[0].Value = FAIL_RATE;
				}
			}
			else
			{
				Singleton<GameJson>.Instance.CommonJson[0].Value = WIN_RATE;
			}
		}
	}
}

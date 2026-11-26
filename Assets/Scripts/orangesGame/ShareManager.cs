using UnityEngine;

namespace orangesGame
{
	internal class ShareManager : Singleton<ShareManager>
	{
		private static int IsShareToday;

		private static int ClickShareTips;

		public void InitClickShareTips(int clickShareTips)
		{
			ClickShareTips = clickShareTips;
		}

		public void SetClickShareTips(bool click)
		{
			ClickShareTips = (click ? 1 : 0);
			PlayerPrefs.SetInt("ClickShareTips", ClickShareTips);
		}

		public void InitIsShareToday(int share)
		{
			IsShareToday = share;
		}

		public bool HasShareToday()
		{
			return IsShareToday == 1;
		}

		public bool HasClickShareTips()
		{
			return ClickShareTips == 1;
		}

		public void SetIsShareToday(bool share)
		{
			IsShareToday = (share ? 1 : 0);
			PlayerPrefs.SetInt("IsShareToday", IsShareToday);
		}

		public void GotShareReward()
		{
			if (!HasShareToday())
			{
				SetIsShareToday(share: true);
				int num = Singleton<DiamondManager>.Instance.ShareDiamond();
				GameAnalysisData.ShareAndGotDiamond(num, (long)(GameGlobalData.CurDiamond - (float)num));
			}
		}
	}
}

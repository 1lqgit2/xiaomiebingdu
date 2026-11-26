using System;

namespace orangesGame
{
	internal class TimeRefreshTask : Singleton<TimeRefreshTask>
	{
		private int hour = 5;

		public bool CheckAndRefresh()
		{
			DateTime now = DateTime.Now;
			int num = now.Hour;
			if (GameGlobalData.RefreshTime == 0)
			{
				DateTime today = DateTime.Today;
				if (num < 5)
				{
					DateTime d = today.AddDays(-1.0).AddHours(hour);
					GameGlobalData.SetRefreshTime(GameGlobalData.GetTimestampByDateTime(d));
				}
				else
				{
					DateTime d2 = today.AddHours(hour);
					GameGlobalData.SetRefreshTime(GameGlobalData.GetTimestampByDateTime(d2));
				}
				return false;
			}
			long refreshTime = GameGlobalData.RefreshTime;
			refreshTime += 86400000;
			if (GameGlobalData.GetTimestampByDateTime(now) < refreshTime)
			{
				return false;
			}
			GameGlobalData.SetRefreshTime(refreshTime);
			return true;
		}

		public bool DoRefresh()
		{
			bool flag = CheckAndRefresh();
			if (flag)
			{
				GameGlobalData.ShowVideoTimes = 0;
				Singleton<ShareManager>.Instance.SetIsShareToday(share: false);
				Singleton<ShareManager>.Instance.SetClickShareTips(click: false);
				Singleton<VipManager>.Instance.IsGotFreeDiamond = false;
				Singleton<LuckyDrawManager>.Instance.SetLuckyBoxTime(draw: false);
				Singleton<TutorialManager>.Instance.CleanGuideStatus();
				Singleton<WeaponMaxManager>.Instance.SetMaxOpenToday(0);
			}
			return flag;
		}
	}
}

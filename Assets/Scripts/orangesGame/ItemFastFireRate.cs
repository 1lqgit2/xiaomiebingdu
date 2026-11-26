namespace orangesGame
{
	public class ItemFastFireRate : ItemBase
	{
		protected override void OnClean()
		{
		}

		protected override void OnContinue()
		{
		}

		protected override void OnInit()
		{
		}

		protected override void OnPause()
		{
		}

		protected override void OnTimeOut()
		{
			bool isRestShoot = Singleton<PlayerManager>.Instance.IsLeaderTouch();
			GameGlobalData.Addfirespeed = 0;
			GameGlobalData.Bullettimes = 1;
			if (GameGlobalData.BattleItemData.BattleItemAddfirespeed == 0)
			{
				Singleton<PlayerManager>.Instance.ChangeLeaderShooter(ShooterType.Change, isEffect: false, isRestShoot);
			}
			else
			{
				Singleton<PlayerManager>.Instance.ChangeLeaderShooter(ShooterType.Super);
			}
		}

		protected override void OnTouchPlayer(Player player)
		{
			GameGlobalData.Addfirespeed = 50;
			GameGlobalData.Bullettimes = 2;
			Singleton<PlayerManager>.Instance.ChangeLeaderShooter(ShooterType.Super);
		}
	}
}

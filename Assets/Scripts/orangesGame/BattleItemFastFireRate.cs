namespace orangesGame
{
	public class BattleItemFastFireRate : BattleItemBase
	{
		public BattleItemFastFireRate(string itemName, int itemPrice, int itemTime, int itemId)
			: base(itemName, itemPrice, itemTime, itemId)
		{
			ItemIconName = "firerate_vip";
		}

		protected override void TimeOut()
		{
			base.TimeOut();
			bool isRestShoot = Singleton<PlayerManager>.Instance.IsLeaderTouch();
			GameGlobalData.BattleItemData.BattleItemAddfirespeed = 0;
			GameGlobalData.BattleItemData.BattleItemBullettimes = 1;
			if (GameGlobalData.Addfirespeed == 0)
			{
				Singleton<PlayerManager>.Instance.ChangeLeaderShooter(ShooterType.Change, isEffect: false, isRestShoot);
			}
			else
			{
				Singleton<PlayerManager>.Instance.ChangeLeaderShooter(ShooterType.Super);
			}
		}

		public override void Employ()
		{
			base.Employ();
			GameGlobalData.BattleItemData.BattleItemAddfirespeed = 50;
			GameGlobalData.BattleItemData.BattleItemBullettimes = 2;
			Singleton<PlayerManager>.Instance.ChangeLeaderShooter(ShooterType.Super);
		}
	}
}

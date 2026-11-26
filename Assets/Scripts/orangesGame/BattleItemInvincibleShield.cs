namespace orangesGame
{
	public class BattleItemInvincibleShield : BattleItemBase
	{
		public BattleItemInvincibleShield(string itemName, int itemPrice, int itemTime, int itemId)
			: base(itemName, itemPrice, itemTime, itemId)
		{
			ItemIconName = "baohuzhao";
		}

		protected override void TimeOut()
		{
			base.TimeOut();
			Singleton<PlayerManager>.Instance.BattleItemSetSheild(isActive: false);
		}

		public override void Employ()
		{
			base.Employ();
			Singleton<PlayerManager>.Instance.BattleItemSetSheild(isActive: true);
		}
	}
}

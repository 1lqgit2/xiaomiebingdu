namespace orangesGame
{
	public class BattleItemWeakenVenom : BattleItemBase
	{
		public BattleItemWeakenVenom(string itemName, int itemPrice, int itemTime, int itemId)
			: base(itemName, itemPrice, itemTime, itemId)
		{
			ItemIconName = "weakenvirus_vip";
		}

		protected override void TimeOut()
		{
			base.TimeOut();
			GameGlobalData.BattleItemData.BattleItemWeakenVenomSpeedFactor = 1f;
		}

		public override void Employ()
		{
			base.Employ();
			GameGlobalData.BattleItemData.BattleItemWeakenVenomSpeedFactor = 0.2f;
		}
	}
}

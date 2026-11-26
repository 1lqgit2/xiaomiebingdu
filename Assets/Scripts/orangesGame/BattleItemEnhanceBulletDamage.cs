namespace orangesGame
{
	public class BattleItemEnhanceBulletDamage : BattleItemBase
	{
		public BattleItemEnhanceBulletDamage(string itemName, int itemPrice, int itemTime, int itemId)
			: base(itemName, itemPrice, itemTime, itemId)
		{
			ItemIconName = "firepower_vip";
		}

		protected override void TimeOut()
		{
			base.TimeOut();
			GameGlobalData.BattleItemData.BattleEnhanceBulletDamageFactor = 1;
			GameGlobalData.BattleItemData.BattleIsPlayerBulletEnhanced = false;
		}

		public override void Employ()
		{
			base.Employ();
			GameGlobalData.BattleItemData.BattleEnhanceBulletDamageFactor = 2;
			GameGlobalData.BattleItemData.BattleIsPlayerBulletEnhanced = true;
		}
	}
}

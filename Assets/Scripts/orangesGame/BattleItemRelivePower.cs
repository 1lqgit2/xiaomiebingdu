namespace orangesGame
{
	public class BattleItemRelivePower : BattleItemBase
	{
		private readonly ImpactWaveEffect _impactWaveEffect;

		public BattleItemRelivePower(string itemName, int itemPrice, int itemTime, int itemId)
			: base(itemName, itemPrice, itemTime, itemId)
		{
			_impactWaveEffect = new ImpactWaveEffect();
			ItemIconName = "dazhao";
		}

		public override void Employ()
		{
			base.Employ();
			_impactWaveEffect.Spawn(Singleton<PlayerManager>.Instance.GetLeader().localPosition);
		}
	}
}

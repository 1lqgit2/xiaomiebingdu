namespace orangesGame
{
	public class ItemWeakenVenom : ItemBase
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
			GameGlobalData.WeakenVenomSpeedFactor = 1f;
		}

		protected override void OnTouchPlayer(Player player)
		{
			GameGlobalData.WeakenVenomSpeedFactor = 0.2f;
		}
	}
}

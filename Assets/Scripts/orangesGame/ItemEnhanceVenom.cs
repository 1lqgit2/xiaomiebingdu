namespace orangesGame
{
	public class ItemEnhanceVenom : ItemBase
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
			GameGlobalData.EnhanceVenomSpeedFactor = 1f;
		}

		protected override void OnTouchPlayer(Player player)
		{
			GameGlobalData.EnhanceVenomSpeedFactor = 1.5f;
		}
	}
}

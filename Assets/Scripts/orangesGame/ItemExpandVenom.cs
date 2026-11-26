namespace orangesGame
{
	public class ItemExpandVenom : ItemBase
	{
		public static EventOnGetExpandVenomItem EventOnGetExpandVenomItem = new EventOnGetExpandVenomItem();

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
			GameGlobalData.ExpandVenomScaleFactor = 1f;
			EventOnGetExpandVenomItem.Invoke();
		}

		protected override void OnTouchPlayer(Player player)
		{
			GameGlobalData.ExpandVenomScaleFactor = 1.5f;
			EventOnGetExpandVenomItem.Invoke();
		}
	}
}

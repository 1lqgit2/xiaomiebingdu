namespace orangesGame
{
	public class ItemWingman : ItemBase
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
			Singleton<PlayerManager>.Instance.WingmanOut();
		}

		protected override void OnTouchPlayer(Player player)
		{
			Wingman wingman = Singleton<PlayerManager>.Instance.WingmanSpawn();
		}
	}
}

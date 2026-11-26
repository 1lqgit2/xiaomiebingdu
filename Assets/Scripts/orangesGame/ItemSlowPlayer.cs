namespace orangesGame
{
	public class ItemSlowPlayer : ItemBase
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
			_target.RecoverSpeed("ItemSlowPlayer");
		}

		protected override void OnTouchPlayer(Player player)
		{
			player.SlowDown("ItemSlowPlayer", 0.07f);
		}
	}
}

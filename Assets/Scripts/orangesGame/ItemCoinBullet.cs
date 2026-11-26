namespace orangesGame
{
	public class ItemCoinBullet : ItemBase
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
			GameGlobalData.IsPlayerCoinBullet = false;
			_target.ShowCoinBulletCoin(isshow: false);
		}

		protected override void OnTouchPlayer(Player player)
		{
			GameGlobalData.IsPlayerCoinBullet = true;
			player.ShowCoinBulletCoin(isshow: true);
			_target = player;
		}
	}
}

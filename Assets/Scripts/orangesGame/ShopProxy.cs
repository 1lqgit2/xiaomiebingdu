using PureMVC.Interfaces;
using PureMVC.Patterns;

namespace orangesGame
{
	public class ShopProxy : Proxy, IProxy
	{
		public ShopProxy(string NAME)
			: base(NAME)
		{
		}

		public float GetLastTimeLimitGiftCoin()
		{
			float fireCost = GameGlobalData.GetFireCost(GameGlobalData.GetFirePowerData());
			return fireCost * 60f;
		}
	}
}

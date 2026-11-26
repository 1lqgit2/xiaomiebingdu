using UnityEngine;

namespace orangesGame
{
	public class ItemEnhanceBulletDamage : ItemBase
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
			GameGlobalData.EnhanceBulletDamageFactor = 1f;
			GameGlobalData.IsPlayerBulletEnhanced = false;
		}

		protected override void OnTouchPlayer(Player player)
		{
			GameGlobalData.EnhanceBulletDamageFactor = 2f;
			GameGlobalData.IsPlayerBulletEnhanced = true;
		}

		private float GetFireMul()
		{
			return Mathf.Max(8f - (float)GameGlobalData.ActualFirePowerLevel / 12f, 2.1f);
		}
	}
}

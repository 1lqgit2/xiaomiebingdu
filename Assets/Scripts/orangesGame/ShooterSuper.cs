using UnityEngine;

namespace orangesGame
{
	public class ShooterSuper : ShooterChange
	{
		protected override void Operation(ShooterVO vo)
		{
			vo.FireBulletCount = Mathf.Max(vo.FireBulletCount * (float)GameGlobalData.Bullettimes * (float)GameGlobalData.BattleItemData.BattleItemBullettimes, GameGlobalData.Addfirespeed + GameGlobalData.BattleItemData.BattleItemAddfirespeed);
			base.Operation(vo);
		}

		private float GetFireMul()
		{
			return Mathf.Max(8f - (float)GameGlobalData.ActualFireSpeedLevel / 12f, 2.1f);
		}
	}
}

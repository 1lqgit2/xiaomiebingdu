namespace orangesGame
{
	public class HitWeaponBoomerangEffect : HitEffect
	{
		public HitWeaponBoomerangEffect()
		{
			_order = "eff_player_weapon1_hit";
			_resPath = "GameEffect/HitEffect/" + _order;
		}
	}
}

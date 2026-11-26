namespace orangesGame
{
	public class HitWeaponFireEffect : HitEffect
	{
		public HitWeaponFireEffect()
		{
			_order = "eff_player_weapon2_hit";
			_resPath = "GameEffect/HitEffect/" + _order;
		}
	}
}

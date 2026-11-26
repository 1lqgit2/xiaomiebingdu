namespace orangesGame
{
	public class HitWeaponThunderEffect : HitEffect
	{
		public HitWeaponThunderEffect()
		{
			_order = "eff_player_weapon5_hit";
			_resPath = "GameEffect/HitEffect/" + _order;
			_countMax = 20;
		}
	}
}

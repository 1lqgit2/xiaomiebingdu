namespace orangesGame
{
	public class FireBallWeapon : Weapon
	{
		private const float ANGLE_MIN = 5f;

		private const float ANGLE_MAX = 10f;

		private void Awake()
		{
			InitAwake();
		}

		public override void SetShooter(ShooterVO vo, bool isChange = false)
		{
			vo.AngleMin = 5f;
			vo.AngleMax = 10f;
			base.SetShooter(vo, isChange);
		}
	}
}

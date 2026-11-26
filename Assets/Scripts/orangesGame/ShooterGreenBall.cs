namespace orangesGame
{
	public class ShooterGreenBall : Shooter
	{
		private float _scale;

		public override void Fire()
		{
			BoomerangBullet boomerangBullet = Singleton<BulletManager>.Instance.Spawn<BoomerangBullet>(BulletType.Boomerang, "GameObject/Bullet/bullet_weapon6");
			boomerangBullet.SetProperty(_atk, _scale, _buttleSpeed, _shootPoint);
			boomerangBullet.tran.localPosition = _shootPoint.position;
		}

		protected override void Operation(ShooterVO vo)
		{
			_distance = vo.FireDistance;
			_atk = vo.FirePower;
			_scale = vo.FireScale;
			_buttleSpeed = vo.BulletMoveSpeed;
			_dir = vo.Dir;
		}
	}
}

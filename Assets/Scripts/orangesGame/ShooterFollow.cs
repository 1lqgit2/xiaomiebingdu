namespace orangesGame
{
	public class ShooterFollow : Shooter
	{
		private BulletType _type = BulletType.Rocket;

		private const string PATH = "GameObject/Bullet/bullet_weapon4";

		private float _scale;

		public override void Fire()
		{
			RockBullet rockBullet = Singleton<BulletManager>.Instance.Spawn<RockBullet>(_type, "GameObject/Bullet/bullet_weapon4");
			rockBullet.SetProperty(_atk, _scale, _buttleSpeed);
			rockBullet.tran.localPosition = _shootPoint.position;
			rockBullet.StartFollow(_dir);
		}

		protected override void Operation(ShooterVO vo)
		{
			_atk = vo.FirePower;
			_scale = vo.FireScale;
			_buttleSpeed = vo.BulletMoveSpeed;
			_distance = vo.FireDistance;
			_dir = vo.Dir;
		}
	}
}

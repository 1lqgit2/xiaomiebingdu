namespace orangesGame
{
	public class ShootDefense : Shooter
	{
		private float _speed;

		public override void Fire()
		{
			DefenseBullet defenseBullet = Singleton<BulletManager>.Instance.Spawn<DefenseBullet>(BulletType.DefenseBullet, "GameObject/Bullet/bullet_weapon7");
			defenseBullet.SetProperty(_atk, _speed, _dir);
			defenseBullet.tran.localPosition = _shootPoint.position;
		}

		protected override void Operation(ShooterVO vo)
		{
			_distance = vo.FireDistance;
			_atk = vo.FirePower;
			_speed = vo.FireScale;
			_dir = ((vo.Dir == 1f) ? 1 : (-1));
		}
	}
}

using UnityEngine;

namespace orangesGame
{
	public class ShooterAngle : Shooter
	{
		private float _angleMin;

		private float _angleMax;

		private float _moveDistance;

		public override void Fire()
		{
			FireBallBullet fireBallBullet = Singleton<BulletManager>.Instance.Spawn<FireBallBullet>(BulletType.FireBall, "GameObject/Bullet/bullet_weapon2");
			fireBallBullet.SetProperty(_atk, _buttleSpeed);
			fireBallBullet.tran.localPosition = _shootPoint.position;
			float num = Random.Range(_angleMin, _angleMax);
			fireBallBullet.SetMoveSpeed(num * _dir);
			fireBallBullet.ScaleTween();
			Singleton<AudioManager>.Instance.PlaySound("fu2");
		}

		protected override void Operation(ShooterVO vo)
		{
			_distance = vo.FireDistance;
			_atk = vo.FirePower;
			_buttleSpeed = vo.BulletMoveSpeed;
			_dir = vo.Dir;
			_angleMin = vo.AngleMin;
			_angleMax = vo.AngleMax;
		}
	}
}

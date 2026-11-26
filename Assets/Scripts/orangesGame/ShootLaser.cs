using UnityEngine;

namespace orangesGame
{
	public class ShootLaser : Shooter
	{
		private float _laseWide;

		public override void Fire()
		{
			LaserBullet laserBullet = Singleton<BulletManager>.Instance.Spawn<LaserBullet>(BulletType.Lase, "GameObject/Bullet/bullet_weapon3");
			laserBullet.SetProperty(_atk);
			Transform tran = laserBullet.tran;
			Vector3 position = _shootPoint.position;
			float x = position.x;
			Vector3 position2 = _shootPoint.position;
			tran.localPosition = new Vector3(x, position2.y);
			laserBullet.SetSize(_laseWide);
			Singleton<AudioManager>.Instance.PlaySpeciSound("fu3");
		}

		protected override void Operation(ShooterVO vo)
		{
			_distance = vo.FireDistance;
			_atk = vo.FirePower;
			_laseWide = vo.FireScale;
		}
	}
}

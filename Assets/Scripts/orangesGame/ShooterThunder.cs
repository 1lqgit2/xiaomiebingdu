using UnityEngine;

namespace orangesGame
{
	public class ShooterThunder : Shooter
	{
		private float _scale;

		private float _netScale;

		private BulletType _type = BulletType.Thunder;

		private string _path = "GameObject/Bullet/bullet_weapon5";

		public override void Fire()
		{
			for (int i = 0; i < 2; i++)
			{
				ThunderBullet thunderBullet = Singleton<BulletManager>.Instance.Spawn<ThunderBullet>(_type, _path);
				thunderBullet.SetProperty(_atk, _scale, _netScale, _distance, (i != 0) ? (-45f) : 45f);
				Transform tran = thunderBullet.tran;
				Vector3 position = _shootPoint.position;
				float x = position.x;
				Vector3 position2 = _shootPoint.position;
				tran.localPosition = new Vector3(x, position2.y);
				thunderBullet.OnStart();
			}
		}

		protected override void Operation(ShooterVO vo)
		{
			_distance = vo.FireDistance;
			_atk = vo.FirePower;
			_scale = vo.FireScale;
			_netScale = vo.NetScale;
		}
	}
}

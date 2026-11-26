using UnityEngine;

namespace orangesGame
{
	public class ShooterManager : Singleton<ShooterManager>
	{
		public Shooter Spawn(ShooterVO vo, Transform parent)
		{
			switch (vo.Type)
			{
			case ShooterType.Change:
				return Spawn<ShooterChange>(vo, parent);
			case ShooterType.Super:
				return Spawn<ShooterSuper>(vo, parent);
			case ShooterType.GreenBall:
				return Spawn<ShooterGreenBall>(vo, parent);
			case ShooterType.Angle:
				return Spawn<ShooterAngle>(vo, parent);
			case ShooterType.Laser:
				return Spawn<ShootLaser>(vo, parent);
			case ShooterType.Follow:
				return Spawn<ShooterFollow>(vo, parent);
			case ShooterType.Thunder:
				return Spawn<ShooterThunder>(vo, parent);
			case ShooterType.Defense:
				return Spawn<ShootDefense>(vo, parent);
			default:
				return null;
			}
		}

		private T Spawn<T>(ShooterVO vo, Transform parent) where T : Shooter, new()
		{
			T result = new T();
			result.Init(vo, parent);
			return result;
		}
	}
}

using UnityEngine;

namespace orangesGame
{
	public abstract class Shooter
	{
		protected float _distance;

		protected float _fireCountFrame;

		protected float _buttleSpeed;

		protected float _atk;

		protected float _dir;

		protected float _fireOffset;

		protected float _moveIndex;

		protected Transform _shootPoint;

		public float distance => _distance;

		public Shooter()
		{
		}

		public void Init(ShooterVO vo, Transform parent)
		{
			Reset(vo);
			_shootPoint = parent;
		}

		public void Reset(ShooterVO vo)
		{
			Operation(vo);
		}

		public abstract void Fire();

		protected abstract void Operation(ShooterVO vo);
	}
}

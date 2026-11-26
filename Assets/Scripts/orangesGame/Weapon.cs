using UnityEngine;

namespace orangesGame
{
	public class Weapon : RoleBase
	{
		protected Timer _timer;

		protected Timer _delayTimer;

		protected float _delay;

		protected bool _isInterval;

		public virtual void Init(int index, PlayerVO vo, Transform parent)
		{
			_delay = vo.weapons[index].FireDistanceOffset;
			_isInterval = vo.weapons[index].IsInterval;
			base.Init(vo, parent);
		}

		public override void SetShooting(bool isShooting)
		{
			if (isShooting)
			{
				if (_delay != 0f && _isInterval)
				{
					_delayTimer = Timer.Register(_delay, delegate
					{
						_timer = Timer.Register(_shooter.distance, OnComplete, null, isLooped: true);
					});
				}
				else
				{
					_timer = Timer.Register(_shooter.distance, OnComplete, null, isLooped: true);
				}
			}
			else
			{
				TimerCancel();
			}
		}

		protected override void SetParent(Transform parent, int orderInLayer = -1)
		{
			int sortingOrder = parent.parent.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
			base.SetParent(parent, sortingOrder);
		}

		public virtual void OnComplete()
		{
			_shooter.Fire();
		}

		protected virtual void TimerCancel()
		{
			if (_delayTimer != null && !_delayTimer.isCancelled)
			{
				_delayTimer.Cancel();
				_delayTimer = null;
			}
			if (_timer != null && !_timer.isCancelled)
			{
				_timer.Cancel();
				_timer = null;
			}
		}
	}
}

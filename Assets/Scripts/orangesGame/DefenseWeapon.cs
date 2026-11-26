using System;
using UnityEngine;

namespace orangesGame
{
	public class DefenseWeapon : Weapon
	{
		public SpriteRenderer sprBullet;

		private void Awake()
		{
			InitAwake();
		}

		public override void Init(int index, PlayerVO vo, Transform parent)
		{
			vo.weapons[index].Dir = index;
			base.Init(index, vo, parent);
		}

        public override void OnComplete()
		{
			base.OnComplete();
			sprBullet.enabled = false;
		}

		protected override void TimerCancel()
		{
			base.TimerCancel();
			sprBullet.enabled = true;
		}

		public override void SetShooting(bool isShooting)
		{
			if (isShooting)
			{
				if (_delay != 0f && _isInterval)
				{
					_delayTimer = Timer.Register(_delay, delegate
					{
						_timer = Timer.Register(_shooter.distance, (Action)((Weapon)this).OnComplete, (Action<float>)null, isLooped: false, useRealTime: false, (MonoBehaviour)null);
					});
				}
				else
				{
					_timer = Timer.Register(_shooter.distance, (Action)((Weapon)this).OnComplete, (Action<float>)null, isLooped: false, useRealTime: false, (MonoBehaviour)null);
				}
			}
			else
			{
				TimerCancel();
			}
		}
	}
}

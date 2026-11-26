using UnityEngine;

namespace orangesGame
{
	public class FollowRocketWeapon : Weapon
	{
		public GameObject ObjRocket;

		private const float SHOW_BULLET_DISTANCE = 2f;

		private Timer _showBulletTimer;

		private void Awake()
		{
			InitAwake();
		}

		public override void Init(int index, PlayerVO vo, Transform parent)
		{
			base.Init(index, vo, parent);
		}

        public override void OnComplete()
		{
			base.OnComplete();
			ObjRocket.SetActive(value: false);
			if (_showBulletTimer != null && !_showBulletTimer.isCancelled)
			{
				_showBulletTimer.Cancel();
			}
			_showBulletTimer = Timer.Register(2f, SetBulletActive);
		}

		protected override void TimerCancel()
		{
			base.TimerCancel();
			if (_showBulletTimer != null && !_showBulletTimer.isCancelled)
			{
				_showBulletTimer.Cancel();
				SetBulletActive();
			}
		}

		private void SetBulletActive()
		{
			ObjRocket.SetActive(value: true);
		}
	}
}

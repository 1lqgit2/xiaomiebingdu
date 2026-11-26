using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class GreenBallWeapon : Weapon
	{
		public Transform tranBullet;

		private void Awake()
		{
			InitAwake();
		}

		public override void Init(int index, PlayerVO vo, Transform parent)
		{
			base.Init(index, vo, parent);
			TranBulletIdel();
		}

        public override void OnComplete()
		{
			base.OnComplete();
			tranBullet.gameObject.SetActive(value: false);
		}

		protected override void TimerCancel()
		{
			base.TimerCancel();
			tranBullet.gameObject.SetActive(value: true);
		}

		private void TranBulletIdel()
		{
			tranBullet.DOKill();
			tranBullet.DORotate(new Vector3(0f, 0f, 200f), 0.5f).SetLoops(-1, LoopType.Incremental);
		}
	}
}

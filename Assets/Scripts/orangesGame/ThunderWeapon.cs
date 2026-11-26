using System;
using UnityEngine;

namespace orangesGame
{
	public class ThunderWeapon : Weapon
	{
		public Animator anim;

		private const string ANIM_SCALE = "anim_weapon_5_rotate";

		private int _index;

		private void Awake()
		{
			InitAwake();
		}

		public override void Init(int index, PlayerVO vo, Transform parent)
		{
			base.Init(index, vo, parent);
			_index = index;
		}

		public override void SetShooting(bool isShooting)
		{
			if (isShooting)
			{
				_timer = Timer.Register(_shooter.distance, (Action)((Weapon)this).OnComplete, (Action<float>)null, isLooped: true, useRealTime: false, (MonoBehaviour)null);
			}
			else
			{
				TimerCancel();
			}
		}

        public override void OnComplete()
		{
			anim.Play("anim_weapon_5_rotate");
		}

		public void Fire()
		{
			if (_index != 1)
			{
				_shooter.Fire();
			}
		}
	}
}

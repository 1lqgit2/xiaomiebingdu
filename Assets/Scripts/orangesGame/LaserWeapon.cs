using UnityEngine;

namespace orangesGame
{
	public class LaserWeapon : Weapon
	{
		public AnimatorWidget anim;

		public GameObject shooterThunder;

		public Transform point;

		private int _index;

		private const string ANIM_NAME = "anim_weapon_3_point";

		private const string IDEL = "Idel";

		private void Awake()
		{
			InitAwake();
		}

		public override void Init(int index, PlayerVO vo, Transform parent)
		{
			base.Init(index, vo, parent);
			_index = index;
			if (index == 1)
			{
				shooterThunder.gameObject.SetActive(value: false);
			}
		}

		public override void SetShooting(bool isShooting)
		{
			if (isShooting)
			{
				if (_index != 1)
				{
					if (_delay != 0f && _isInterval)
					{
						_delayTimer = Timer.Register(_delay, delegate
						{
							_timer = Timer.Register(_shooter.distance, OnComplete, null, isLooped: true);
							anim.Play("anim_weapon_3_point");
						});
						return;
					}
					_timer = Timer.Register(_shooter.distance, OnComplete, null, isLooped: true);
					anim.Play("anim_weapon_3_point");
				}
			}
			else
			{
				TimerCancel();
				Clean();
			}
		}

		private void Clean()
		{
			anim.Play("Idel");
			point.localScale = Vector3.one * 0.2f;
		}
	}
}

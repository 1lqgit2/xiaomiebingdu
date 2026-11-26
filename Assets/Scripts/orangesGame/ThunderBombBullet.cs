using UnityEngine;

namespace orangesGame
{
	public class ThunderBombBullet : Bullet
	{
		public Collider2D range;

		public Animator anim;

		private const string NAME = "anim_bullet_bomb_5";

		private void Awake()
		{
			AwakeInit();
		}

		public void SetProperty(float atk, float scale)
		{
			if (_atk != atk)
			{
				_atk = atk;
			}
			Vector3 localScale = _tran.localScale;
			if (localScale.x != scale)
			{
				_tran.localScale = Vector2.one * scale;
			}
			anim.Play("anim_bullet_bomb_5");
		}

		public override void OnTriggerLogic(float ballValue = 0f)
		{
			_atk = 0f;
		}

		public override void Clean()
		{
			base.Clean();
		}
	}
}

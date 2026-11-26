using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class GreenBallBullet : Bullet
	{
		public SpriteRenderer spriteRenderer;

		private float _count;

		private float _wideHalf;

		private const string TAG_BARR = "Player";

		private float _angle = 20f;

		private void Awake()
		{
			AwakeInit();
			float num = spriteRenderer.sprite.textureRect.width / 100f;
			_wideHalf = num / 2f;
			_count = 3f;
		}

		private void Update()
		{
			if (!isDirty)
			{
				Moving();
			}
		}

		public override void Init()
		{
			base.Init();
			spriteRenderer.transform.DOBlendableRotateBy(new Vector3(0f, 0f, 30f), 1f).SetLoops(-1, LoopType.Incremental);
		}

		public override void OnTriggerLogic(float ballValue)
		{
			_count -= 1f;
			if (_count <= 0f)
			{
				Clean();
			}
		}

		public override void Clean()
		{
			_count = 3f;
			_tran.DOKill();
			spriteRenderer.transform.DOKill();
			base.Clean();
		}

		private void Moving()
		{
			_tran.localPosition += _curMoveSpeed * Time.deltaTime;
			Vector3 localPosition = _tran.localPosition;
			if (localPosition.y >= 9.6f + _wideHalf)
			{
				Clean();
				return;
			}
			Vector3 localPosition2 = _tran.localPosition;
			if (localPosition2.x < -5.4f + _wideHalf)
			{
				SetMoveSpeed(0f - _angle);
				return;
			}
			Vector3 localPosition3 = _tran.localPosition;
			if (localPosition3.x > 5.4f - _wideHalf)
			{
				SetMoveSpeed(_angle);
			}
		}

		public void SetMoveSpeed(float dir)
		{
			Vector2 v = Quaternion.Euler(0f, 0f, _angle * dir) * Vector2.up * _speed;
			_curMoveSpeed = v;
		}

		public void SetProperty(float atk, float scale, float speed)
		{
			if (_atk != atk)
			{
				_atk = atk;
				_speed = speed;
			}
			Vector3 localScale = _tran.localScale;
			if (localScale.x != scale)
			{
				_tran.localScale = Vector2.one * scale;
				float wideHalf = _wideHalf;
				Vector3 localScale2 = _tran.localScale;
				_wideHalf = wideHalf * localScale2.x;
			}
		}

		public override void PlaySound()
		{
			Singleton<AudioManager>.Instance.PlaySpeciSound("fu1");
		}
	}
}

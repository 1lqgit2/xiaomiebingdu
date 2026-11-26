using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class FireBallBullet : Bullet
	{
		public SpriteRenderer sprRenderer;

		private float _wideHalf;

		private float _topPos;

		private Vector2 _scaleTo;

		private Vector2 _scaleFrom;

		private float _moveDistance;

		private IEffectAction _hitAction;

		private void Awake()
		{
			AwakeInit();
			float num = sprRenderer.sprite.textureRect.width / 100f;
			_wideHalf = num / 2f;
			_topPos = 9.6f + _wideHalf;
			_scaleTo = Vector2.one * 0.5f;
			_scaleFrom = Vector2.one * 1.2f;
			_hitAction = Singleton<EffectManager>.Instance.GetEffAction(3u);
		}

		private void Update()
		{
			if (!isDirty)
			{
				Moving();
			}
		}

		public override void Clean()
		{
			_tran.DOKill();
			_moveDistance = 0f;
			base.Clean();
		}

		public override void OnTriggerLogic(float ballValue = 0f)
		{
			Clean();
		}

		public void SetProperty(float atk, float speed)
		{
			if (_atk != atk)
			{
				_atk = atk;
				_speed = speed;
			}
		}

		public void ScaleTween()
		{
			_tran.localScale = _scaleTo;
			_tran.DOScale(_scaleFrom, 0.2f).SetAutoKill(autoKillOnCompletion: true).SetAutoKill(autoKillOnCompletion: true);
		}

		public void SetMoveSpeed(float angle)
		{
			_tran.localRotation = Quaternion.Euler(0f, 0f, angle);
			Vector2 v = Quaternion.Euler(0f, 0f, angle) * Vector2.up * _speed;
			_curMoveSpeed = v;
		}

		private void Moving()
		{
			Vector3 vector = _curMoveSpeed * Time.deltaTime;
			_tran.localPosition += vector;
			_moveDistance += vector.y;
			if (_moveDistance >= 7f)
			{
				_hitAction.Spawn(_tran.localPosition);
				Clean();
				return;
			}
			Vector3 localPosition = _tran.localPosition;
			if (localPosition.y >= _topPos)
			{
				Clean();
				return;
			}
			Vector3 localPosition2 = _tran.localPosition;
			if (localPosition2.x < -5.4f)
			{
				Clean();
				return;
			}
			Vector3 localPosition3 = _tran.localPosition;
			if (localPosition3.x > 5.4f)
			{
				Clean();
			}
		}
	}
}

using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class BoomerangBullet : Bullet
	{
		public SpriteRenderer sprRenderer;

		private bool _isBack;

		private float _borderTop;

		private Transform _player;

		private void Awake()
		{
			AwakeInit();
			float num = sprRenderer.sprite.textureRect.width * 0.005f;
			_isBack = false;
			_borderTop = Singleton<UIViewManager>.Instance.originalHeightHalf;
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
			sprRenderer.transform.DOKill();
			sprRenderer.transform.localRotation = Quaternion.identity;
			base.Clean();
		}

		public void SetProperty(float atk, float scale, float speed, Transform tranShoot)
		{
			if (_atk != atk)
			{
				_atk = atk;
				_speed = speed;
				_tran.localScale = Vector2.one * scale;
			}
			_curMoveSpeed = new Vector3(0f, _speed, 0f);
			_isBack = false;
			_player = tranShoot;
			sprRenderer.transform.DORotate(new Vector3(0f, 0f, 200f), 0.1f).SetLoops(-1, LoopType.Incremental);
		}

		private void Moving()
		{
			if (_isBack)
			{
				if (_player != null)
				{
					Vector2 to = (_player.position - base.tran.localPosition).normalized;
					float num = Vector2.Angle(Vector2.up, to);
					Vector3 localPosition = base.tran.localPosition;
					float x = localPosition.x;
					Vector3 position = _player.position;
					if (x < position.x)
					{
						num = 0f - num;
					}
					_curMoveSpeed = Quaternion.Euler(0f, 0f, num) * Vector2.up * _speed * Time.deltaTime;
					_tran.localPosition += _curMoveSpeed;
				}
				if (Vector2.Distance(_player.position, base.tran.localPosition) <= 0.5f)
				{
					Clean();
				}
			}
			else
			{
				Vector3 localPosition2 = base.tran.localPosition;
				if (localPosition2.y >= _borderTop)
				{
					_isBack = true;
				}
				_tran.localPosition += _curMoveSpeed * Time.deltaTime;
			}
		}
	}
}

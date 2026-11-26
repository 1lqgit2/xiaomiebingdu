using UnityEngine;

namespace orangesGame
{
	public class ThunderBullet : Bullet
	{
		private float _wideHalf;

		private float _netScale;

		private float _startAngle;

		private float _tempPlayerShootPos;

		private void Awake()
		{
			AwakeInit();
			float num = _renderers[0].sprite.textureRect.width / 100f;
			_wideHalf = num / 2f;
		}

		private void Update()
		{
			if (!isDirty)
			{
				Moving();
			}
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
			if (localPosition2.x < _tempPlayerShootPos - 1f)
			{
				_startAngle = 0f - _startAngle;
				SetMoveSpeed(_startAngle);
				return;
			}
			Vector3 localPosition3 = _tran.localPosition;
			if (localPosition3.x > _tempPlayerShootPos + 1f)
			{
				_startAngle = 0f - _startAngle;
				SetMoveSpeed(_startAngle);
			}
		}

		public void OnStart()
		{
			Vector3 position = base.tran.position;
			_tempPlayerShootPos = position.x;
			SetMoveSpeed(_startAngle);
		}

		public void SetMoveSpeed(float angle)
		{
			Vector2 v = Quaternion.Euler(0f, 0f, angle) * Vector2.up * _speed;
			_curMoveSpeed = v;
		}

		public void SetProperty(float atk, float scale, float netScale, float distance, float startAngle)
		{
			if (_atk != atk)
			{
				_atk = atk;
				_speed = distance;
			}
			Vector3 localScale = _tran.localScale;
			if (localScale.x != scale)
			{
				_netScale = netScale;
				_tran.localScale = Vector2.one * scale;
				float wideHalf = _wideHalf;
				Vector3 localScale2 = _tran.localScale;
				_wideHalf = wideHalf * localScale2.x;
			}
			_startAngle = startAngle;
		}

		public override void OnTriggerLogic(float ballValue = 0f)
		{
			Bombing();
			Singleton<AudioManager>.Instance.PlaySpeciSound("fu5");
			Clean();
		}

		private void Bombing()
		{
			ThunderBombBullet thunderBombBullet = Singleton<BulletManager>.Instance.Spawn<ThunderBombBullet>(BulletType.ThunderBomb, "GameObject/Bullet/bullet_weapon5_bomb");
			thunderBombBullet.SetProperty(_atk, _netScale);
			thunderBombBullet.tran.localPosition = _tran.localPosition;
		}
	}
}

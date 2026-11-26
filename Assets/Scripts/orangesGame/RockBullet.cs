using System;
using UnityEngine;

namespace orangesGame
{
	public class RockBullet : Bullet
	{
		private const string VENOM = "Venom";

		private float _curTime;

		private float _speedOffset;

		private Action _logicAction;

		private Transform _follow;

		private const float DELAYTIME = 0.5f;

		private float _curSpeed;

		private float _bombAtk;

		private float _bombScale;

		private void Awake()
		{
			AwakeInit();
		}

		private void Update()
		{
			if (!isDirty && _logicAction != null)
			{
				_logicAction();
			}
		}

		public void SetProperty(float atk, float scale, float speed)
		{
			if (_bombAtk != atk)
			{
				_bombAtk = atk;
				_bombScale = scale;
				_speed = speed;
				_speedOffset = _speed * 0.1f;
			}
		}

		public void StartFollow(float dir)
		{
			_tran.localRotation = Quaternion.identity;
			_logicAction = StateStart;
		}

		public override void OnTriggerLogic(float ballValue = 0f)
		{
			Bombing();
			Singleton<AudioManager>.Instance.PlaySpeciSound("fu4");
			Clean();
		}

		public override void Clean()
		{
			_curTime = 0f;
			_atk = 0f;
			_curSpeed = 0f;
			_follow = null;
			base.Clean();
		}

		private void Bombing()
		{
			RockBombBullet rockBombBullet = Singleton<BulletManager>.Instance.Spawn<RockBombBullet>(BulletType.RocketBomb, "GameObject/Bullet/bullet_weapon4_bomb");
			rockBombBullet.SetProperty(_bombAtk, _bombScale);
			rockBombBullet.OnStart(_tran.localPosition);
		}

		private void Moving()
		{
			Vector3 localPosition = _tran.localPosition;
			if (!(localPosition.y >= 9.6f))
			{
				Vector3 localPosition2 = _tran.localPosition;
				if (!(localPosition2.y <= -9.6f))
				{
					_curTime += Time.deltaTime;
					if (_follow != null && _follow.gameObject.activeSelf)
					{
						Vector2 to = (_follow.localPosition - base.tran.localPosition).normalized;
						float num = Vector2.Angle(Vector2.up, to);
						Vector3 localPosition3 = base.tran.localPosition;
						float x = localPosition3.x;
						Vector3 localPosition4 = _follow.localPosition;
						if (x < localPosition4.x)
						{
							num = 0f - num;
						}
						_tran.localRotation = Quaternion.Lerp(_tran.localRotation, Quaternion.Euler(0f, 0f, num), _curTime * 0.1f);
					}
					else
					{
						_follow = Singleton<BallManger>.Instance.GetBarrierTran();
						if (_follow != null)
						{
							_curTime = 0f;
						}
						else
						{
							_curTime = 1f;
						}
					}
					_curSpeed = Mathf.Lerp(_curSpeed, _speed, _curTime);
					_curMoveSpeed = _tran.localRotation * Vector2.up * _curSpeed * Time.deltaTime;
					_tran.localPosition += _curMoveSpeed;
					return;
				}
			}
			Clean();
		}

		private void StateStart()
		{
			_curTime += Time.deltaTime;
			if (_curTime >= 0.5f)
			{
				_curTime = 0f;
				_logicAction = StateClimb;
			}
		}

		private void StateClimb()
		{
			_curTime += Time.deltaTime;
			_curSpeed = Mathf.Lerp(_curSpeed, _speedOffset, _curTime);
			if (_curSpeed >= _speedOffset)
			{
				_curTime = 0f;
				_follow = Singleton<BallManger>.Instance.GetBarrierTran();
				_logicAction = Moving;
			}
			_curMoveSpeed = _tran.localRotation * Vector2.up * _curSpeed * Time.deltaTime;
			_tran.localPosition += _curMoveSpeed;
		}
	}
}

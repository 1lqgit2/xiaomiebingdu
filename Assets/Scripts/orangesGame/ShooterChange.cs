using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class ShooterChange : Shooter
	{
		protected float _curFireOffset;

		protected float _curfireCountFrame;

		private float _power;

		private float bulletMoveEndY = 11f;

		public override void Fire()
		{
			_curFireOffset += _fireOffset;
			if (_curFireOffset > 1f)
			{
				_curFireOffset -= 1f;
				_curfireCountFrame = _fireCountFrame + 1f;
			}
			else
			{
				_curfireCountFrame = _fireCountFrame;
			}
			_moveIndex = _curfireCountFrame - 1f;
			for (float num = 0f; num < _curfireCountFrame; num += 1f)
			{
				Move(num);
			}
		}

		private void Move(float index)
		{
			SmallBullet buttle = Singleton<BulletManager>.Instance.Spawn<SmallBullet>(GameGlobalData.PlayerBulletType, GameGlobalData.PlayerBulletResPath);
			buttle.SetPropter(_atk);
			Vector2 v = _shootPoint.position;
			buttle.tran.localPosition = v;
			buttle.tran.localRotation = Quaternion.identity;
			float num = index * buttle.wide;
			float num2 = _moveIndex * buttle.wideHalf;
			Vector2 v2 = new Vector2(v.x - num2 + num, v.y + 1.5f);
			float num3 = bulletMoveEndY;
			Vector3 position = buttle.tran.position;
			float num4 = num3 - position.y;
			float moveTime = num4 / (_buttleSpeed * 100f);
			buttle.tran.DOMove(v2, 0.1f).OnComplete(delegate
			{
				Transform tran = buttle.tran;
				Vector3 localPosition = buttle.tran.localPosition;
				Tweener t = tran.DOMove(new Vector2(localPosition.x, bulletMoveEndY), moveTime);
				SmallBullet smallBullet = buttle;
				t.OnComplete(((Bullet)smallBullet).Clean);
			});
		}

		protected override void Operation(ShooterVO vo)
		{
			_atk = vo.FirePower;
			_buttleSpeed = vo.BulletMoveSpeed;
			_distance = 0.8f / vo.FireBulletCount;
			if (_distance >= vo.FireSpeedMax)
			{
				_fireCountFrame = 1f;
				return;
			}
			_fireCountFrame = vo.FireSpeedMax / _distance;
			float num = Mathf.Floor(_fireCountFrame);
			_fireOffset = _fireCountFrame - num;
			_fireCountFrame = num;
			_curFireOffset = 0f;
			_distance = vo.FireSpeedMax;
		}
	}
}

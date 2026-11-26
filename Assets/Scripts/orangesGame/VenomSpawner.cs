using DG.Tweening;
using UnityEngine;

namespace orangesGame
{
	public class VenomSpawner : VenomCommon
	{
		public Transform Body;

		public Transform Head;

		protected float _spawnPeriod = 5f;

		protected int _divideID = 16;

		protected float _spawningTimeInterval = 0.5f;

		protected int _spawnCount = 6;

		protected int _spawnPeriodCount = 2;

		protected float _spawningSpeed = 1f;

		protected float _preMoveTime = 0.17f;

		protected float _pausePreMoveTime = 0.8f;

		protected float _preMoveSpeed = 1f;

		protected float _spawnRandomAngle = 45f;

		protected bool _isSpawningPaused;

		protected Sequence m_preMoveSeq;

		protected Sequence m_spawningSeq;

		protected float _parallelAngleLimit = 5f;

		protected override void InitState(BallVO ballVO)
		{
			base.InitState(ballVO);
			_spawningSpeed = 1f;
			if (_ballVO.DivideCount != 1)
			{
				SpawnChildren();
			}
			_preMoveSpeed = 1f;
		}

		protected virtual void SpawnChildren()
		{
			Sequence sequence = DOTween.Sequence();
			sequence.AppendInterval(_spawnPeriod);
			for (int i = 0; i < _spawnCount; i++)
			{
				sequence.AppendCallback(SpawnChild);
				sequence.AppendInterval(_spawningTimeInterval);
			}
			sequence.SetLoops(_spawnPeriodCount).SetEase(Ease.Linear);
			m_spawningSeq = sequence;
		}

		protected virtual void SpawnChild()
		{
			if (!checkBounds())
			{
				VenomSpawner venomSpawner = Singleton<BallManger>.Instance.CreateOne(Head.position, _divideID, Mathf.Ceil(_ballVO.HitCount * 0.05f), 1) as VenomSpawner;
				Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(0f - _spawnRandomAngle, _spawnRandomAngle));
				Vector2 vector = _curMoveSpeed.normalized * venomSpawner.GetInitMoveSpeed.magnitude;
				Vector2 speed = rotation * venomSpawner.GetInitMoveSpeed;
				venomSpawner.SetMoveSpeed(Quaternion.identity, speed);
			}
		}

		protected override void SetRotate()
		{
			float duration = 2 * _ballVO.DivideCount;
			DOTween.Kill(RotateQiu);
			RotateQiu.transform.DORotate(RotateQiu.transform.rotation.eulerAngles + new Vector3(0f, 0f, 360f), duration, RotateMode.FastBeyond360).SetLoops(-1);
		}

		protected override void Update()
		{
			base.Update();
			Body.transform.up = Vector3.Slerp(Body.transform.up, -_curMoveSpeed, Time.deltaTime * 20f);
		}

		protected override void SetMovingPos(Vector3 pos)
		{
			base.SetMovingPos(pos * _spawningSpeed * _preMoveSpeed);
		}

		public override void Pause()
		{
			base.Pause();
			if (m_preMoveSeq != null && m_preMoveSeq.IsActive())
			{
				m_preMoveSeq.Pause();
			}
			if (m_spawningSeq != null && m_spawningSeq.IsActive())
			{
				m_spawningSeq.Pause();
			}
			_isSpawningPaused = true;
		}

		public override void Continue()
		{
			base.Continue();
			if (m_preMoveSeq != null && m_preMoveSeq.IsActive())
			{
				m_preMoveSeq.Play();
			}
			if (m_spawningSeq != null && m_spawningSeq.IsActive())
			{
				m_spawningSeq.Play();
			}
			_isSpawningPaused = false;
		}

		private bool checkBounds()
		{
			Vector3 localPosition = _tran.localPosition;
			if (localPosition.y < 0f - Singleton<UIViewManager>.Instance.screenHeightHalf + Singleton<UIViewManager>.Instance.screenHeight / 5f)
			{
				return true;
			}
			return false;
		}

		protected override void OnDie()
		{
			base.OnDie();
			if (m_preMoveSeq != null)
			{
				m_preMoveSeq.Kill();
			}
			m_preMoveSeq = null;
			if (m_spawningSeq != null)
			{
				m_spawningSeq.Kill();
			}
			m_spawningSeq = null;
		}
	}
}

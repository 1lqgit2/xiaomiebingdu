using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace orangesGame
{
	public class Wingman : RoleBase
	{
		public Vector2 InitPos = new Vector2(0f, -13f);

		public Vector2 StartPos = new Vector2(0f, -5f);

		public float CheckInTime = 0.7f;

		public float ApproachLeaderRadius = 1f;

		public float ApproachSpeed = 5f;

		private Transform _leader;

		private WaitForSeconds _waitForSeconds;

		private Vector2 _cachedPos;

		private bool _isCheckingIn;

		private float _cacheSpeed;

		private float _speed;

		private Vector2 _normalized;

		private Tweener _stopTweener;

		private void Awake()
		{
			InitAwake();
		}

		private void Update()
		{
			if (_leader != null && !_isCheckingIn)
			{
				float num = ApproachSpeed;
				float sqrMagnitude = ((Vector2)(base.tran.localPosition - _leader.transform.localPosition)).sqrMagnitude;
				float num2 = ApproachLeaderRadius * ApproachLeaderRadius;
				if (!(sqrMagnitude > num2))
				{
					num = ((!(sqrMagnitude > num2 / 2f)) ? 0f : (num * 0.01f));
				}
				_normalized = _leader.transform.localPosition.normalized;
				_cachedPos = _leader.transform.localPosition;
				_cacheSpeed = num;
				_tran.localPosition = Vector2.Lerp(_tran.localPosition, _normalized * 2f + _cachedPos, Time.deltaTime * _speed);
			}
			_speed = Mathf.Lerp(_speed, _cacheSpeed, Time.deltaTime);
		}

		public override void Init(PlayerVO vo, Transform parent)
		{
			SetParent(parent);
			_tran.localPosition = InitPos;
			_speed = ApproachSpeed;
			_isCheckingIn = true;
			if (_stopTweener != null && _stopTweener.IsActive())
			{
				_stopTweener.Kill();
				_stopTweener = null;
			}
			_tran.DOLocalMove(StartPos, CheckInTime).OnComplete(delegate
			{
				_isCheckingIn = false;
			}).SetEase(Ease.InQuad);
			_leader = Singleton<PlayerManager>.Instance.GetLeader();
		}

		public override void SetShooter(ShooterVO vo, bool isChange = false)
		{
			_shooter = Singleton<ShooterManager>.Instance.Spawn(vo, _shooterTran);
			_waitForSeconds = new WaitForSeconds(_shooter.distance);
			SetShooting(isPlay: true);
		}

		public void Stop()
		{
			SetShooting(isPlay: false);
			_stopTweener = _tran.DOMove(InitPos, CheckInTime).SetEase(Ease.OutQuad).OnComplete(delegate
			{
				base.gameObject.SetActive(value: false);
			});
		}

		public new void SetShooting(bool isPlay)
		{
			if (isPlay)
			{
				if (GameGlobalData.GameState != GameState.Pass && GameGlobalData.GameState != 0 && base.gameObject.activeSelf)
				{
					objFire.SetActive(value: true);
					StartCoroutine(ShootCoroutines());
				}
			}
			else
			{
				objFire.SetActive(value: false);
				StopAllCoroutines();
			}
		}

		private IEnumerator ShootCoroutines()
		{
			while (true)
			{
				_shooter.Fire();
				yield return _waitForSeconds;
			}
		}
	}
}

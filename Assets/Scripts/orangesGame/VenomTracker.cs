using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class VenomTracker : VenomCommon
	{
		public VenomScanner Scanner;

		public Sprite TentacleSp;

		public Transform TentacleTran;

		public int TentacleCount = 16;

		public float NormalStateRadius = 0.4f;

		public float TrackedStateRadius = 0.65f;

		public float AnimDuration = 0.3f;

		public float AnimInterval = 0.1f;

		protected float ScanSmallestRadius = 4f;

		protected bool _isTentaclesInited;

		protected Player _target;

		protected List<SpriteRenderer> _tentacleRenderers = new List<SpriteRenderer>();

		protected Vector2 _speedCache;

		protected bool _isPauseTracking;

		protected Sequence _pauseTrackingTimer;

		[SerializeField]
		protected Transform TestScannerRadius;

		[SerializeField]
		protected bool IsShowScannerRadius;

		public bool GetIsTracked => _target != null;

		protected override void Awake()
		{
			base.Awake();
			Scanner.EventOnTargetStay.AddListener(OnPlayerStay);
			Scanner.EventOnRemoveTarget.AddListener(OnPlayerRemove);
		}

		protected override void InitState(BallVO ballVO)
		{
			InitTentacles();
			base.InitState(ballVO);
			_target = null;
			_isPauseTracking = true;
			_pauseTrackingTimer = DOTween.Sequence();
			_pauseTrackingTimer.AppendInterval(1f);
			_pauseTrackingTimer.AppendCallback(delegate
			{
				_isPauseTracking = false;
			});
		}

		public override void Pause()
		{
			base.Pause();
			if (_pauseTrackingTimer != null && _pauseTrackingTimer.IsActive())
			{
				_pauseTrackingTimer.Pause();
			}
		}

		public override void Continue()
		{
			base.Continue();
			if (_pauseTrackingTimer != null && _pauseTrackingTimer.IsActive())
			{
				_pauseTrackingTimer.Play();
			}
		}

		protected virtual void ClearTentacles()
		{
			IEnumerator enumerator = TentacleTran.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					transform.gameObject.SetActive(value: false);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		protected virtual void InitTentacles()
		{
			float num = (float)Math.PI * 2f / (float)TentacleCount;
			for (int i = 0; i < TentacleCount; i++)
			{
				float x = Mathf.Cos((float)i * num) * NormalStateRadius;
				float y = Mathf.Sin((float)i * num) * NormalStateRadius;
				if (!_isTentaclesInited)
				{
					SpriteRenderer component = new GameObject(i.ToString(), typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
					component.sprite = TentacleSp;
					component.transform.SetParent(TentacleTran);
					component.transform.localScale = Vector3.one * 0.6f;
					component.transform.localPosition = new Vector3(x, y, 0f);
					component.transform.localRotation = Quaternion.Euler(0f, 0f, 57.29578f * num * (float)i - 90f);
					_tentacleRenderers.Add(component);
				}
				else
				{
					Transform child = TentacleTran.GetChild(i);
					child.gameObject.SetActive(value: true);
					child.localPosition = new Vector3(x, y, 0f);
					child.localRotation = Quaternion.Euler(0f, 0f, 57.29578f * num * (float)i - 90f);
				}
			}
			_isTentaclesInited = true;
		}

		protected override void ChangeColor(Color color)
		{
			base.ChangeColor(color);
			foreach (SpriteRenderer tentacleRenderer in _tentacleRenderers)
			{
				tentacleRenderer.color = color;
			}
		}

		protected override void Update()
		{
			base.Update();
			if (Scanner.Radius != ScanSmallestRadius * _circleCollider2D.radius)
			{
				Scanner.Radius = ScanSmallestRadius * _circleCollider2D.radius;
			}
			if (IsShowScannerRadius && (bool)TestScannerRadius)
			{
				TestScannerRadius.localScale = Vector3.one * Scanner.Radius;
				if (!TestScannerRadius.gameObject.activeInHierarchy)
				{
					TestScannerRadius.gameObject.SetActive(value: true);
				}
			}
			if (GetIsTracked && GameGlobalData.GameState == GameState.Gaming)
			{
				_curMoveSpeed = Vector3.Lerp(_curMoveSpeed, _moveSpeed.magnitude * ((Vector2)_target.tran.position - (Vector2)base.transform.position).normalized, Time.deltaTime * 5f);
				_originMoveSpeed = Vector3.Lerp(_originMoveSpeed, _moveSpeed.magnitude * ((Vector2)_target.tran.position - (Vector2)base.transform.position).normalized, Time.deltaTime * 5f);
			}
		}

		protected void OnPlayerStay(GameObject go)
		{
			if (!_isPauseTracking && !GetIsTracked)
			{
				_target = go.GetComponent<Player>();
				if (_target != null)
				{
					PlayFindTargetAnim();
				}
			}
		}

		protected virtual void PlayFindTargetAnim()
		{
			Vector2 to = _target.tran.position - base.transform.position;
			Vector2 from = base.transform.right;
			float num = Vector2.SignedAngle(from, to);
			int num2 = ((int)(num / (360f / (float)TentacleCount)) + TentacleCount) % TentacleCount;
			for (int i = 0; i <= TentacleCount / 2; i++)
			{
				Transform child = TentacleTran.GetChild((num2 + i) % TentacleCount);
				child.DOLocalMove(child.up * TrackedStateRadius, AnimDuration).SetDelay((float)i * AnimInterval);
				child = TentacleTran.GetChild((num2 - i + TentacleCount) % TentacleCount);
				child.DOLocalMove(child.up * TrackedStateRadius, AnimDuration).SetDelay((float)i * AnimInterval);
			}
		}

		protected virtual void PlayMissTargetAnim()
		{
			IEnumerator enumerator = TentacleTran.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					transform.DOKill();
					transform.DOLocalMove(transform.up * NormalStateRadius, AnimDuration);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		private void LateUpdate()
		{
		}

		protected void OnPlayerRemove(GameObject go)
		{
			if (!_isPauseTracking && go.GetComponent<Player>() != null)
			{
				_target = null;
				PlayMissTargetAnim();
			}
		}
	}
}

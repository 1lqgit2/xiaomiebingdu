using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class VenomBlaster : VenomCommon
	{
		public Transform Needle1;

		public Transform Needle2;

		protected float _threshold = 0.1f;

		protected float _explodeRadius = 3f;

		protected Sequence _explodeAnim;

		protected Vector2 _pos;

		protected bool _isExploding;

		private List<SpriteRenderer> _needles;

		protected override void Awake()
		{
			base.Awake();
			_needles = new List<SpriteRenderer>();
			IEnumerator enumerator = Needle1.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					Transform transform2 = transform;
					DOTween.Kill(transform2, complete: true);
					Transform target = transform2;
					Vector3 localScale = transform2.localScale;
					target.DOScaleY(localScale.y - 0.15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
					_needles.Add(transform.GetComponent<SpriteRenderer>());
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
			IEnumerator enumerator2 = Needle2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Transform transform3 = (Transform)enumerator2.Current;
					Transform transform4 = transform3;
					DOTween.Kill(transform4, complete: true);
					Transform target2 = transform4;
					Vector3 localScale2 = transform4.localScale;
					target2.DOScaleY(localScale2.y - 0.15f, 0.5f).SetLoops(-1, LoopType.Yoyo);
					_needles.Add(transform3.GetComponent<SpriteRenderer>());
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
		}

		protected override void InitState(BallVO ballVO)
		{
			base.InitState(ballVO);
			StopAnim();
			_isExploding = false;
		}

		protected override void OnDie()
		{
			base.OnDie();
			Explode();
		}

		protected override void Hitting(Vector2 pos, Bullet bullet)
		{
			if (_isDie || GameGlobalData.IsDie)
			{
				return;
			}
			if (_ballVO.isLaserHit)
			{
				_ballVO.isLaserHit = false;
				return;
			}
			_hitCount -= bullet.atk * GameGlobalData.EnhanceBulletDamageFactor * (float)GameGlobalData.BattleItemData.BattleEnhanceBulletDamageFactor;
			HitEff(bullet, pos);
			if (!_isDie && _hitCount <= 0f)
			{
				SetTextZero();
				StartExplodeAnim();
			}
			else if (_hitCount > 0f)
			{
				SetText();
				SetSprite();
				HurtZoom();
				HitCoin();
			}
		}

		public void StopAnim()
		{
			if (_explodeAnim != null)
			{
				_explodeAnim.Kill();
			}
			_explodeAnim = null;
		}

		public void StartExplodeAnim()
		{
			if (!_isExploding)
			{
				StopAnim();
				if (_ballVO.DivideCount == 1)
				{
					_pos = _tran.position;
					_isExploding = true;
					_explodeAnim = DOTween.Sequence();
					_explodeAnim.Append(_parentTran.DOScale(Vector3.one * 1.35f, 1f).SetEase(Ease.OutQuad));
					_explodeAnim.AppendCallback(Exploding);
					_explodeAnim.OnUpdate(Vibrate);
				}
				else
				{
					ProcessDie();
				}
			}
		}

		protected virtual void Exploding()
		{
			ParticleSystem particleSystem = UnityEngine.Object.Instantiate(Resources.Load<ParticleSystem>("GameEffect/BlasterExplo"));
			particleSystem.transform.SetParent(Singleton<UIViewManager>.Instance.GetLayerTran(40));
			particleSystem.transform.localPosition = _pos;
			ProcessDie();
		}

		public override void Pause()
		{
			base.Pause();
			if (_explodeAnim != null && _explodeAnim.IsActive())
			{
				_explodeAnim.Pause();
			}
		}

		public override void Continue()
		{
			base.Continue();
			if (_explodeAnim != null && _explodeAnim.IsActive())
			{
				_explodeAnim.Play();
			}
		}

		protected void Vibrate()
		{
			if (!_isPaused)
			{
				_tran.position = _pos + UnityEngine.Random.insideUnitCircle * 0.05f;
			}
		}

		protected override void ChangeColor(Color color)
		{
			base.ChangeColor(color);
			foreach (SpriteRenderer needle in _needles)
			{
				needle.color = color;
			}
		}

		public void Explode()
		{
			if (_ballVO.DivideCount != 1)
			{
				return;
			}
			Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, _explodeRadius, 256);
			foreach (Collider2D collider2D in array)
			{
				VenomBlaster component = collider2D.GetComponent<VenomBlaster>();
				if (component != null)
				{
					component.TryExplode();
				}
			}
		}

		public void TryExplode()
		{
			float num = Mathf.Clamp01(_hitCount / _ballVO.HitCount);
			if ((num <= _threshold || _hitCount <= 1f) && !_isDie && !GameGlobalData.IsDie)
			{
				_hitCount = 0f;
				if (!_isDie && _hitCount <= 0f)
				{
					StartExplodeAnim();
				}
			}
		}

		protected override void SetRotate()
		{
			float duration = 4 * _ballVO.DivideCount;
			DOTween.Kill(Needle1);
			Needle1.transform.DORotate(new Vector3(0f, 0f, 360f), duration, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental);
			DOTween.Kill(Needle2);
			Needle2.transform.DORotate(new Vector3(0f, 0f, -360f), duration, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Incremental);
		}
	}
}

using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace orangesGame
{
	public class VenomRegenerationer : VenomCommon
	{
		public Transform CrossTran;

		public float HealPeriod = 0.5f;

		public float HealRating = 0.01f;

		public SpriteRenderer[] CrossRenderers;

		protected float _healFactor = 1f;

		protected float _timer;

		protected override void InitState(BallVO ballVO)
		{
			base.InitState(ballVO);
			_healFactor = 1f;
			_timer = HealPeriod;
		}

		protected override void SetRotate()
		{
			float duration = 2 * _ballVO.DivideCount;
			IEnumerator enumerator = CrossTran.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					transform.DOKill();
					transform.DORotate(transform.transform.rotation.eulerAngles + new Vector3(0f, 0f, 360f), duration, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
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

		protected override void Update()
		{
			base.Update();
			if (!base.GetIsFullHitCount)
			{
				base.GetHealParticle.Play();
			}
			else
			{
				base.GetHealParticle.Stop();
			}
			if (_timer > 0f)
			{
				_timer -= Time.deltaTime;
				return;
			}
			_timer = HealPeriod;
			AddHPRating(HealRating);
		}

		protected override void ChangeColor(Color color)
		{
			base.ChangeColor(color);
			SpriteRenderer[] crossRenderers = CrossRenderers;
			foreach (SpriteRenderer spriteRenderer in crossRenderers)
			{
				spriteRenderer.color = color;
			}
		}
	}
}

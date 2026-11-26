using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace orangesGame
{
	public class VenomSplasher : VenomCommon
	{
		public float RotatePeriod1 = 7f;

		public float RotatePeriod2 = 13f;

		protected override void SetRotate()
		{
			base.SetRotate();
			for (int i = 0; i < base.sprRenderers.Length; i++)
			{
				DOTween.Kill(base.sprRenderers[i]);
				base.sprRenderers[i].transform.DORotate(base.sprRenderers[i].transform.rotation.eulerAngles + new Vector3(0f, 0f, 360 * (-2 * i + 1)), RotatePeriod1, RotateMode.FastBeyond360).SetLoops(-1);
			}
			DOTween.Kill(RotateQiu);
			RotateQiu.transform.DORotate(RotateQiu.transform.rotation.eulerAngles + new Vector3(0f, 0f, 360f), 2 * _ballVO.DivideCount, RotateMode.FastBeyond360).SetLoops(-1);
			SpriteRenderer[] sprRenderers = base.sprRenderers;
			foreach (SpriteRenderer spriteRenderer in sprRenderers)
			{
				IEnumerator enumerator = spriteRenderer.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Transform transform = (Transform)enumerator.Current;
						DOTween.Kill(transform);
						transform.localScale = Vector3.one;
						transform.DOScale(0.5f, 2 + UnityEngine.Random.Range(0, 2)).SetLoops(-1, LoopType.Yoyo);
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
		}

		protected override void StartDivide()
		{
			base.StartDivide();
			VenomCommon venomCommon = Singleton<BallManger>.Instance.CreateOne(_tran.localPosition, _ballVO.DivideID, _ballVO.HitCount / 2f, 0) as VenomCommon;
			VenomCommon venomCommon2 = venomCommon;
			Quaternion identity = Quaternion.identity;
			Vector2 up = Vector2.up;
			BallVO getBallVO = venomCommon.GetBallVO;
			venomCommon2.SetMoveSpeed(identity, up * getBallVO.MoveSpeed);
		}
	}
}
